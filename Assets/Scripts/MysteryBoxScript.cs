using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class MysteryBoxScript : MonoBehaviour {

	float timeSinceCreation = 0f; // used to destroy object after 10 seconds
	Animator animator;
	GameObject player;
	PlayerScript playerScript;
	float randomNumber = 0f; //used to control which powerup the player gets
	Text powerupText;
	int killedBox = -1; // no arrow selected to kill
	// Use this for initialization
	void Start () {
		powerupText = GetComponentInChildren<Text> ();
		powerupText.enabled = false;
		powerupText.GetComponent<Animator> ().enabled = false;
		randomNumber = Random.Range (1, 11);
		animator = GetComponent<Animator> ();
		animator.enabled = false;
		player = GameObject.FindWithTag ("Player");
		playerScript = player.GetComponent<PlayerScript> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(playerScript.gameStarted)
		timeSinceCreation += Time.deltaTime;
	
		if (timeSinceCreation >= 7)
			animator.enabled = true;
		if (timeSinceCreation >= 10f || playerScript.gameEnded)
			Destroy (gameObject);
			
	}

	void OnTriggerEnter2D(Collider2D other){
		
		if (other.tag == "Player") {
			if (randomNumber >= 1 && randomNumber <= 3) {
				if (playerScript.shieldActive == true) {
					playerScript.totalShieldTime = 0f;
				

				} else {
					playerScript.shieldActive = true;


				}
			} else if ((randomNumber ==4 || randomNumber ==5) && killedBox ==-1) {
			//activate arrow kill for 5 seconds
				gameObject.GetComponent<Collider2D> ().enabled = false;

				killedBox = Random.Range(0,4);
				while(playerScript.arrows [killedBox].GetComponent<Arrow> ().arrowKilled)
					killedBox = Random.Range(0,4);

				GetComponentInChildren<Text> ().text = "Arrow Kill!";
				powerupText.enabled = true;


				powerupText.GetComponent<Animator> ().enabled = true;
				powerupText.GetComponent<Animator> ().SetInteger ("KillArrow", 1);

				playerScript.arrows [killedBox].GetComponent<Arrow> ().killArrowFunction ();

			}
				
			else {
				GetComponent<SpriteRenderer> ().enabled = false;
				powerupText.enabled = true;
				powerupText.GetComponent<Animator> ().enabled = true;
				playerScript.freezePowerup += 1;

			}
			StartCoroutine (deathDelay ());
		}
	}

	IEnumerator deathDelay(){
		yield return new WaitForSeconds (0.6f);
		Destroy (gameObject);


	
	}


}
