using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Scr_LevelSelectButton : MonoBehaviour {
    
    public string level;
	[SerializeField]
	private AudioSource click;

    public void LoadMission()
    {
		click.Play ();
        if ((Scr_TeamController.menuBlueDogs.Count >= 3 && Scr_TeamController.menuBlueDogs.Count <= 5) && 
        (Scr_TeamController.menuRedDogs.Count >= 3 && Scr_TeamController.menuRedDogs.Count <= 5) &&
        (Scr_TeamController.menuBlueDogs.Count == Scr_TeamController.menuRedDogs.Count)) {
            Debug.Log("Loading");
            SceneManager.LoadScene(level);
        }
    }
}
