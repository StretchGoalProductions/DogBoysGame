using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scr_TurnText : MonoBehaviour {

    string CurrentTeam;

    public GameObject RedTeam, BlueTeam;

	void Update () {
		CurrentTeam = Scr_GameController.displayTeamName_;
        if (CurrentTeam == "Blue Bandits")
        {
            RedTeam.SetActive(false);
            BlueTeam.SetActive(true);
        }
        else if (CurrentTeam == "Red Rovers")
        {
            RedTeam.SetActive(true);
            BlueTeam.SetActive(false);
        }
    }
}
