using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scr_TurnText : MonoBehaviour {

    public static string CurrentTeam;

    public GameObject RedTeam, BlueTeam;
    private static GameObject _redTeam, _blueTeam;
    public GameObject redActions, blueActions;
    private static GameObject _redActions, _blueActions;

    void Start() {
        for (int i=0; i < Scr_TeamController.menuBlueDogs.Count; i++) {
            redActions.transform.GetChild(i).gameObject.SetActive(true);
            blueActions.transform.GetChild(i).gameObject.SetActive(true);
        }
        _redTeam = RedTeam;
        _blueTeam = BlueTeam;
        _redActions = redActions;
        _blueActions = blueActions;
    }


    public static void updateText() {
        CurrentTeam = Scr_GameController.displayTeamName_;
        if (CurrentTeam == "Blue Bandits")
        {
            _redTeam.SetActive(false);
            _blueTeam.SetActive(true);

            for (int i=0; i < Scr_TeamController.blueTeam.Count; i++) {
                _blueActions.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = Scr_TeamController.blueTeam[i].GetComponent<Scr_DogBase>().movesLeft.ToString();
            }
        }
        else if (CurrentTeam == "Red Rovers")
        {
            _redTeam.SetActive(true);
            _blueTeam.SetActive(false);

            for (int i=0; i < Scr_TeamController.redTeam.Count; i++) {
                _redActions.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = Scr_TeamController.redTeam[i].GetComponent<Scr_DogBase>().movesLeft.ToString();
            }
        }
    }
}
