using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour {

	[SerializeField]
	private float deltaPhi;
	private Quaternion initial;

	// Use this for initialization
	void Start () {
		initial = gameObject.transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.Rotate (Vector3.right * deltaPhi * Time.deltaTime);
		if (gameObject.transform.rotation.x > 145) {
			gameObject.transform.rotation = initial;
		}
	}
}
