using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scr_TurnText : MonoBehaviour {

	void Update () {
		GetComponent<Text>().text = Scr_GameController.displayTeamName_;
	}
}
