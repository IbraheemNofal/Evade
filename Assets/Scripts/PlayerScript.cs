using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Heyzap;
using Facebook.Unity;

public class PlayerScript : MonoBehaviour {


	public Text score;
	float scoreF = 0;
	public bool gameEnded = false;
   //touch and drag
	public bool isTouched = false; //used to check if the player object was touched or not
	public bool gameStarted = false;
	//powerups
	public bool shieldActive = false;
	public int freezePowerup = 100; //no of repulsion powerups remaining
	private Animator animator;
	public float totalShieldTime = 0f;
	public GameObject[] arrows;
	public float force = 100f;
	bool repulsionPowerupUsed = false; //used to check if powerup used in last second
	float repulsionPowerupCounter = 0f;
	Canvas gameOverCanvas;
	float highScore = 0f;
	Text remainingFreezes;
	public AudioSource ShieldPowerup;
	public AudioClip lost;
	public Sprite SoundOn;
	public Sprite SoundOff;
	Button soundToggleButton;
	bool soundOn = true; // used to control audio play based on sound prefs
	bool shieldCountingDown = false;
	int ShieldController = 0;
	int totalPlayTime = 0; // used to show ad every 120 seconds
	int numberOfBackButtonsPressed = 0;// used to keep counts of number of back buttons pressed, if > 1 in last 3 seconds, exit
	CanvasRenderer exitTextRenderer;
	// Use this for initialization

	void Awake(){
		//fb plugin initialization
		FB.Init();


	}

	void Start () {
		//Exit text not showing
		exitTextRenderer = GameObject.Find ("ExitOverlay").GetComponent<CanvasRenderer> ();
		exitTextRenderer.SetAlpha (0f);
		exitTextRenderer.GetComponentInChildren<Text>().GetComponent<CanvasRenderer> ().SetAlpha(0f);

		HeyzapAds.Start("74ffa522d9d72b0bf95f103d3c142b2f", HeyzapAds.FLAG_NO_OPTIONS);
		HZVideoAd.Fetch();
		if (PlayerPrefs.HasKey ("First time playing")) {
			GameObject.Find ("HowToPlayCanvas").GetComponent<Canvas> ().enabled = false;
		}

		//HeyzapAds.ShowMediationTestSuite();// used for test purposes


		if(PlayerPrefs.HasKey("Total Time Played Since Last ad"))
			totalPlayTime = PlayerPrefs.GetInt("Total Time Played Since Last ad");

		GameObject temp = GameObject.Find ("SoundToggleButton");
		soundToggleButton = temp.GetComponent<Button> ();
		highScore = PlayerPrefs.GetFloat ("Highscore");
		ShieldPowerup = GetComponent<AudioSource> ();
		GameObject z = GameObject.Find ("HUDCanvas");
		score = z.GetComponentInChildren<Text>();
		animator = GetComponent<Animator> ();
		//Animation x = animator.GetComponent<Animation> ();
		animator.enabled = false;
		GameObject y = GameObject.Find ("GameOverCanvas");
		gameOverCanvas = y.GetComponent<Canvas> ();
		gameOverCanvas.enabled = false;

		y = GameObject.Find ("RemainingFreezesText");
		remainingFreezes = y.GetComponent<Text> ();
		remainingFreezes.text = freezePowerup.ToString ();
		//repulsion powerup
		arrows = new GameObject[4];
		arrows [0] = GameObject.Find ("arrow");
		arrows [1] = GameObject.Find ("arrow (1)");
		arrows [2] = GameObject.Find ("arrow (2)");
		arrows [3] = GameObject.Find ("arrow (3)");

		//disable animation of shield remaining time at first 
		GameObject.Find ("ShieldCountDownText").GetComponent<Animator> ().enabled = false;
		GameObject.Find ("ShieldCountDownText").GetComponent<Text> ().enabled = false;

		//retrieve last player audio preferences
		int i = PlayerPrefs.GetInt("Audio");
		//1 means it's off, else means it's on
		if (i == 1) {
			soundToggleButton.image.sprite = SoundOff;
			soundOn = false;
		} else{
			soundToggleButton.image.sprite = SoundOn;
			soundOn = true;
		}

	}
	
	// Update is called once per frame
	void Update () {



		if (Input.GetKeyDown (KeyCode.Escape)) {
			numberOfBackButtonsPressed++;
			if (numberOfBackButtonsPressed == 1)
				StartCoroutine (exitButtonCheck ());
		}
		
		if(gameStarted && !gameEnded)
		scoreF += Time.deltaTime;
		
		score.text = "Score: " + scoreF.ToString ("0.00");
		/*
		if(Input.mousePresent){
			Vector3 mousePos = Input.mousePosition;
			Debug.Log(Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 8)));
			//Camera.main.ScreenToWorldPoint (new Vector3(mousePos.x, mousePos.y, 8));
			transform.position=Camera.main.ScreenToWorldPoint((new Vector3(mousePos.x, mousePos.y, 8)));
					}
					*/
		if (shieldActive) {
			//enable shield
			if (ShieldController==0) {
				if(soundOn)
				ShieldPowerup.Play ();
				animator.enabled = true;
				shieldCountingDown = true;
				animator.SetInteger ("ShieldStart", 1);
				ShieldController = 1;
			}
			totalShieldTime += Time.deltaTime;

		}
		if (totalShieldTime >= 2f && shieldCountingDown){
			shieldCountingDown = false;
			GameObject.Find ("ShieldCountDownText").GetComponent<Text> ().enabled = true;

			GameObject.Find ("ShieldCountDownText").GetComponent<Animator> ().enabled = true;
			GameObject.Find ("ShieldCountDownText").GetComponent<Animator> ().SetInteger ("ShieldCountingDown", 0);

			StartCoroutine (shieldCountDown ());
		}
		if(totalShieldTime>=5f){
			//disable shield
			animator.SetInteger ("ShieldStart", 0);
			totalShieldTime = 0f;
			GameObject y = arrows[0];
			bool collided = false;
			shieldActive = false;
			ShieldController = 0;
			foreach(GameObject x in arrows){
				if (GetComponent<Collider2D> ().IsTouching (x.GetComponent<Collider2D> ())) {
					y = x;
					collided = true;
				}
				break;
					}
			if(collided)
			OnTriggerEnter2D(y.GetComponent<Collider2D>());

		}
			
		if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Moved) {
			//move player
			Touch x = new Touch();
			x = Input.GetTouch (0);
			remainingFreezes.text = freezePowerup.ToString ();


			Vector3 wp = Camera.main.ScreenToWorldPoint(x.position);
			Vector2 touchPos = new Vector2(wp.x, wp.y);
			if(isTouched)//if the player object is touched, but the touch and the object aren't overlapping now, still move the player
				transform.position = Camera.main.ScreenToWorldPoint (new Vector3 (x.position.x, x.position.y, 8));
			
			else if (GetComponent<Collider2D>() == Physics2D.OverlapPoint (touchPos) ) {
				isTouched = true;
				gameStarted = true;
			}


		}
		if(Input.touchCount>0 && Input.GetTouch(0).phase == TouchPhase.Ended){
			isTouched=false;

		}

		if (Input.touchCount >= 2 && freezePowerup>=1 &&!repulsionPowerupUsed) {
			freezePowerup -= 1;
			remainingFreezes.text = freezePowerup.ToString ();

			foreach (GameObject x in arrows) {

				//Rigidbody2D thisBody = GetComponent<Rigidbody2D>();
				//Rigidbody2D thatBody = x.GetComponent<Rigidbody2D>();
				//var rel = new Vector2(thisBody.position.x, thatBody.position.y) - new Vector2(thatBody.position.x, thatBody.position.y);
				Arrow arrowScript = x.GetComponent<Arrow> ();
				//arrowScript.playerPos = x.transform.position;
				arrowScript.gameStarted = false; // actually freezes, too lazy to rename
				arrowScript.gameStartedControl = true;

				//rel.Normalize();
				//thatBody.AddForce(rel*force);
			
			}
			repulsionPowerupUsed = true;
			
			
			
			}

		if (repulsionPowerupUsed) {
			repulsionPowerupCounter += Time.deltaTime;
		}

		if (repulsionPowerupCounter >= 1f) {
			repulsionPowerupCounter = 0f;
			repulsionPowerupUsed = false;
			ResumeArrowMovement ();
		}






	}


	void OnTriggerEnter2D(Collider2D other){
		if(!gameEnded){
			if (other.gameObject.tag == "Enemy" && !shieldActive) {
				ShieldPowerup.clip = lost;
				gameStarted = false;
				StartCoroutine (GameOverMenuDelay ());

				gameEnded = true;
				SpriteRenderer sr = this.GetComponent<SpriteRenderer> ();
				sr.enabled = false;

				foreach (GameObject x in arrows) {


					Arrow arrowScript = x.GetComponent<Arrow> ();
					arrowScript.gameStarted = false;
					arrowScript.gameStartedControl = true;

					x.SetActive (false);


				}


				GameObject z = GameObject.Find ("EndOfGameScoreText");
				Text score1 = z.GetComponent<Text> ();
				score1.text += scoreF.ToString ("0.00");
				//isTouched = false;
				scoreF = Mathf.Round (scoreF * 100f) / 100f;
				highScore = Mathf.Round (highScore * 100f) / 100f; // round to nearest 100th

				// Set taunts here
				Text tauntText = GameObject.Find ("TauntText").GetComponent<Text> ();

				if (scoreF > highScore) {
					
					PlayerPrefs.SetFloat ("Highscore", scoreF);
					highScore = scoreF;

					z = GameObject.Find ("HighscoreText");
					score1 = z.GetComponent<Text> ();
					score1.text += highScore.ToString ("0.00");

					int x = Random.Range (1, 4); // 3 taunts
					switch (x) {
					case 1: 
						tauntText.text = "You've outdone yourself!";
						tauntText.fontSize = 20;

						break;
					case 2:
						tauntText.text = "New highscore!";
						tauntText.fontSize = 22;

						break;
					case 3:
						tauntText.text = "Well done!";
						tauntText.fontSize = 22;

						break;
					}
				} else {
					// no new highscore
					if (scoreF < 15) {
						// below 15 seconds
						int x = Random.Range (1, 5); // 5 taunts
						switch (x) {
						case 1: 
							tauntText.text = "Well, this is embarrassing";
							tauntText.fontSize = 19;

							break;
						case 2:
							tauntText.text = "That's not all you've got,\nis it?!";
							tauntText.transform.localPosition = new Vector3 (tauntText.transform.localPosition.x, 18.7f, 0f);
							tauntText.fontSize = 18;

							break;
						case 3:
							tauntText.text = "Is that your best?";
							tauntText.fontSize = 21;

							break;
						case 4:
							tauntText.text = "Uh-Oh";
							tauntText.fontSize = 22;
							break;

						}


					}else if (scoreF >= (highScore - 3f) && scoreF < highScore) {
						// below highscore by 3
						int x = Random.Range (1, 3); // 2 taunts
						switch (x) {
						case 1: 
							tauntText.text = "So close!";
							tauntText.fontSize = 22;

							break;
						case 2:
							tauntText.text = "A new highscore.. almost";
							tauntText.fontSize = 20;

							break;
						}

					}
					else if (scoreF >= 15) {
						// above or equal to 15 seconds
						int x = Random.Range (1, 3); // 2 taunts
						switch (x) {
						case 1: 
							tauntText.text = "Give it one more go";
							tauntText.fontSize = 22;

							break;
						case 2:
							tauntText.text = "You can do better";
							tauntText.fontSize = 22;

							break;
						}
					} 

					z = GameObject.Find ("HighscoreText");
					score1 = z.GetComponent<Text> ();
					score1.text += highScore.ToString ("0.00");


				}
				//Video Ad
				if (totalPlayTime+ Mathf.FloorToInt(scoreF) >= 90) {
					if (HZVideoAd.IsAvailable ()) {
						PlayerPrefs.SetInt ("Total Time Played Since Last ad", 0);
						HZVideoAd.Show ();
					}

				} else
					PlayerPrefs.SetInt ("Total Time Played Since Last ad", totalPlayTime + Mathf.FloorToInt(scoreF));
			}
		
	}
	}

	void ResumeArrowMovement(){

		foreach (GameObject x in arrows) {


			Arrow arrowScript = x.GetComponent<Arrow> ();
			arrowScript.gameStarted = true;
			arrowScript.gameStartedControl = false;
			//SpriteRenderer sr =	x.GetComponent<SpriteRenderer>(); 
			//sr.enabled = false;
		}

	}

	public void newGameButtonClicked(){
		SceneManager.LoadScene ("scene 1");

	
	}

	IEnumerator GameOverMenuDelay(){
		if(soundOn)
		ShieldPowerup.Play ();
		yield return new WaitForSeconds (0.2f);
		gameOverCanvas.enabled = true;
	
		//DoAThingOverTime(new Color(), new Color(), 0.3f);
	}

	IEnumerator DoAThingOverTime(Color start, Color end, float duration) {
		Color someColorValue;
		Renderer z = gameOverCanvas.GetComponent<Renderer> ();
		for (float t=0f;t<duration;t+=Time.deltaTime) {
			float normalizedTime = t/duration;
			//right here, you can now use normalizedTime as the third parameter in any Lerp from start to end
			someColorValue = Color.Lerp(start, end, normalizedTime);
			z.material.SetColor ("_Color", someColorValue);
				
			yield return null;
		}
		someColorValue = end; //without this, the value will end at something like 0.9992367
	}

	public void toggleAudioPrefs(){

		if (soundOn) {
			soundToggleButton.image.sprite = SoundOff;
			PlayerPrefs.SetInt ("Audio", 1);

		} else {
			soundToggleButton.image.sprite = SoundOn;
			PlayerPrefs.SetInt ("Audio", 0);

		}
		soundOn = !soundOn;


}
	public void showIntructions(){
	
		GameObject.Find ("HowToPlayCanvas").GetComponent<Canvas> ().enabled = true;
		GameObject.Find ("Image").GetComponent<Image> ().enabled = true;

	}

	IEnumerator shieldCountDown(){
		GameObject.Find ("ShieldCountDownText").GetComponent<Text> ().text = 3.ToString ();

		yield return new WaitForSeconds (0.6f);
		GameObject.Find ("ShieldCountDownText").GetComponent<Animator> ().enabled = false;
		yield return new WaitForSeconds (0.4f);
		GameObject.Find ("ShieldCountDownText").GetComponent<Text> ().text = 2.ToString ();
		GameObject.Find ("ShieldCountDownText").GetComponent<Animator> ().enabled = true;
		yield return new WaitForSeconds (0.6f);
		GameObject.Find ("ShieldCountDownText").GetComponent<Animator> ().enabled = false;
		yield return new WaitForSeconds (0.4f);
		GameObject.Find ("ShieldCountDownText").GetComponent<Text> ().text = 1.ToString ();
		GameObject.Find ("ShieldCountDownText").GetComponent<Animator> ().enabled = true;
		yield return new WaitForSeconds (0.6f);
		GameObject.Find ("ShieldCountDownText").GetComponent<Animator> ().SetInteger ("ShieldCountingDown", 1);
		GameObject.Find ("ShieldCountDownText").GetComponent<Text> ().enabled = false;

		yield return new WaitForSeconds (0.6f);
		GameObject.Find ("ShieldCountDownText").GetComponent<Animator> ().enabled = false;
	}

	IEnumerator exitButtonCheck(){
		if (numberOfBackButtonsPressed>1)
			Application.Quit ();
		// if button clicked again in last 3 seconds, exit
		CanvasRenderer textRenderer = exitTextRenderer.GetComponentInChildren<Text> ().GetComponent<CanvasRenderer> ();


		for (int i = 0; i <20; i++) {
			//check for clicks, fade text in
			exitTextRenderer = GameObject.Find ("ExitOverlay").GetComponent<CanvasRenderer> ();
			exitTextRenderer.SetAlpha ((exitTextRenderer.GetAlpha() + 0.05f));
			textRenderer.SetAlpha((textRenderer.GetAlpha()+0.05f));
			if (numberOfBackButtonsPressed>1)
				Application.Quit ();
			yield return new WaitForSeconds (0.05f);

		}
		for (int i = 0; i <20; i++) {
			//check for clicks
			if (numberOfBackButtonsPressed>1)
				Application.Quit ();
			yield return new WaitForSeconds (0.05f);

		}

		for (int i = 0; i <20; i++) {
			//check for clicks, fade text out
			exitTextRenderer = GameObject.Find ("ExitOverlay").GetComponent<CanvasRenderer> ();
			exitTextRenderer.SetAlpha (exitTextRenderer.GetAlpha() - 0.05f);
			textRenderer.SetAlpha(textRenderer.GetAlpha()-0.05f);
			if (numberOfBackButtonsPressed>1)
				Application.Quit ();
			yield return new WaitForSeconds (0.05f);

		}

		numberOfBackButtonsPressed = 0;
	}

	public void fbShareClicked(){
		// share action
		//Text tauntText = GameObject.Find ("TauntText").GetComponent<Text> ();
		//tauntText.text = Application.persistentDataPath + "/Assets/Sprites/HighResIcon.png";




		
			FB.ShareLink (new System.Uri ("https://play.google.com/store/apps/details?id=com.Nofal.Evade"), "I just got a score of " +scoreF+" on Evade! Think you can beat it?", 
			"Evade is a simple, yet addictive game that'll put your speed and reflexes to the test. Seemingly easy, it's anything but. Most people can't survive for longer than 60 seconds, can you?",
			null, null);
		//new System.Uri(WWW.EscapeURL("file://"+Application.dataPath+"/Sprites/HighResIcon.png"))
		//string facebookshare = "https://www.facebook.com/sharer/sharer.php?u=" + WWW.EscapeURL("https://play.google.com/store/apps/details?id=com.Nofal.Evade");
		//Application.OpenURL(facebookshare);
	}

	public void TwitterShareClicked(){
		 const string TWITTER_ADDRESS = "http://twitter.com/intent/tweet";
		 const string TWEET_LANGUAGE = "en"; 


			Application.OpenURL(TWITTER_ADDRESS +
			"?text=" + WWW.EscapeURL("I just got a score of " +scoreF+" on Evade! Think you can beat it? #Evade\n https://play.google.com/store/apps/details?id=com.Nofal.Evade") +
				"&amp;lang=" + WWW.EscapeURL(TWEET_LANGUAGE));
		

	}

		
}
