using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Hotkeys : MonoBehaviour {

	private Scr_DogBase dog;

	void Start () {
		dog = GetComponent<Scr_DogBase>();
	}
	
	void Update () {
		if(dog.currentState == Scr_DogBase.dogState.selected) {
			CheckForHotKeyPress();
		}

		if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
	}



    private void CheckForHotKeyPress()
    {
        //Using hotkeys that XCOM uses.  1 for attack, 2 for overwatch, backspace for skip turn
        //Hotkey for entering attackmode
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            dog.UIController.OnClickAttackButton();
        }
        //Hotkey for Unselecting a Character
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            dog.UnselectCharacter();
        }
        //HotKey for overwatch(setGuardDog)
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            dog.UIController.OnClickOverwatchButton();
        }
        //Hotkey for skip turn
        else if (Input.GetKeyDown(KeyCode.Backspace))
        {
            dog.SkipTurn();
        }
    }
}
