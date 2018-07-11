using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cls_Node {

	public int gridX;
	public int gridY;
	public bool isWall;
	public Vector3 position;
	public Cls_Node parent;
	public int gCost;
	public int hCost;
	public int fCost { get { return gCost + hCost; } }

	public Cls_Node (bool a_isWall, Vector3 b_position, int c_gridX, int d_gridY) {
		isWall = a_isWall;
		position = b_position;
		gridX = c_gridX;
		gridY = d_gridY;
	}


}
