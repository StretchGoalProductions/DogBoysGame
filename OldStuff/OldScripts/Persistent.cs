using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Persistent : MonoBehaviour {
	#region test code
	/*				Test code
	private int n;
	// Use this for initialization
	void Start () {
		n = 0;
	}

	// Update is called once per frame
	void Update () {
		Debug.Log ("I'm still here" + n);
		n++;
	}*/
	#endregion

	void Awake(){
		DontDestroyOnLoad (this.gameObject);	//keeps object persistent
	}
}
