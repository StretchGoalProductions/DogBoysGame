using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class New_Game_Controller : MonoBehaviour
{
    #region Variables
    private TeamController teamInfo_;
    private GameObject selectedDog_;
    private List<GameObject> redTeam = new List<GameObject>();
    private List<GameObject> blueTeam = new List<GameObject>();
    private bool redTeamTurn_;
    private bool blueTeamTurn_;
    private bool attackMode_;
    private int roundCount_;
    private GameObject cameraPivot_;
    private Text displayTeamName_;
    #endregion

    private void Awake()
    {
        teamInfo_ = this.gameObject.GetComponent<TeamController>();
        cameraPivot_ = GameObject.FindGameObjectsWithTag("MainCamera")[0]; //There should only be one object that is set to MainCamera
        //displayTeamName_ = this.gameObject.GetComponentInChildren<Text>();
        blueTeamTurn_ = true;
        redTeamTurn_ = false;
        attackMode_ = false;
        roundCount_ = 1;
    }

    private void Start()
    {
        redTeam = teamInfo_.redTeam;
        blueTeam = teamInfo_.blueTeam;
    }

    public void Update()
    {
        winGameCheck(); //This should be called on "Dog death", but currently I don't know where to put this
        checkTurn(); //This can be called on dog/actor actions so we can ignore the issues of calling a for loop every frame 
    }

    //Check for an elimination win case
    private void winGameCheck()
    {
        if (blueTeam.Count <= 0)
        {
            Constants.WinScreen.C_WinText = "Red Rovers Win!!";
            SceneManager.LoadScene("Win Scene");
        }
        else if (redTeam.Count <= 0)
        {
            Constants.WinScreen.C_WinText = "Blue Bandits Win!!";
            SceneManager.LoadScene("Win Scene");
        }
    }

    //Can a character be selected
    private void setSelectedCharacter(GameObject selectedCharacter)
    {
        selectedDog_ = selectedCharacter;

        if (canTakeAction(selectedCharacter))
        {
            unselectCharacter();
        }
    }

    //Unselect the currently select dog
    private void unselectCharacter()
    {
        if (attackMode_) //If a dog is currently in attack mode and you are trying to unselect it, deactivate the attack mode before unselecting
            activateAttackMode();
        selectedDog_.GetComponent<Character>().UnselectCharacter();
        selectedDog_ = null;
    }

    //Check to see if and individual dog still has an action left
    private bool canTakeAction(GameObject selectedCharacter)
    {
        if (selectedCharacter.GetComponent<cls_DogBase>().getMovesLeft() > 0)
        {
            return true;
        }
        return false;
    }

    //Check to see if the user has pressed a hotkey that has a command assigned to it
    private void checkForHotKeyPress()
    {
        //Using hotkeys that XCOM uses.  1 for attack, 2 for overwatch, backspace for skip turn
        //Hotkey for entering attackmode
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            activateAttackMode();
        }
        //Hotkey for Unselecting a Character
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            unselectCharacter();
        }
        //HotKey for overwatch(setGuardDog)
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            activateGuardDog();
        }
        //Hotkey for skip turn
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            activateTurnSkip();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    #region Functions for updating round
    //Check for the start of a new round
    private void checkTurn()
    {
        if (blueTeamTurn_)
        {
            int coutNoActionsLeft = 0;
            foreach (GameObject dog in blueTeam)
            {
                if (dog.GetComponent<cls_DogBase>().getMovesLeft() <= 0)
                {
                    coutNoActionsLeft += 1;
                }
            }

            if (blueTeam.Count <= coutNoActionsLeft)
            {
                roundUpdate();
            }
        }

        if (redTeamTurn_)
        {
            int coutNoActionsLeft = 0;
            foreach (GameObject dog in redTeam)
            {
                if(dog.GetComponent<cls_DogBase>().getMovesLeft() <= 0)
                {
                    coutNoActionsLeft += 1;
                }
            }

            if (redTeam.Count == coutNoActionsLeft)
            {
                roundUpdate();
            }
        }
    }

    //Update all information related to the start of a new round
    private void roundUpdate()
    {
        if ((roundCount_ % 2 == 1))
        {
            blueTeamTurn_ = true;
            redTeamTurn_ = false;
            resetActionCount(blueTeam);
            changeCameraPivot();
            roundCount_ += 1;
        }
        else
        {
            blueTeamTurn_ = false;
            redTeamTurn_ = true;
            resetActionCount(redTeam);
            changeCameraPivot();
            roundCount_ += 1;
        }
    }

    //On new round, reset the dogs available actions back to 2
    private void resetActionCount(List<GameObject> allDogs)
    {
        foreach (GameObject dog in allDogs)
        {
            dog.GetComponent<cls_DogBase>().setMovesLeft(2);
        }
    }

    //On new round, force the camera to move   
    private void changeCameraPivot()
    {
        cameraPivot_.GetComponent<Camera_Movement>().toggle = true;
    }

    //If there will be a gloabl canvas this function will handle updating the canvas information
    private void updateDisplay()
    {
        return;
    }
    #endregion

    #region Action that can be taken by Dogs
    private void activateAttackMode()
    {
        attackMode_ = !attackMode_;
        selectedDog_.GetComponent<Character>().toggleAttackMode();
    }

    private void activateGuardDog()
    {
        selectedDog_.GetComponent<Character>().IsOnOverwatch = true;
        selectedDog_.GetComponent<cls_DogBase>().setMovesLeft(0);
        unselectCharacter();
    }

    private void activateTurnSkip()
    {
        selectedDog_.GetComponent<cls_DogBase>().setMovesLeft(0);
        unselectCharacter();
    }
    #endregion
}
