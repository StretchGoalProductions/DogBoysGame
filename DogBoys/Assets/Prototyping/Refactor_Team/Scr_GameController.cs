using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scr_GameController : MonoBehaviour
{
    #region Variables
    private Scr_TeamController teamInfo_;
    public static GameObject selectedDog_;
    public static bool redTeamTurn_;
    public static bool blueTeamTurn_;
    public static bool attackMode_;
    public static int roundCount_;
    private static GameObject cameraPivot_;
    public static string displayTeamName_;
    public static Scr_GameController Instance;
    #endregion

    private void Awake()
    {
        Instance = this;
        teamInfo_ = this.gameObject.GetComponent<Scr_TeamController>();
        cameraPivot_ = GameObject.FindGameObjectsWithTag("MainCamera")[0]; //There should only be one object that is set to MainCamera
        displayTeamName_ = "";
        blueTeamTurn_ = true;
        redTeamTurn_ = false;
        attackMode_ = false;
        roundCount_ = 1;
    }

    //Check for an elimination win case
    public static void WinGameCheck()
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
    public static void CheckTurn()
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
    private static void RoundUpdate()
    {
        if ((roundCount_ % 2 == 1))
        {
            blueTeamTurn_ = true;
            redTeamTurn_ = false;
            ResetActionCount(Scr_TeamController.blueTeam);
            ChangeCameraPivot();
            roundCount_ += 1;
            displayTeamName_ = "Blue Bandits";
        }
        else
        {
            blueTeamTurn_ = false;
            redTeamTurn_ = true;
            ResetActionCount(Scr_TeamController.redTeam);
            ChangeCameraPivot();
            roundCount_ += 1;
            displayTeamName_ = "Red Rovers";
        }
    }

    //On new round, reset the dogs available actions back to 2
    private static void ResetActionCount(List<GameObject> allDogs)
    {
        foreach (GameObject dog in allDogs)
        {
            dog.GetComponent<Scr_DogBase>().movesLeft = 2;
        }
    }

    //On new round, force the camera to move   
    private static void ChangeCameraPivot()
    {
        cameraPivot_.GetComponent<Camera_Movement>().toggle = true;
    }

    //If there will be a gloabl canvas this function will handle updating the canvas information
    private static void UpdateDisplay()
    {
        return;
    }
    #endregion

}
