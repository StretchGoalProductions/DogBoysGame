using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_movement : MonoBehaviour
{
    #region variables
    //positions for movement
    [SerializeField]
    private Vector3 start; //= new Vector3(0,5,-23);
    [SerializeField]
    private Vector3 draft; // = new Vector3(0,6,11.5);
    private Quaternion draftRotate = Quaternion.Euler(0f, 0f, 0f);
    [SerializeField]
    private Vector3 level; // = new Vector3(11,7,12);
    private Quaternion levelRotate = Quaternion.Euler(0f,90f,0f);
    [SerializeField]
    private Vector3 options; // = new Vector3(-11,7,12);
    private Quaternion optionsRotate = Quaternion.Euler(0f, -90f, 0f);
    [SerializeField]
    private Vector3 howTo; // = new Vector3(0,5,10.5);
    private Quaternion howToRotate = Quaternion.Euler(0f, 180f, 0f);
    //interactive objects w/ scripts
    [SerializeField]
    private GameObject door_l;
    [SerializeField]
    private GameObject door_r;
    //lerp times
    private float doorTime = 2.5f;
    private float barTime = 1.5f;
    private float currentLerpTime = 0;
    //action # -- 0 = stationary, 1-7 = moving
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
    [SerializeField]
    private GameObject howToPanel;
    [SerializeField]
    private GameObject alwaysPanel;
    //for how to movement
    [SerializeField]
    private Vector3 previous;
    [SerializeField]
    private GameObject last;
    private Quaternion lastR;
    #endregion

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
                    door_l.GetComponent<open_door>().setOpen(0);   //open left door
                    door_r.GetComponent<open_door>().setOpen(1);   //open right door
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
                else if (hit.collider.gameObject.name == "How_To_Play"){
                    //levelPanel.SetActive(false);

                    howToPanel.SetActive(!howToPanel.activeSelf);

                    //action = 6;
                }
            }
        }
        #endregion

        #region movement
        //check movement
        //move through door -> draft
        if (action == 1)
        {
            moveTo(start, draftRotate, draft, draftRotate, doorTime);
            if (checkPosition(draft))
            {
                action = 0;
                draftPanel.SetActive(true);
                alwaysPanel.SetActive(true);
            }
        }
        //draft -> options
        else if (action == 2)
        {
            moveTo(draft, draftRotate, options, optionsRotate, barTime);
            if (checkPosition(options))
            {
                action = 0;
                optionsPanel.SetActive(true);
            }
        }

        //options -> draft
        else if (action == 3)
        {
            moveTo(options, optionsRotate, draft, draftRotate, barTime);
            if (checkPosition(draft))
            {
                action = 0;
                draftPanel.SetActive(true);
            }
        }

        //draft -> level
        else if (action == 4)
        {
            moveTo(draft, draftRotate, level, levelRotate, barTime);
            if (checkPosition(level))
            {
                action = 0;
                levelPanel.SetActive(true);
            }
        }
        //level -> draft
        else if (action == 5)
        {
            moveTo(level, levelRotate, draft, draftRotate, barTime);
            if(checkPosition(draft)){
                action = 0;
                draftPanel.SetActive(true);
            }
        }
        //anywhere -> HowTo
        else if (action == 6){
            if(checkPosition(draft)){
                previous = draft;
            }
            else if(checkPosition(level)){
                previous = level;
            }
            else if (checkPosition(options))
            {
                previous = options;
            }
            last.SetActive(false);
            alwaysPanel.transform.GetChild(0).gameObject.SetActive(false);
            moveHowTo(previous, howTo, barTime);
            if (checkPosition(howTo))
            {
                action = 0;
                alwaysPanel.transform.GetChild(1).gameObject.SetActive(true);
                howToPanel.SetActive(true);
            }
        }
        //HowTo -> anywhere
        else if(action == 7){
            alwaysPanel.transform.GetChild(1).gameObject.SetActive(false);
            moveHowTo(howTo, previous, barTime);
            if (checkPosition(previous))
            {
                action = 0;
                alwaysPanel.transform.GetChild(0).gameObject.SetActive(true);
                last.SetActive(true);
            }
        }
        //movement complete -- reset currentLerpTime
        else if (action == 0)
        {
            currentLerpTime = 0;
        }
        #endregion
    
    }

    void moveTo(Vector3 from, Quaternion fromR, Vector3 to, Quaternion toR, float maxTime)
    {
        currentLerpTime += Time.deltaTime;
        if (currentLerpTime >= maxTime)
        {
            currentLerpTime = maxTime;
        }
        float perc = currentLerpTime / maxTime;
        perc = perc*perc*perc *(perc * (6f*perc - 15f) + 10f);
        this.gameObject.transform.position = Vector3.Lerp(from, to, perc);
        this.gameObject.transform.rotation = Quaternion.Lerp(fromR, toR, perc);
    }

    void moveHowTo(Vector3 from, Vector3 to, float maxTime)
    {
        Quaternion toHow = Quaternion.Euler(0f, 180f, 0f);
        Quaternion fromHow = setLastR(previous);
        currentLerpTime += Time.deltaTime;
        if (currentLerpTime >= maxTime)
        {
            currentLerpTime = maxTime;
        }
        float perc = currentLerpTime / maxTime;
        perc = perc * perc * perc * (perc * (6f * perc - 15f) + 10f);
        if(to == howTo){
            this.gameObject.transform.position = Vector3.Lerp(from, to, perc);
            this.gameObject.transform.rotation = Quaternion.Lerp(this.gameObject.transform.rotation, toHow, perc);
        }else{
            this.gameObject.transform.position = Vector3.Lerp(from, to, perc);
            this.transform.rotation = Quaternion.Lerp(transform.rotation, fromHow, perc);
        }
    }

    GameObject setLast(Vector3 now)
    {
        if(now == draft){
            return draftPanel;
        }
        else if (now == level)
        {
            return levelPanel;

        }
        else if (now == options)
        {
            return optionsPanel;
        }
        else
        {
            return null;
        }
    }

    Quaternion setLastR(Vector3 now)
    {
        if (now == draft)
        {
            return draftRotate;
        }
        else if (now == level)
        {
            return levelRotate;

        }
        else if (now == options)
        {
            return optionsRotate;
        }
        else
        {
            return draftRotate; //draft == default 0,0,0
        }
    }

    bool checkPosition(Vector3 check)
    {
        if (this.transform.position == check)
            return true;
        else
            return false;
    }

    public void actionHowTo(){
        last = setLast(this.transform.position);
        action = 6;
    }

    public void actionFromHowTo(){
        howToPanel.SetActive(false);
        action = 7;
    }
}
