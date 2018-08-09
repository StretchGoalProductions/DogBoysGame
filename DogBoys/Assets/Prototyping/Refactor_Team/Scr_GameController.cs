using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scr_GameController : MonoBehaviour
{
    #region Variables
    private Scr_TeamController teamInfo_;
    public static GameObject selectedDog_;
    public static bool redTeamTurn_;
    public static bool blueTeamTurn_;
    public static bool attackMode_;
    public static int roundCount_;
    private static Camera_Movement cameraPivot_;
    public static string displayTeamName_;
    public static Scr_GameController Instance;
    public GameObject winSceneInfo;
    public static GameObject winInfo_;
    #endregion

    private void Awake()
    {
        Instance = this;
        teamInfo_ = this.gameObject.GetComponent<Scr_TeamController>();
        cameraPivot_ = Camera.main.transform.parent.GetComponent<Camera_Movement>();
        displayTeamName_ = "Blue Bandits";
        blueTeamTurn_ = true;
        redTeamTurn_ = false;
        attackMode_ = false;
        roundCount_ = 1;
        winInfo_ = (GameObject)Instantiate(winSceneInfo, transform.position, transform.rotation);
    }

    //Check for an elimination win case
    public static void WinGameCheck()
    {
        if (Scr_TeamController.blueTeam.Count <= 0)
        {
            winInfo_.GetComponent<cls_Win_Screne_Info>().winner_ = true;
            SceneManager.LoadScene("Win Scene");
        }
        else if (Scr_TeamController.redTeam.Count <= 0)
        {
            winInfo_.GetComponent<cls_Win_Screne_Info>().winner_ = false;
            SceneManager.LoadScene("Win Scene");
        }
    }

    #region Functions for updating round
    //Check for the start of a new round
    public static void CheckTurn()
    {
        if (blueTeamTurn_)
        {
            int countNoActionsLeft = 0;
            foreach (GameObject dog in Scr_TeamController.blueTeam)
            {
                if (dog.GetComponent<Scr_DogBase>().movesLeft <= 0)
                {
                    countNoActionsLeft += 1;
                }
            }

            if (Scr_TeamController.blueTeam.Count <= countNoActionsLeft)
            {
                RoundUpdate();
            }
        }

        if (redTeamTurn_)
        {
            int countNoActionsLeft = 0;
            foreach (GameObject dog in Scr_TeamController.redTeam)
            {
                if(dog.GetComponent<Scr_DogBase>().movesLeft <= 0)
                {
                    countNoActionsLeft += 1;
                }
            }

            if (Scr_TeamController.redTeam.Count == countNoActionsLeft)
            {
                RoundUpdate();
            }
        }
    }

    //Update all information related to the start of a new round
    private static void RoundUpdate()
    {
        attackMode_ = false;
        
        if (redTeamTurn_)
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
        cameraPivot_.toggle = true;
    }

    //If there will be a gloabl canvas this function will handle updating the canvas information
    private static void UpdateDisplay()
    {
        return;
    }
    
    public static List<GameObject> returnRedTeam()
    {
        return Scr_TeamController.redTeam;
    }

    public static List<GameObject> returnBlueTeam()
    {
        return Scr_TeamController.blueTeam;
    }
    #endregion

}
