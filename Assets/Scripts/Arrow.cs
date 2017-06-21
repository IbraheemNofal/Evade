using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {
	public GameObject player;
	float speed = 2.3f; // previous value was 2.5f, then in v1.1 it was 2.4
	public bool gameStarted;
	private Vector3 targetPoint;
	private Quaternion targetRotation;
	PlayerScript playerScript;
	Animator animator;
	public bool gameStartedControl = false; //used so gamestarted value isn't updated on every update
	int i = 0; //used to enter coroutine once
	public Vector3 playerPos;//modify to make arrow move towards target
	float startDelayTime = 0f; //used to keep track of time since game start for delay at beginning
	int numberOfSpeedupsSoFar = 0; //used to limit the speedups to 5
	bool startDelayCalled = false;
	public bool arrowKilled = false;
	bool arrowKilledFinished = true;
	// Use this for initialization
	void Start () {
	    player = GameObject.FindWithTag ("Player");
		playerScript = player.GetComponent<PlayerScript> ();
		gameStarted = false;
		animator = GetComponent<Animator> ();


	}
	
	// Update is called once per frame
	void Update () {

		if (arrowKilled)
			StartCoroutine (killArrow());
		
		if (Input.touchCount > 0 && playerScript.isTouched && !gameStarted && !gameStartedControl) {
			startDelayTime += Time.deltaTime;
			if (!startDelayCalled) {
				startDelayCalled = true;
				StartCoroutine (startDelay ());
			}
		}
		if (gameStarted == true) {
			if (i == 0) {
				i = 1;
				animator.SetInteger ("StopWiggle", 1);
				//Animation animation = animator.GetComponent<Animation> ();
				animator.enabled = false;
				//animation.enabled = false;
				//animator.enabled = false;

			}
			if (numberOfSpeedupsSoFar <1) {

				StartCoroutine (speedUp ());
			}
			//playerPos = player.transform.position;
			//playerPos += new Vector3 (Random.Range (0f, 1.5f), Random.Range (0f, 0.25f), 0);
			Vector3 playerPosForRotation = playerPos;
			playerPos.z = -1;
			//transform.Translate (playerPos*Time.deltaTime*speed);
			Vector3 moveTowards = Vector3.MoveTowards (transform.position, playerPos, speed * Time.deltaTime * 3f);
			moveTowards.z = -1;
			if(!(float.IsNaN(moveTowards.x) || float.IsNaN(moveTowards.y)) && arrowKilledFinished)
			transform.position = moveTowards;

			//rotate object to face player
			Vector3 vectorToTarget = playerPosForRotation - transform.position;
			float angle = Mathf.Atan2 (vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
			Quaternion q = Quaternion.AngleAxis (angle, Vector3.forward);
			if(!(float.IsNaN(vectorToTarget.x) || float.IsNaN(vectorToTarget.y)))
			transform.rotation = Quaternion.Slerp (transform.rotation, q, Time.deltaTime * speed * 2.7f);

			if (transform.position == playerPos ) {
			//if it reaches its destination, follow player
				/*
				 moveTowards = Vector3.MoveTowards (transform.position, playerScript.transform.position, speed * Time.deltaTime * 3f);
				transform.position = moveTowards;
				 vectorToTarget = playerPosForRotation - transform.position;
				 angle = Mathf.Atan2 (vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
				 q = Quaternion.AngleAxis (angle, Vector3.forward);
				transform.rotation = Quaternion.Slerp (transform.rotation, q, Time.deltaTime * speed * 2.7f);
			*/
				GameObject.Find ("background").GetComponent<ArrowController> ().nextPositionRequested (gameObject);
			}
				
			
		} /*else {
			for (float i = 0f; i < 1f; i += 0.05f) {
				Vector3 IdleWiggle = transform.position + new Vector3 (i, i, 0);

				float angle = Mathf.Atan2 (IdleWiggle.y, IdleWiggle.x) * Mathf.Rad2Deg;
				Quaternion q = Quaternion.AngleAxis (angle, Vector3.forward);
				transform.rotation = Quaternion.Slerp (transform.rotation, q, Time.deltaTime * speed * 2.5f);

			}
			for (float i = 1f; i > -1f; i -= 0.05f) {
				Vector3 IdleWiggle = transform.position + new Vector3 (i, i, 0);

				float angle = Mathf.Atan2 (IdleWiggle.y, IdleWiggle.x) * Mathf.Rad2Deg;
				Quaternion q = Quaternion.AngleAxis (angle, Vector3.forward);
				transform.rotation = Quaternion.Slerp (transform.rotation, q, Time.deltaTime * speed * 2.5f);
			
			}
		
		}*/
	}


	IEnumerator speedUp(){
		numberOfSpeedupsSoFar = 1;
		yield return new WaitForSeconds (5f);
		speed += 0.2f;
		yield return new WaitForSeconds (5f);
		speed += 0.1f;
		yield return new WaitForSeconds (5f);
		speed += 0.1f;
		yield return new WaitForSeconds (5f);
		speed += 0.1f;
		yield return new WaitForSeconds (5f);
		speed += 0.2f;
	}

	IEnumerator startDelay(){
		yield return new WaitForSeconds (0.1f);
		gameStarted = true;

	}

	IEnumerator killArrow(){
		
		arrowKilled = false;
		animator.enabled = true;
		animator.SetInteger ("StopWiggle", 1);
		yield return new WaitForSeconds (1f);
		animator.SetInteger ("Fade", 1);// fade out
		yield return new WaitForSeconds (1f); // change value of fade so it doesn't play again
		animator.SetInteger ("Fade", 0);
		yield return new WaitForSeconds (0.25f); // wait for fade to finish
		//animator.enabled = false;
		//this.enabled = false;
		this.GetComponent<Collider2D> ().enabled = false;
		this.GetComponent<SpriteRenderer> ().enabled = false;
		yield return new WaitForSeconds (5f);// killed for 5 seconds
		// fade back in
		//animator.enabled = true;
		//this.enabled = true;
		animator.SetInteger ("Fade", 2);
		this.GetComponent<Collider2D> ().enabled = true;
		this.GetComponent<SpriteRenderer> ().enabled = true;
		yield return new WaitForSeconds (1f);
		animator.SetInteger ("Fade", -1);
		//gameStarted = true;
		//gameStartedControl = false;
		yield return new WaitForSeconds (0.25f);
		animator.enabled = false;
		//transform.rotation = Quaternion.identity;
		arrowKilledFinished = true;
	}

	public void killArrowFunction(){
		arrowKilled = true;
		arrowKilledFinished = false;
	}


}
