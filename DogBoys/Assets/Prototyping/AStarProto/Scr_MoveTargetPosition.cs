using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_MoveTargetPosition : MonoBehaviour {
    
	public Camera mainCamera;
	public LayerMask hitLayers;
    
	public void Start() {
		mainCamera = GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponentInChildren<Camera>();
	}

	void Update () {
		if(Input.GetMouseButtonDown(0)) {
			Vector3 mouse = Input.mousePosition;
			Ray castPoint = mainCamera.ScreenPointToRay(mouse);

			RaycastHit hit;
			
			if(Physics.Raycast(castPoint, out hit, Mathf.Infinity, hitLayers)) {
				transform.position = hit.point;
			}
		}
		
	}
}
