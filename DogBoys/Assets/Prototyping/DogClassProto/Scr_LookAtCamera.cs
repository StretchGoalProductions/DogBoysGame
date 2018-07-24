using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_LookAtCamera : MonoBehaviour {

	void Update () {
        float z = transform.localEulerAngles.z;
        transform.LookAt(Camera.main.transform);
	}
}
