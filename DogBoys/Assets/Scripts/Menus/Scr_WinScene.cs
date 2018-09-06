using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Need for UI components
using UnityEngine.SceneManagement;

public class Scr_WinScene : MonoBehaviour {
    private bool winner_; //If true then red team has won, else the blue team has won
    public GameObject redDancingDog_;
    public GameObject blueDancingDog_;
    public GameObject redWinImageDisplay_;
    public GameObject blueWinImageDisplay_;
    private GameObject info;
    private GameObject oldGameController_;

    private void Awake()
    {
        info = GameObject.Find("Win_Scene_Info(Clone)");
        oldGameController_ = GameObject.Find("TeamController(Clone)");
    }

    private void Start()
    {
        winner_ = info.GetComponent<cls_Win_Screne_Info>().winner_;
        //Destroy(info);
        //Destroy(oldGameController_);

        if (winner_) //Red team has won 
        {
            redDancingDog_.SetActive(true);
            blueDancingDog_.SetActive(false);
            redWinImageDisplay_.SetActive(true);
            blueWinImageDisplay_.SetActive(false);
        }
        else //Blue team has won 
        {
            redDancingDog_.SetActive(false);
            blueDancingDog_.SetActive(true);
            redWinImageDisplay_.SetActive(false);
            blueWinImageDisplay_.SetActive(true);
        }

        Destroy(cls_Win_Screne_Info.Instance);
        Destroy(Scr_TeamController.Instance);
    }

    public void returnToDogHouse()
    {
        Destroy(cls_Win_Screne_Info.Instance);
        Destroy(Scr_TeamController.Instance);

        SceneManager.LoadScene(0);
    }
}
