using UnityEngine;
using System.Collections;

public class Generate : MonoBehaviour {

	float instantiationTime = 0f;
	float totalTimeSinceStart = -2f;//guarantees no box in first 7-12 seconds, previous value before update was -3
	GameObject player;
	PlayerScript playerScript;
	bool gameStarted = false;

	public GameObject mysteryBox;
	// Use this for initialization
	void Start () {
		instantiationTime = Random.Range (5f, 10f);
		player = GameObject.FindWithTag ("Player");
		playerScript = player.GetComponent<PlayerScript> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (playerScript.isTouched) {
			gameStarted = true;
		} else
			gameStarted = false;
		if (gameStarted) {
			totalTimeSinceStart += Time.deltaTime;
		}
		if (totalTimeSinceStart >= instantiationTime) {
			Instantiate (mysteryBox, new Vector3 (Random.Range (-3, 3), Random.Range (-5, +5), -1), Quaternion.identity);
			instantiationTime = Random.Range (7f, 25f);
			totalTimeSinceStart = 0f;
		}
	}
}
