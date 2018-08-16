using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class open_door : MonoBehaviour {

    public GameObject pivot;
    
    private int index;
    private int action; //0 = inactive, 1 = active
    private Quaternion doorOpen;
    private float currentLerpTime = 0;
    private float openTime = 1.5f;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (action == 1)
        {
            open();
        }
	}

    public void setOpen(int num)
    {
        index = num;
        action = 1;
    }

    void open()
    {
        currentLerpTime += Time.deltaTime;
        if (currentLerpTime >= openTime)
        {
            currentLerpTime = openTime;
        }
        float perc = currentLerpTime / openTime;
        perc = perc * perc * perc * (perc * (6f * perc - 15f) + 10f);


        if (index == 0)   //0 = left door
        {
            doorOpen = Quaternion.Euler(0f, -90f, 0f);
        }
        if (index == 1)   //1 = right door
        {
            doorOpen = Quaternion.Euler(0f, 90f, 0f);
        }

        pivot.transform.rotation = Quaternion.Lerp(pivot.transform.rotation, doorOpen, perc);
    }

    public void reset()
    {
        pivot.transform.Rotate(0, 0, 0);
        action = 0;
    }
}
