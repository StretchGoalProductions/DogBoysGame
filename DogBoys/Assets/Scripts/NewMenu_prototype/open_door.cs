using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class open_door : MonoBehaviour {

    public GameObject pivot;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void open(int num)
    {
        if (num == 0)   //0 = left door
        {
            Debug.Log("Left");
            pivot.transform.Rotate(0, -90f, 0);
        }
        if (num == 1)   //1 = right door
        {
            Debug.Log("Right");
            pivot.transform.Rotate(0, 90f, 0);
        }
    }
}
