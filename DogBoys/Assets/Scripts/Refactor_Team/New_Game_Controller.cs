using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class New_Game_Controller : MonoBehaviour {
    #region Variables
    private TeamController teamInfo_;
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
        checkTurn();
    }

    private void checkTurn()
    {
        int currentActionUsed = 0;
        if (blueTeamTurn_)
        {
            int maxActionAllowed = blueTeam.Count * 2;
            foreach (GameObject dog in blueTeam)
            {
                dog.GetComponent<>();
            }

            if (currentActionUsed >= maxActionAllowed)
            {
                roundUpdate();
            }
        }

        if (redTeamTurn_)
        {
            int maxActionAllowed = redTeam.Count * 2;
            foreach (GameObject dog in redTeam)
            {
                dog.GetComponent<>();
            }

            if (currentActionUsed >= maxActionAllowed)
            {
                roundUpdate();
            }
        }
    }

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
}
