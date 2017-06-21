using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FirstTimePlaying : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void InstructionScreenClicked(){
		GameObject.Find ("Image").GetComponent<Image> ().enabled = false;
	}

	public void InstructionScreenClicked2(){
		PlayerPrefs.SetInt ("First time playing", 1);
		GameObject.Find ("HowToPlayCanvas").GetComponent<Canvas> ().enabled = false;

	}
}
