using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_DisplayLocalDogUI : MonoBehaviour {

    public GameObject percent;

    private bool instanciated = false;
    private GameObject[] dogs = null;

    // Update is called once per frame
    void Update () {   

        // Attack percents
        if (Scr_GameController.attackMode_) {
            if (!instanciated) {
                if (Scr_GameController.redTeamTurn_) {
                    dogs = GameObject.FindGameObjectsWithTag("Blue_Team");
                } else if (Scr_GameController.blueTeamTurn_) {
                    dogs = GameObject.FindGameObjectsWithTag("Red_Team");
                }

                for (int i = 0; i < dogs.Length; i++) {
                    GameObject percentObj = Instantiate(percent, transform);
                }

                instanciated = true;
            }
        } else {
            instanciated = false;
        }
    }
}
