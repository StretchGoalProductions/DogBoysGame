using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Scr_LevelSelectButton : MonoBehaviour {
    
    public string level;

    public void LoadMission()
    {
        Debug.Log("Loading");
        SceneManager.LoadScene(level);
    }
}
