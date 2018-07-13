using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cls_Node {

	public int gridX;
	public int gridY;
	public bool isWall;
	public bool isPlayer;
	public Vector3 position;
	public Cls_Node parent;
	public int gCost;
	public int hCost;
	public int fCost { get { return gCost + hCost; } }

	public Cls_Node (bool a_isWall, bool b_isPlayer, Vector3 c_position, int d_gridX, int e_gridY) {
		isWall = a_isWall;
		isPlayer = b_isPlayer;
		position = c_position;
		gridX = d_gridX;
		gridY = e_gridY;
	}


}
