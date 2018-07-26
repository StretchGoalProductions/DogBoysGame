using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_movement : MonoBehaviour
{
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
    private float doorTime = 2.5f;
    private float barTime = .5f;
    private float currentLerpTime = 0;
    //action # -- 0 = stationary, 1-5 = moving
    int action;
    //menus
    [SerializeField]
    private GameObject titlePanel;
    [SerializeField]
    private GameObject draftPanel;
    [SerializeField]
    private GameObject optionsPanel;
    [SerializeField]
    private GameObject levelPanel;

	// Use this for initialization
	void Start () {
        action = 0;
	}
	
	// Update is called once per frame
    void Update()
    {
        #region mouse input
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
                    titlePanel.SetActive(false);
                    action = 1;
                }else if (hit.collider.gameObject.name == "To_Options"){
                    draftPanel.SetActive(false);
                    action = 2;
                }
                else if (hit.collider.gameObject.name == "From_Options"){
                    optionsPanel.SetActive(false);
                    action = 3;
                }
                else if (hit.collider.gameObject.name == "To_Level"){
                    draftPanel.SetActive(false);
                    action = 4;
                }
                else if (hit.collider.gameObject.name == "From_Level"){
                    levelPanel.SetActive(false);
                    action = 5;
                }
            }
        }
        #endregion

        #region movement
        //check movement
        //move through door -> draft
        if (action == 1)
        {
            moveTo(start, draft, doorTime);
            if (checkPosition(draft))
            {
                action = 0;
                draftPanel.SetActive(true);
            }
        }
        //draft -> options
        else if (action == 2)
        {
            moveTo(draft, options, barTime);
            if (checkPosition(options))
            {
                action = 0;
                optionsPanel.SetActive(true);
            }
        }

        //options -> draft
        else if (action == 3)
        {
            moveTo(options, draft, barTime);
            if (checkPosition(draft))
            {
                action = 0;
                draftPanel.SetActive(true);
            }
        }

        //draft -> level
        else if (action == 4)
        {
            moveTo(draft, level, barTime);
            if (checkPosition(level))
            {
                action = 0;
                levelPanel.SetActive(true);
            }
        }
        //level -> draft
        else if (action == 5)
        {
            moveTo(level, draft, barTime);
            if(checkPosition(draft)){
                action = 0;
                draftPanel.SetActive(true);
            }
        }
        //movement complete -- reset currentLerpTime
        else if (action == 0)
        {
            currentLerpTime = 0;
        }
        #endregion
    
    }

    void moveTo(Vector3 from, Vector3 to, float maxTime)
    {
        currentLerpTime += Time.deltaTime;
        if (currentLerpTime >= maxTime)
        {
            currentLerpTime = maxTime;
        }
        float perc = currentLerpTime / maxTime;
        this.gameObject.transform.position = Vector3.Lerp(from, to, perc);
    }

    bool checkPosition(Vector3 check)
    {
        if (this.transform.position == check)
            return true;
        else
            return false;
    }

}
