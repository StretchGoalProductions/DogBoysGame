using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cls_Win_Screne_Info : MonoBehaviour
{

    public bool winner_;
    public static cls_Win_Screne_Info Instance;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
}
