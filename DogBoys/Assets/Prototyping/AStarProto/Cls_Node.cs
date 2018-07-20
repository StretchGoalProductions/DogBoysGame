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

	public enum nodeState {empty, player, wall};
	public nodeState currentState;

	public Cls_Node (nodeState a_currentState, Vector3 b_position, int c_gridX, int d_gridY) {
		currentState = a_currentState;
		position = b_position;
		gridX = c_gridX;
		gridY = d_gridY;
	}


}
