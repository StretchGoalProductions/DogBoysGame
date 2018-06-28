using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tile_effect : MonoBehaviour {

	public bool on;

	// Use this for initialization
	void Start () {
		on = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (on == false)
			Destroy (this.gameObject);
	}
}
