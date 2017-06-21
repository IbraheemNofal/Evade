using UnityEngine;
using System.Collections;

public class ArrowController : MonoBehaviour {

	public int[,] grid;
	GameObject[] arrows;
	GameObject player;
	PlayerScript playerScript;
	Arrow arrowScript;
	//float totalTime = 0f;
	Vector3 playerPos;
	Vector3 previousPlayerPos;
	Vector3 velocity = new Vector3(0,0,0);
	//int i=0;
	float timeSinceLastMove = 0f;
	float nextMoveTime = 0f;
	// Use this for initialization
	void Start () {
		grid = new int[5, 3];
		arrows = new GameObject[4];
		player = GameObject.FindWithTag ("Player");
		playerScript=player.GetComponent<PlayerScript> ();
		arrows [0] = GameObject.Find ("arrow");
		arrows [1] = GameObject.Find ("arrow (1)");
		arrows [2] = GameObject.Find ("arrow (2)");
		arrows [3] = GameObject.Find ("arrow (3)");

	}
	
	// Update is called once per frame
	void Update () {
		timeSinceLastMove += Time.deltaTime;
		if (timeSinceLastMove >= nextMoveTime)
			moveTarget ();
		playerPos = player.transform.position;
		playerPos.z = -1;
	
	}

	void moveTarget(){
		nextMoveTime = Random.Range (0.45f, 0.55f);
		timeSinceLastMove = 0f;
			Vector3 deltapos;

			if (Input.touchCount > 0) {
			deltapos = previousPlayerPos - playerPos;
			velocity = deltapos / 0.2f;
				
			}
			arrowScript = arrows [0].GetComponent<Arrow> ();
			float nextPosInTime; //used to get a random value in time to predict player pos at that time
			nextPosInTime = Random.Range(0.42f, 0.65f);
			velocity.x = velocity.x * nextPosInTime;
			velocity.y = velocity.y * nextPosInTime;
		    velocity.z = 0f;
		    velocity += new Vector3(Random.Range (-0.20f, 0.20f), Random.Range (-0.25f, 0.25f), 0);
		    arrowScript.playerPos = playerPos + (velocity);
			arrowScript.playerPos.z = -1;

		//Out of bounds adjustments
		if (arrowScript.playerPos.x > 3.5f)
			arrowScript.playerPos.x -= ((arrowScript.playerPos.x) - 3.5f);
		else if (arrowScript.playerPos.x < -3.5f)
			arrowScript.playerPos.x += ((arrowScript.playerPos.x) + 3.5f);
		if (arrowScript.playerPos.y < -5.8f)
			arrowScript.playerPos.y += ((arrowScript.playerPos.y) + 5.8f);
		else if (arrowScript.playerPos.y > 5.8f)
			arrowScript.playerPos.y -= ((arrowScript.playerPos.y) - 5.8f);
		
		    //arrows [0].SetActive (true);

			arrowScript = arrows [1].GetComponent<Arrow> ();
			velocity = velocity / nextPosInTime;
			nextPosInTime = Random.Range(0.3f, 0.6f);
			velocity = velocity * nextPosInTime;
			arrowScript.playerPos = playerPos - (velocity);
			arrowScript.playerPos.z = -1;

		//Out of bounds adjustments
		if (arrowScript.playerPos.x > 3.5f)
			arrowScript.playerPos.x -= ((arrowScript.playerPos.x) - 3.5f);
		else if (arrowScript.playerPos.x < -3.5f)
			arrowScript.playerPos.x += ((arrowScript.playerPos.x) + 3.5f);
		if (arrowScript.playerPos.y < -5.8f)
			arrowScript.playerPos.y += ((arrowScript.playerPos.y) + 5.8f);
		else if (arrowScript.playerPos.y > 5.8f)
			arrowScript.playerPos.y -= ((arrowScript.playerPos.y) - 5.8f);
		
			arrowScript = arrows [2].GetComponent<Arrow> ();
			velocity = velocity / nextPosInTime;
			nextPosInTime = Random.Range(0.3f, 0.6f);
			velocity = velocity * nextPosInTime;
			arrowScript.playerPos.x = playerPos.x + (velocity.x);
			arrowScript.playerPos.y = playerPos.y - velocity.y;
			arrowScript.playerPos.z = -1;

		//Out of bounds adjustments
		if (arrowScript.playerPos.x > 3.5f)
			arrowScript.playerPos.x -= ((arrowScript.playerPos.x) - 3.5f);
		else if (arrowScript.playerPos.x < -3.5f)
			arrowScript.playerPos.x += ((arrowScript.playerPos.x) + 3.5f);
		if (arrowScript.playerPos.y < -5.8f)
			arrowScript.playerPos.y += ((arrowScript.playerPos.y) + 5.8f);
		else if (arrowScript.playerPos.y > 5.8f)
			arrowScript.playerPos.y -= ((arrowScript.playerPos.y) - 5.8f);
		
			arrowScript = arrows [3].GetComponent<Arrow> ();
			velocity = velocity / nextPosInTime;
			nextPosInTime = Random.Range(0.3f, 0.6f);
			velocity = velocity * nextPosInTime;
			arrowScript.playerPos.x = playerPos.x - (velocity.x);
			arrowScript.playerPos.y = playerPos.y + velocity.y;
			arrowScript.playerPos.z = -1;

		//Out of bounds adjustments
		if (arrowScript.playerPos.x > 3.5f)
			arrowScript.playerPos.x -= ((arrowScript.playerPos.x) - 3.5f);
		else if (arrowScript.playerPos.x < -3.5f)
			arrowScript.playerPos.x += ((arrowScript.playerPos.x) + 3.5f);
		if (arrowScript.playerPos.y < -5.8f)
			arrowScript.playerPos.y += ((arrowScript.playerPos.y) + 5.8f);
		else if (arrowScript.playerPos.y > 5.8f)
			arrowScript.playerPos.y -= ((arrowScript.playerPos.y) - 5.8f);

		previousPlayerPos = playerPos;
		previousPlayerPos.z = -1;

	}

	public void nextPositionRequested(GameObject o){
		Vector3 deltapos;

		deltapos = previousPlayerPos - playerPos;
		velocity = deltapos / timeSinceLastMove;
		float nextPosInTime;//used to get a random value in time to predict player pos at that time
		nextPosInTime = Random.Range(0.5f, 0.9f);
		playerPos.z = -1;
		if (o.name.Equals(arrows [0].name)) {
			arrowScript = arrows [0].GetComponent<Arrow> ();

			nextPosInTime = Random.Range(0.5f, 0.7f);
			velocity.x = velocity.x * nextPosInTime;
			velocity.y = velocity.y * nextPosInTime;
			velocity.z = 0f;
			velocity += new Vector3(Random.Range (-0.25f, 0.25f), Random.Range (-0.35f, 0.35f), 0);
			arrowScript.playerPos = playerPos + (velocity);
			arrowScript.playerPos.z = -1;

			//Out of bounds adjustments
			if (arrowScript.playerPos.x > 3.5f)
				arrowScript.playerPos.x -= ((arrowScript.playerPos.x) - 3.5f);
			else if (arrowScript.playerPos.x < -3.5f)
				arrowScript.playerPos.x += ((arrowScript.playerPos.x) + 3.5f);
			if (arrowScript.playerPos.y < -5.8f)
				arrowScript.playerPos.y += ((arrowScript.playerPos.y) + 5.8f);
			else if (arrowScript.playerPos.y > 5.8f)
				arrowScript.playerPos.y -= ((arrowScript.playerPos.y) - 5.8f);
			
		} else if (o.name.Equals(arrows [1].name)) {
			arrowScript = arrows [1].GetComponent<Arrow> ();
			velocity = velocity / nextPosInTime;
			nextPosInTime = Random.Range(0.4f, 0.6f);
			velocity = velocity * nextPosInTime;
			arrowScript.playerPos = playerPos - (velocity);
			arrowScript.playerPos.z = -1;
			//Out of bounds adjustments
			if (arrowScript.playerPos.x > 3.5f)
				arrowScript.playerPos.x -= ((arrowScript.playerPos.x) - 3.5f);
			else if (arrowScript.playerPos.x < -3.5f)
				arrowScript.playerPos.x += ((arrowScript.playerPos.x) + 3.5f);
			if (arrowScript.playerPos.y < -5.8f)
				arrowScript.playerPos.y += ((arrowScript.playerPos.y) + 5.8f);
			else if (arrowScript.playerPos.y > 5.8f)
				arrowScript.playerPos.y -= ((arrowScript.playerPos.y) - 5.8f);
			
		} else if (o.name.Equals(arrows [2].name)) {
			arrowScript = arrows [2].GetComponent<Arrow> ();
			velocity = velocity / nextPosInTime;
			nextPosInTime = Random.Range(0.4f, 0.6f);
			velocity = velocity * nextPosInTime;
			arrowScript.playerPos.x = playerPos.x + (velocity.x);
			arrowScript.playerPos.y = playerPos.y - velocity.y;
			arrowScript.playerPos.z = -1;

			//Out of bounds adjustments
			if (arrowScript.playerPos.x > 3.5f)
				arrowScript.playerPos.x -= ((arrowScript.playerPos.x) - 3.5f);
			else if (arrowScript.playerPos.x < -3.5f)
				arrowScript.playerPos.x += ((arrowScript.playerPos.x) + 3.5f);
			if (arrowScript.playerPos.y < -5.8f)
				arrowScript.playerPos.y += ((arrowScript.playerPos.y) + 5.8f);
			else if (arrowScript.playerPos.y > 5.8f)
				arrowScript.playerPos.y -= ((arrowScript.playerPos.y) - 5.8f);
			
		}
		else if (o.name.Equals(arrows [3].name)) {
			arrowScript = arrows [3].GetComponent<Arrow> ();
			velocity = velocity / nextPosInTime;
			nextPosInTime = Random.Range(0.4f, 0.6f);
			velocity = velocity * nextPosInTime;
			arrowScript.playerPos.x = playerPos.x - (velocity.x);
			arrowScript.playerPos.y = playerPos.y + velocity.y;
			arrowScript.playerPos.z = -1;

			//Out of bounds adjustments
			if (arrowScript.playerPos.x > 3.5f)
				arrowScript.playerPos.x -= ((arrowScript.playerPos.x) - 3.5f);
			else if (arrowScript.playerPos.x < -3.5f)
				arrowScript.playerPos.x += ((arrowScript.playerPos.x) + 3.5f);
			if (arrowScript.playerPos.y < -5.8f)
				arrowScript.playerPos.y += ((arrowScript.playerPos.y) + 5.8f);
			else if (arrowScript.playerPos.y > 5.8f)
				arrowScript.playerPos.y -= ((arrowScript.playerPos.y) - 5.8f);
		}

	
	}
}
