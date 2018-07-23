using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scr_MenuController : MonoBehaviour {

	public void LoadMission() {
		Debug.Log(Scr_TeamController.menuRedDogs.Count + " " + Scr_TeamController.menuBlueDogs.Count);
		if (Scr_TeamController.menuRedDogs.Count < 3 || Scr_TeamController.menuBlueDogs.Count < 3) {
			Debug.Log("teams too small - must have at least 3 dogs");
		}else if (Scr_TeamController.menuRedDogs.Count > 5 || Scr_TeamController.menuBlueDogs.Count > 5){
			Debug.Log("teams too big - must have at most 5 dogs");
		}else{
			SceneManager.LoadScene ("DogClassProto_level1");
		}
	}

	public void Exit() {
		Application.Quit ();
	}
}
