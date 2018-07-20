using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DogClassProto_TurnText : MonoBehaviour {

	private DogClassProto_GameController gc;

	// Use this for initialization
	void Start () {
		gc = DogClassProto_GameController.Instance;
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.GetComponent<Text>().text = gc.turn;
	}
}
