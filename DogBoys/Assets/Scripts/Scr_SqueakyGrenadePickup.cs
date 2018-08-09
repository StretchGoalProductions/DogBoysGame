using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_SqueakyGrenadePickup : MonoBehaviour {

	public Cls_Node currentNode;

	void Start () {
		currentNode = Scr_Grid.NodeFromWorldPosition(transform.position);
		currentNode.currentState = Cls_Node.nodeState.pickup;
		currentNode.grenadePickup = this;
	}
	
	public void pickUp() {
		currentNode.grenadePickup = null;
		currentNode.dog.grenadesHeld += 1;
		Destroy(gameObject);
		currentNode.currentState = Cls_Node.nodeState.player;
	}
}
