using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Scr_GamePause : MonoBehaviour {

    [SerializeField]
    private GameObject alwaysPanel;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void exitOn()
    {
        alwaysPanel.transform.GetChild(1).gameObject.SetActive(true);
    }

    public void goToOn()
    {
        alwaysPanel.transform.GetChild(0).gameObject.SetActive(true);
    }
}
