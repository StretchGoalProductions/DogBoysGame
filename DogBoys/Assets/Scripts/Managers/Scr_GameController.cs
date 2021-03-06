﻿using System.Collections;
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
    public static bool grenadeMode_;
    public static int roundCount_;
    private static Camera_Movement cameraPivot_;
    public static string displayTeamName_;
    public static Scr_GameController Instance;
    public GameObject winInfoPrefab;
    public static GameObject winInfo_;
	private static GunEffects fx;

    public GameObject squeakyPickUp;
    private static GameObject _squeakyPickUp;
    private static bool shouldSpawnPickup = false;
    #endregion

	private void Start(){
		fx = GunEffects.Instance ();
	}

    private void Awake()
    {
        _squeakyPickUp = squeakyPickUp;
        Instance = this;
        teamInfo_ = this.gameObject.GetComponent<Scr_TeamController>();
        cameraPivot_ = Camera.main.transform.parent.GetComponent<Camera_Movement>();
        winInfo_ = Instantiate(winInfoPrefab, transform.position, transform.rotation);
        displayTeamName_ = "Blue Bandits";
        blueTeamTurn_ = true;
        redTeamTurn_ = false;
        attackMode_ = false;
        grenadeMode_ = false;
        roundCount_ = 1;
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
        Scr_TurnText.updateText();
        
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
			fx.SwitchTurn ();
            displayTeamName_ = "Blue Bandits";
        }
        else
        {
            blueTeamTurn_ = false;
            redTeamTurn_ = true;
            ResetActionCount(Scr_TeamController.redTeam);
            ChangeCameraPivot();
            roundCount_ += 1;
			fx.SwitchTurn ();
            displayTeamName_ = "Red Rovers";
        }
        Scr_TurnText.updateText();

        if (shouldSpawnPickup) {
            int x = Random.Range(0, Scr_Grid.gridSizeX - 1);
            int y = Random.Range(0, Scr_Grid.gridSizeY - 1);

            if (Scr_Grid.grid[x,y].currentState == Cls_Node.nodeState.empty) {
                Vector3 spawnPos = new Vector3(Scr_Grid.grid[x,y].position.x, 0.2f, Scr_Grid.grid[x,y].position.z);
                Instantiate(_squeakyPickUp, spawnPos, Quaternion.identity);
                //Scr_Grid.grid[x,y].grenadePickup.GetComponent<AudioSource>().Play();
                shouldSpawnPickup = false;
            }
        }
        else if (roundCount_ % 5 == 0) {
            shouldSpawnPickup = true;

            int x = Random.Range(0, Scr_Grid.gridSizeX - 1);
            int y = Random.Range(0, Scr_Grid.gridSizeY - 1);

            if (Scr_Grid.grid[x,y].currentState == Cls_Node.nodeState.empty) {
                Vector3 spawnPos = new Vector3(Scr_Grid.grid[x,y].position.x, 0.2f, Scr_Grid.grid[x,y].position.z);
                Instantiate(_squeakyPickUp, spawnPos, Quaternion.identity);
                //Scr_Grid.grid[x,y].grenadePickup.GetComponent<AudioSource>().Play();
                shouldSpawnPickup = false;
            }
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
    
    #endregion

}
