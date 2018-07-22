using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_TeamBuilder : MonoBehaviour
{
    #region variables
    #region prefabs
    public GameObject tcPrefab;

    public GameObject revolverDogRed;
    public GameObject revolverDogBlue;
    public GameObject rifleDogRed;
    public GameObject rifleDogBlue;
    public GameObject shotgunDogRed;
    public GameObject shotgunDogBlue;
    #endregion

    public Vector3[] redSpawn;
    public Vector3[] blueSpawn;

    private List<GameObject> redTeam = new List<GameObject>();
    private List<GameObject> blueTeam = new List<GameObject>();
    #endregion

    void Awake() {
        if(Scr_TeamController.Instance == null) {
            Instantiate(tcPrefab);
            Scr_TeamController.menuBlueDogs.Add("rf");
            Scr_TeamController.menuBlueDogs.Add("rf");
            Scr_TeamController.menuBlueDogs.Add("rf");
            Scr_TeamController.menuRedDogs.Add("rf");
            Scr_TeamController.menuRedDogs.Add("rf");
            Scr_TeamController.menuRedDogs.Add("rf");
        }
    }


	void Start () {
        //create the teams from the list-of-strings from draft
        Spawner(Scr_TeamController.menuRedDogs, Scr_TeamController.menuBlueDogs);
        //send team info to team controller
        Scr_TeamController.redTeam = redTeam;
        Scr_TeamController.blueTeam = blueTeam;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    #region spawner
    void Spawner(List<string> red, List<string> blue)
    {
        int i = 0, j = 0;
        //create red team
        foreach (string n in red)
        {
           BuildRed(n, redSpawn[i]);
            i++;
        }
        //create blue team
        foreach (string m in blue)
        {
            BuildBlue(m, blueSpawn[j]);
            j++;
        }
    }

    void BuildRed(string type, Vector3 location)
    {
        //location.y = 0.5f;	//offset so dogs are on the tiles properly
        switch (type)
        {		//create new dog based on type
            case "rv":
                GameObject newRevolver = Instantiate(revolverDogRed, location, Quaternion.identity);
                newRevolver.transform.Rotate(0, 180, 0, Space.Self);//rotates dog so it faces the right direction
                redTeam.Add(newRevolver);		//add new dog to player 1s characters in game controller
                break;
            case "rf":
                GameObject newRifle = Instantiate(rifleDogRed, location, Quaternion.identity);
                newRifle.transform.Rotate(0, 180, 0, Space.Self);	//rotates dog so it faces the right direction
                redTeam.Add(newRifle);		//add new dog to player 1s characters in game controller
                break;
            case "sg":
                GameObject newShotgun = Instantiate(shotgunDogRed, location, Quaternion.identity);
                newShotgun.transform.Rotate(0, 180, 0, Space.Self);//rotates dog so it faces the right direction
                redTeam.Add(newShotgun);		//add new dog to player 1s characters in game controller
                break;
        }
    }

    void BuildBlue(string type, Vector3 location)
    {
        //location.y = 0.5f;	//offset so dogs are on the tiles properly
        switch (type)
        {		//create new dog based on type
            case "rv":
                GameObject newRevolver = Instantiate(revolverDogBlue, location, Quaternion.identity);
                blueTeam.Add(newRevolver);		//add new dog to player 1s characters in game controller
                break;
            case "rf":
                GameObject newRifle = Instantiate(rifleDogBlue, location, Quaternion.identity);
                blueTeam.Add(newRifle);		//add new dog to player 1s characters in game controller
                break;
            case "sg":
                GameObject newShotgun = Instantiate(shotgunDogBlue, location, Quaternion.identity);
                blueTeam.Add(newShotgun);		//add new dog to player 1s characters in game controller
                break;
        }
    }
    #endregion

}
