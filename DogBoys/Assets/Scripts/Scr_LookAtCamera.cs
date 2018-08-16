using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_LookAtCamera : MonoBehaviour {

	void Update () {
        transform.LookAt(Camera.main.transform);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, Mathf.Round(transform.eulerAngles.y / 90) * 90, transform.eulerAngles.z);
	}
}
