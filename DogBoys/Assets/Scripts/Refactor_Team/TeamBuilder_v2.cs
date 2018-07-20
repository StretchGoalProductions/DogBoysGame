using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamBuilder_v2 : MonoBehaviour
{
    #region variables
    #region prefabs
    //prefabs - depending on how characters are implemented, we can probably streamline these. For now, this is how we did it last time
    [SerializeField]
    private GameObject revolverDogR;	//red dog
    [SerializeField]
    private GameObject revolverDogB;	//blue dog
    [SerializeField]
    private GameObject rifleDogR;	//red dog
    [SerializeField]
    private GameObject rifleDogB;	//blue dog
    [SerializeField]
    private GameObject shotgunDogR;	//red dog
    [SerializeField]
    private GameObject shotgunDogB;	//blue dog
    #endregion
    //team list - strings (from draft)
    [SerializeField]
    private List<string> redTeamProto = new List<string>();
    [SerializeField]
    private List<string> blueTeamProto = new List<string>();
    //team controller
    [SerializeField]
    private GameObject teamController;
    //team spawn positions
    [SerializeField]
    private GameObject[] redSpawn;
    [SerializeField]
    private GameObject[] blueSpawn;
    //team lists - final
    [SerializeField]
    private List<GameObject> redTeam = new List<GameObject>();
    [SerializeField]
    private List<GameObject> blueTeam = new List<GameObject>();
    #endregion

    // Use this for initialization
	void Start () {
        //create the teams from the list-of-strings from draft
        Spawner(redTeamProto, blueTeamProto);
        //send team info to team controller
        teamController = GameObject.Find("TeamController");
        teamController.GetComponent<TeamController>().setRed(redTeam);
        teamController.GetComponent<TeamController>().setBlue(blueTeam);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    List<GameObject> getRed()
    {
        return redTeam;
    }

    List<GameObject> getBlue()
    {
        return blueTeam;
    }

    #region spawner
    void Spawner(List<string> red, List<string> blue)
    {
        int i = 0, j = 0;
        //create red team
        foreach (string n in red)
        {
            buildRed(n, redSpawn[i].transform.position);
            i++;
        }
        //create blue team
        foreach (string m in blue)
        {
            buildBlue(m, blueSpawn[j].transform.position);
            j++;
        }
    }

    void buildRed(string type, Vector3 location)
    {
        location.y = 0.5f;	//offset so dogs are on the tiles properly
        switch (type)
        {		//create new dog based on type
            case "rv":
                GameObject newRevolver = Instantiate(revolverDogR, location, Quaternion.identity);
                newRevolver.transform.Rotate(0, 180, 0, Space.Self);//rotates dog so it faces the right direction
                redTeam.Add(newRevolver);		//add new dog to player 1s characters in game controller
                break;
            case "rf":
                GameObject newRifle = Instantiate(rifleDogR, location, Quaternion.identity);
                newRifle.transform.Rotate(0, 180, 0, Space.Self);	//rotates dog so it faces the right direction
                redTeam.Add(newRifle);		//add new dog to player 1s characters in game controller
                break;
            case "sg":
                GameObject newShotgun = Instantiate(shotgunDogR, location, Quaternion.identity);
                newShotgun.transform.Rotate(0, 180, 0, Space.Self);//rotates dog so it faces the right direction
                redTeam.Add(newShotgun);		//add new dog to player 1s characters in game controller
                break;
        }
    }

    void buildBlue(string type, Vector3 location)
    {
        location.y = 0.5f;	//offset so dogs are on the tiles properly
        switch (type)
        {		//create new dog based on type
            case "rv":
                GameObject newRevolver = Instantiate(revolverDogB, location, Quaternion.identity);
                blueTeam.Add(newRevolver);		//add new dog to player 1s characters in game controller
                break;
            case "rf":
                GameObject newRifle = Instantiate(rifleDogB, location, Quaternion.identity);
                blueTeam.Add(newRifle);		//add new dog to player 1s characters in game controller
                break;
            case "sg":
                GameObject newShotgun = Instantiate(shotgunDogB, location, Quaternion.identity);
                blueTeam.Add(newShotgun);		//add new dog to player 1s characters in game controller
                break;
        }
    }
    #endregion

}
