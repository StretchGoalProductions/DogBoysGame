using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cls_Win_Screne_Info : MonoBehaviour
{

    public bool winner_;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
