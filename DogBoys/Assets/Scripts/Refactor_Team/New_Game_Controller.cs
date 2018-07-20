using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class New_Game_Controller : MonoBehaviour {
    #region Variables
    private TeamController teamInfo_;
    private GameObject selectedDog_;
    private List<GameObject> redTeam = new List<GameObject>();
    private List<GameObject> blueTeam = new List<GameObject>();
    private bool redTeamTurn_;
    private bool blueTeamTurn_;
    private int roundCount_;
    #endregion

    private void Awake()
    {
        teamInfo_ = this.gameObject.GetComponent<TeamController>();
        blueTeamTurn_ = true;
        redTeamTurn_ = false;
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
        checkTurn();
    }

    //Check for the start of a new round
    private void checkTurn()
    {
        if (blueTeamTurn_)
        {
            int maxActionAllowed = blueTeam.Count * 2;
            foreach (GameObject dog in blueTeam)
            {
                maxActionAllowed -= dog.GetComponent<cls_DogBase>().getMovesLeft();
            }

            if (0 <= maxActionAllowed)
            {
                roundUpdate();
            }
        }

        if (redTeamTurn_)
        {
            int maxActionAllowed = redTeam.Count * 2;
            foreach (GameObject dog in redTeam)
            {
                maxActionAllowed -= dog.GetComponent<cls_DogBase>().getMovesLeft();
            }

            if (0 <= maxActionAllowed)
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
            blueTeamTurn_ = false;
            redTeamTurn_ = true;
            roundCount_ += 1;
        }
        else
        {
            blueTeamTurn_ = true;
            redTeamTurn_ = false;
            roundCount_ += 1;
        }
    }

    //Check for and elimination win case
    private void winGameCheck()
    {
        if (redTeam.Count <= 0)
        {
            Constants.WinScreen.C_WinText = "Red Rovers Win!!";
            SceneManager.LoadScene("Win Scene");
        }
        else if (blueTeam.Count <= 0)
        {
            Constants.WinScreen.C_WinText = "Blue Bandits Win!!";
            SceneManager.LoadScene("Win Scene");
        }
    }

}
