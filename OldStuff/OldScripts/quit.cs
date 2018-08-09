using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class quit : MonoBehaviour {

	public void quitGame(){
		Debug.Log ("quitting");
		Application.Quit ();
		Debug.Log ("quit");
	}

}
