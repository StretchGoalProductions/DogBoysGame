using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teamBuilder : MonoBehaviour {
	//prefabs
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
	//draft ish
	[SerializeField]
	private GameObject fromDraft;
	[SerializeField]
	private List<string> redTeam = new List<string>();
	[SerializeField]
	private List<string> blueTeam = new List<string>();
	//team spawn positions
	[SerializeField]
	private GameObject[] redSpawn;
	[SerializeField]
	private GameObject[] blueSpawn;
	//game controller
	private GameController gc;

	// Use this for initialization
	void Start () {
		gc = GameController.Instance;
		//get object with team comp
		fromDraft = GameObject.Find("Game_Constants");
		//get the spawn locations
		redSpawn = GameObject.FindGameObjectsWithTag("redSpawn");
		blueSpawn = GameObject.FindGameObjectsWithTag("blueSpawn");
		//get the teams
		redTeam = fromDraft.GetComponent<Constants_Values>().getRed();
		blueTeam = fromDraft.GetComponent<Constants_Values> ().getBlue();
		//spawn teams
		deployRed();
		deployBlue ();
	}
	
	void deployRed(){
		int i = 0;
		foreach (string n in redTeam) {
			//Instantiate (prefab1,new Vector3(3,4,3),Quaternion.identity);
			createRed(n,redSpawn[i].transform.position);
			i++;
		}
	}

	void deployBlue(){
		int i = 0;
		foreach (string n in blueTeam) {
			//Instantiate (prefab1,new Vector3(3,4,3),Quaternion.identity);
			createBlue(n,blueSpawn[i].transform.position);
			i++;
		}
	}

	void createRed(string type,Vector3 location){
		//GameObject newDog = Instantiate (revolverDog,location,Quaternion.identity);
		location.y = 0.5f;	//offset so dogs are on the tiles properly
		switch (type) {		//create new dog based on type
		case "rv":
			GameObject newRevolver = Instantiate (revolverDogR, location, Quaternion.identity);	
			newRevolver.transform.Rotate(0, 180, 0, Space.Self);//rotates dog so it faces the right direction
			gc.p2Chars.Add(newRevolver);		//add new dog to player 1s characters in game controller
			break;
		case "rf":
			GameObject newRifle = Instantiate (rifleDogR, location, Quaternion.identity);
			newRifle.transform.Rotate(0, 180, 0, Space.Self);	//rotates dog so it faces the right direction
			gc.p2Chars.Add(newRifle);		//add new dog to player 1s characters in game controller
			break;
		case "sg":
			GameObject newShotgun = Instantiate (shotgunDogR, location, Quaternion.identity);
			newShotgun.transform.Rotate(0, 180, 0, Space.Self);//rotates dog so it faces the right direction
			gc.p2Chars.Add(newShotgun);		//add new dog to player 1s characters in game controller
			break;
		}
	}

	void createBlue(string type, Vector3 location){
		//GameObject newDog = Instantiate (revolverDog,location,Quaternion.identity);
		location.y = 0.5f;	//offset so dogs are on the tiles properly
		switch (type) {		//create new dog based on type
		case "rv":
			GameObject newRevolver = Instantiate (revolverDogB, location, Quaternion.identity);
			gc.p1Chars.Add(newRevolver);		//add new dog to player 1s characters in game controller
			break;
		case "rf":
			GameObject newRifle = Instantiate (rifleDogB, location, Quaternion.identity);
			gc.p1Chars.Add(newRifle);		//add new dog to player 1s characters in game controller
			break;
		case "sg":
			GameObject newShotgun = Instantiate (shotgunDogB, location, Quaternion.identity);
			gc.p1Chars.Add(newShotgun);		//add new dog to player 1s characters in game controller
			break;
		}
	}
}
