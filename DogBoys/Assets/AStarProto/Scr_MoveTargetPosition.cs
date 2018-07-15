﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_MoveTargetPosition : MonoBehaviour {

	public LayerMask hitLayers;
	void Update () {
		if(Input.GetMouseButtonDown(0)) {
			Vector3 mouse = Input.mousePosition;
			Ray castPoint = Camera.main.ScreenPointToRay(mouse);

			RaycastHit hit;
			
			if(Physics.Raycast(castPoint, out hit, Mathf.Infinity, hitLayers)) {
				transform.position = hit.point;
			}
		}
		
	}
}
