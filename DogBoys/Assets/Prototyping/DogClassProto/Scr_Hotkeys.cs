using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Hotkeys : MonoBehaviour {

    private GameObject dogObj;
	private Scr_DogBase dogScr;
    private Scr_UIController UICont;

    void Start() {
        UICont = GetComponent<Scr_UIController>();
    }
	
	void Update () {
        dogObj = Scr_GameController.selectedDog_;

        if (dogObj != null) {
            dogScr = dogObj.GetComponent<Scr_DogBase>();
            if (dogScr.currentState == Scr_DogBase.dogState.selected) {
                CheckForHotKeyPress();
            }
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
            UICont.OnClickAttackButton();
        }
        //Hotkey for Unselecting a Character
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            dogScr.UnselectCharacter();
        }
        //HotKey for overwatch(setGuardDog)
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UICont.OnClickOverwatchButton();
        }
        //Hotkey for skip turn
        else if (Input.GetKeyDown(KeyCode.Backspace))
        {
            dogScr.SkipTurn();
        }
    }
}
