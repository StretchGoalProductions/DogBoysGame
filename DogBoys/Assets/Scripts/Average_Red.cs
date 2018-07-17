using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Average_Red : MonoBehaviour {
	
	private GameObject[] Dogs;

	void Update () {
		Dogs = null;
		Dogs = GameObject.FindGameObjectsWithTag("Red_Team");

		float x = 0f, y = 0f;
		for (int i = 0; i < Dogs.Length; i++) {
			x += Dogs[i].transform.position.x;
			y += Dogs[i].transform.position.z;
		}

		x /= Dogs.Length;
		y /= Dogs.Length;
		transform.position = new Vector3(x, transform.position.y, y);
	}
}
