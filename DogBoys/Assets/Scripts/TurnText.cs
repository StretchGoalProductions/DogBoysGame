using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnText : MonoBehaviour {

	private GameController gc;

	// Use this for initialization
	void Start () {
		gc = GameController.Instance;
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.GetComponent<Text>().text = gc.turn;
	}
}
