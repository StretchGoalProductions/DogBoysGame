using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menu_control : MonoBehaviour {
	[SerializeField]
	private GameObject gc;

	public void loadMission() {
		if (gc.GetComponent<Constants_Values> ().getRed ().Count < 3 ||
		    gc.GetComponent<Constants_Values> ().getBlue ().Count < 3) {
			//teams too small
			Debug.Log("teams too small - must have at least 3 dogs");
		}else if (gc.GetComponent<Constants_Values> ().getRed ().Count > 5 ||
			gc.GetComponent<Constants_Values> ().getBlue ().Count > 5){
			//teams too big
			Debug.Log("teams too big - must have at most 5 dogs");
		}else{
			//gc.GetComponent<Constants_Values>().
			SceneManager.LoadScene ("Level01");
		}
	}

	public void exit() {
		Application.Quit ();
	}
		
}
