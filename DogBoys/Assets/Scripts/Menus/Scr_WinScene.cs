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
        Destroy(info);
        Destroy(oldGameController_);
    }

    public void Update()
    {
        screneUpdate();
    }

    private void screneUpdate()
    {
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
    }

    public void returnToDogHouse()
    {
        SceneManager.LoadScene(0);
    }
}
