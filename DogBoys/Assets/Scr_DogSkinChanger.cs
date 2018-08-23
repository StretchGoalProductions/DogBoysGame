using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_DogSkinChanger : MonoBehaviour {

    public List<Texture> skins;
    public GameObject thisDog;

	// Use this for initialization
	void Start () {
        Random.seed = (int)System.DateTime.Now.Ticks;
        int i = Random.Range(0, skins.Count); ;
        thisDog.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", skins[i]);
	}
	
}
