using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_movement : MonoBehaviour {

    //positions for movement
    [SerializeField]
    private Vector3 start; //= new Vector3(0,5,-23);
    [SerializeField]
    private Vector3 draft; // = new Vector3(0,6,11.5);
    [SerializeField]
    private Vector3 level; // = new Vector3(11,7,12);
    [SerializeField]
    private Vector3 options; // = new Vector3(-11,7,12);
    //interactive objects w/ scripts
    [SerializeField]
    private GameObject door_l;
    [SerializeField]
    private GameObject door_r;
    //lerp times
    private float doorTime = 5;
    private float barTime = 3;
    private float currentLerpTime = 0;
    //action #
    int action;

	// Use this for initialization
	void Start () {
        action = 0;
	}
	
	// Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100f))
            {
                if(hit.transform.tag == "Menu_door"){
                    //Debug.Log("door clicked");
                    door_l.GetComponent<open_door>().open(0);   //open left door
                    door_r.GetComponent<open_door>().open(1);   //open right door
                    action = 1;
                }else if (hit.collider.gameObject.name == "To_Options"){
                    action = 2;
                }
                else if (hit.collider.gameObject.name == "From_Options"){
                    action = 3;
                }
                else if (hit.collider.gameObject.name == "To_Level"){
                    action = 4;
                }
                else if (hit.collider.gameObject.name == "From_Level"){
                    action = 5;
                }
            }
        }

        //move through door
        if (action == 1)
        {
            currentLerpTime += Time.deltaTime;
            if (currentLerpTime >= doorTime)
            {
                currentLerpTime = doorTime;
            }
            float perc = currentLerpTime / doorTime;
            this.gameObject.transform.position = Vector3.Lerp(start, draft, perc);
        }

        //draft -> options
        if (action == 2)
        {
            currentLerpTime += Time.deltaTime;
            if (currentLerpTime >= barTime)
            {
                currentLerpTime = barTime;
            }
            float perc = currentLerpTime / barTime;
            this.gameObject.transform.position = Vector3.Lerp(draft, options, perc);
        }

        //options -> draft
        if (action == 3)
        {
            currentLerpTime += Time.deltaTime;
            if (currentLerpTime >= barTime)
            {
                currentLerpTime = barTime;
            }
            float perc = currentLerpTime / barTime;
            this.gameObject.transform.position = Vector3.Lerp(options, draft, perc);
        }

        //draft -> level
        if (action == 4)
        {
            currentLerpTime += Time.deltaTime;
            if (currentLerpTime >= barTime)
            {
                currentLerpTime = barTime;
            }
            float perc = currentLerpTime / barTime;
            this.gameObject.transform.position = Vector3.Lerp(draft, level, perc);
        }

        //level -> draft
        if (action == 5)
        {
            currentLerpTime += Time.deltaTime;
            if (currentLerpTime >= barTime)
            {
                currentLerpTime = barTime;
            }
            float perc = currentLerpTime / barTime;
            this.gameObject.transform.position = Vector3.Lerp(level, draft, perc);
        }

    }
}
