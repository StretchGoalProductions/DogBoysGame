﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scr_DogListable : MonoBehaviour {

	public GameObject left;
	public GameObject right;
	public Text text;
	public Text countText;
	public int count;
    public int team;    //0 = available, 1 = blue, 2 = red
	//panels
	private GameObject listAvailable;
	private GameObject listBlue;
	private GameObject listRed;
	//character this object represents
	[SerializeField]
	private string thisDog;//dog type

	//sound
	[SerializeField]
	private AudioSource click;

	public void deincrementCount(){
		count--;	//deincrement count
		countText.text = count.ToString();	//update display
	}
	public void incrementCount(){
		count ++;	//increment count
		countText.text = count.ToString();	//update display
	}

	void Start () {
		Button lButton = left.GetComponent<Button> ();
		Button rButton = right.GetComponent<Button>();

		lButton.onClick.AddListener (AddLeft);
		rButton.onClick.AddListener (AddRight);
		//get proper object based on type
		listAvailable = GameObject.Find("Dog_Available");
		listBlue= GameObject.Find("Blue_Drafted");
		listRed= GameObject.Find("Red_Drafted");

		//determine text to be displayed based on type
		switch (thisDog){
		case "rv":
			text.text = "Revolver";
			//get proper object based on type
			listAvailable = GameObject.Find("Available_Revolver");
			listBlue= GameObject.Find("Blue_Revolver");
			listRed= GameObject.Find("Red_Revolver");
			break;
		case "rf":
			text.text = "Rifle";
			//get proper object based on type
			listAvailable = GameObject.Find("Available_Rifle");
			listBlue= GameObject.Find("Blue_Rifle");
			listRed= GameObject.Find("Red_Rifle");
			break;
		case "sg":
			text.text = "Shotgun";
			//get proper object based on type
			listAvailable = GameObject.Find("Available_Shotgun");
			listBlue= GameObject.Find("Blue_Shotgun");
			listRed= GameObject.Find("Red_Shotgun");
			break;
		}
		countText.text = count.ToString();
	}

	void AddLeft(){
		click.Play ();
		if (count != 0) {
			if (transform.parent.name == "Red_Drafted") {
				deincrementCount ();
				Scr_TeamController.moveDog (thisDog,Scr_TeamController.menuRedDogs,Scr_TeamController.menuAvailableDogs);
				listAvailable.GetComponent<Scr_DogListable> ().incrementCount ();
			}
			else if (transform.parent.name == "Dog_Available") {
				deincrementCount ();
				Scr_TeamController.moveDog (thisDog,Scr_TeamController.menuAvailableDogs,Scr_TeamController.menuBlueDogs);
				listBlue.GetComponent<Scr_DogListable> ().incrementCount ();
			}
		}
	}
	void AddRight(){
		click.Play ();
		if (count != 0) {
			if (transform.parent.name == "Blue_Drafted") {
				deincrementCount ();
				Scr_TeamController.moveDog (thisDog,Scr_TeamController.menuBlueDogs,Scr_TeamController.menuAvailableDogs);
				listAvailable.GetComponent<Scr_DogListable> ().incrementCount ();
			}
			else if (transform.parent.name == "Dog_Available") {
				deincrementCount ();
				Scr_TeamController.moveDog (thisDog,Scr_TeamController.menuAvailableDogs,Scr_TeamController.menuRedDogs);
				listRed.GetComponent<Scr_DogListable> ().incrementCount ();
			}
		}
	}

    void Update()
    {
        if (Scr_TeamController.menuBlueDogs.Count == 5 && team != 2)
        {
            left.SetActive(false);
        }
        else if (team != 1)
        {
            left.SetActive(true);
        }

        if (Scr_TeamController.menuRedDogs.Count == 5 && team != 1)
        {
            right.SetActive(false);
        }
        else if (team != 2)
        {
            right.SetActive(true);
        }
    }
}
