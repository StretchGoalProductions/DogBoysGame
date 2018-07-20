using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamController : MonoBehaviour
{
    #region variables
    //team-related
    [SerializeField]
    private GameObject teamBuilder;
    [SerializeField]
    public List<GameObject> redTeam = new List<GameObject>();
    [SerializeField]
    public List<GameObject> blueTeam = new List<GameObject>();
    #endregion

    // Use this for initialization
	void Start ()
    {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setRed(List<GameObject> red)
    {
        redTeam = red;
    }

    public void setBlue(List<GameObject> blue)
    {
        blueTeam = blue;
    }
}
