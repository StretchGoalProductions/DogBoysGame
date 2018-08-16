using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scr_TurnText : MonoBehaviour {

    string CurrentTeam;

    public GameObject RedTeam, BlueTeam;
    public GameObject redActions, blueActions;

    void Start() {
        for (int i=0; i < Scr_TeamController.menuBlueDogs.Count; i++) {
            redActions.transform.GetChild(i).gameObject.SetActive(true);
            blueActions.transform.GetChild(i).gameObject.SetActive(true);
        }
    }

	void Update () {
		CurrentTeam = Scr_GameController.displayTeamName_;
        if (CurrentTeam == "Blue Bandits")
        {
            RedTeam.SetActive(false);
            BlueTeam.SetActive(true);

            for (int i=0; i < Scr_TeamController.menuBlueDogs.Count; i++) {
                blueActions.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = Scr_TeamController.blueTeam[i].GetComponent<Scr_DogBase>().movesLeft.ToString();
            }
        }
        else if (CurrentTeam == "Red Rovers")
        {
            RedTeam.SetActive(true);
            BlueTeam.SetActive(false);

            for (int i=0; i < Scr_TeamController.menuRedDogs.Count; i++) {
                redActions.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = Scr_TeamController.redTeam[i].GetComponent<Scr_DogBase>().movesLeft.ToString();
            }
        }
    }
}
