using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cls_Node {

	public int gridX;
	public int gridY;
	public Vector3 position;
	public Cls_Node parent;
	public int gCost;
	public int hCost;
	public int fCost { get { return gCost + hCost; } }

	public enum nodeState {empty, player, wall, cover, pickup};
	public nodeState currentState;

	public Scr_DogBase dog;
	public Scr_SqueakyGrenadePickup grenadePickup;

	public Cls_Node (nodeState currentState, Vector3 position, int gridX, int gridY) {
		this.currentState = currentState;
		this.position = position;
		this.gridX = gridX;
		this.gridY = gridY;
	}


}
