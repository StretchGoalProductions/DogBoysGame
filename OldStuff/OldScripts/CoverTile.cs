using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverTile : MonoBehaviour {
    #region Variables and Declarations
    [SerializeField]
    private Constants.Global.C_CoverTypeAndDirection[] dirCoveringPlayer;
    #endregion

    #region CoverTile Methods
    public Constants.Global.C_CoverTypeAndDirection[] GetDirection() {
        return dirCoveringPlayer;
    }
    #endregion

    #region Unity Overrides
    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {

    }
    #endregion
}
