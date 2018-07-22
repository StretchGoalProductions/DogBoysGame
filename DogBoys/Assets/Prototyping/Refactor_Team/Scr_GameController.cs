using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scr_GameController : MonoBehaviour
{
    #region Variables
    private Scr_TeamController teamInfo_;
    public GameObject selectedDog_;
    public bool redTeamTurn_;
    public bool blueTeamTurn_;
    public bool attackMode_;
    public int roundCount_;
    private GameObject cameraPivot_;
    private Text displayTeamName_;
    public static Scr_GameController Instance;
    #endregion

    private void Awake()
    {
        Instance = this;
        teamInfo_ = this.gameObject.GetComponent<Scr_TeamController>();
        cameraPivot_ = GameObject.FindGameObjectsWithTag("MainCamera")[0]; //There should only be one object that is set to MainCamera
        //displayTeamName_ = this.gameObject.GetComponentInChildren<Text>();
        blueTeamTurn_ = true;
        redTeamTurn_ = false;
        attackMode_ = false;
        roundCount_ = 1;
    }

    //Check for an elimination win case
    public void WinGameCheck()
    {
        if (Scr_TeamController.blueTeam.Count <= 0)
        {
            Constants.WinScreen.C_WinText = "Red Rovers Win!!";
            SceneManager.LoadScene("Win Scene");
        }
        else if (Scr_TeamController.redTeam.Count <= 0)
        {
            Constants.WinScreen.C_WinText = "Blue Bandits Win!!";
            SceneManager.LoadScene("Win Scene");
        }
    }

    #region Functions for updating round
    //Check for the start of a new round
    public void CheckTurn()
    {
        if (blueTeamTurn_)
        {
            int coutNoActionsLeft = 0;
            foreach (GameObject dog in Scr_TeamController.blueTeam)
            {
                if (dog.GetComponent<Scr_DogBase>().movesLeft <= 0)
                {
                    coutNoActionsLeft += 1;
                }
            }

            if (Scr_TeamController.blueTeam.Count <= coutNoActionsLeft)
            {
                RoundUpdate();
            }
        }

        if (redTeamTurn_)
        {
            int coutNoActionsLeft = 0;
            foreach (GameObject dog in Scr_TeamController.redTeam)
            {
                if(dog.GetComponent<Scr_DogBase>().movesLeft <= 0)
                {
                    coutNoActionsLeft += 1;
                }
            }

            if (Scr_TeamController.redTeam.Count == coutNoActionsLeft)
            {
                RoundUpdate();
            }
        }
    }

    //Update all information related to the start of a new round
    private void RoundUpdate()
    {
        if ((roundCount_ % 2 == 1))
        {
            blueTeamTurn_ = true;
            redTeamTurn_ = false;
            ResetActionCount(Scr_TeamController.blueTeam);
            ChangeCameraPivot();
            roundCount_ += 1;
        }
        else
        {
            blueTeamTurn_ = false;
            redTeamTurn_ = true;
            ResetActionCount(Scr_TeamController.redTeam);
            ChangeCameraPivot();
            roundCount_ += 1;
        }
    }

    //On new round, reset the dogs available actions back to 2
    private void ResetActionCount(List<GameObject> allDogs)
    {
        foreach (GameObject dog in allDogs)
        {
            dog.GetComponent<Scr_DogBase>().movesLeft = 2;
        }
    }

    //On new round, force the camera to move   
    private void ChangeCameraPivot()
    {
        cameraPivot_.GetComponent<Camera_Movement>().toggle = true;
    }

    //If there will be a gloabl canvas this function will handle updating the canvas information
    private void UpdateDisplay()
    {
        return;
    }
    #endregion

}
