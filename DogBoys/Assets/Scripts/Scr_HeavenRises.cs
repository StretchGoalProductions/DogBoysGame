using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_HeavenRises : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Destroy(this.gameObject, 32.0f);
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += new Vector3(0.0f, 0.5f, 0.0f) * Time.deltaTime;
	}
}
