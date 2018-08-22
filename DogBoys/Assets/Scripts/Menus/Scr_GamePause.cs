using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Pause : MonoBehaviour {

    [SerializeField]
    private GameObject alwaysPanel;
    [SerializeField]
    private GameObject pausePanel;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    #region actives
    //so I was having trouble having new functions show up for use with the buttons, so sorry these are poorly named for what they do
    public void exitOn()
    {
        //make the close pause menu button visible/open button invisible
        alwaysPanel.transform.GetChild(1).gameObject.SetActive(true);
        alwaysPanel.transform.GetChild(0).gameObject.SetActive(false);
        //show menu
        pausePanel.SetActive(true);
        //pause game
        //----coming soon

    }

    public void goToOn()
    {
        //make the pause button visible/close button invisible
        alwaysPanel.transform.GetChild(0).gameObject.SetActive(true);
        alwaysPanel.transform.GetChild(1).gameObject.SetActive(false);
        //hide menu
        pausePanel.SetActive(false);
        //unpause game
        //----coming soon
    }
    #endregion

}
