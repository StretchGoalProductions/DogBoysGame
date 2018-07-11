using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Grid : MonoBehaviour {

	public Transform startPosition;
	public LayerMask wallMask;
	public LayerMask playerMask;
	public Vector2 gridWorldSize;
	public float nodeRadius;
	public float nodeDistance;

	public Cls_Node[,] grid;
	public List<Cls_Node> finalPath;

	private float nodeDiameter;
	private int gridSizeX, gridSizeY;

	private void Start() {
		nodeDiameter = nodeRadius * 2;
		gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
		gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
		CreateGrid();
	}


	private void CreateGrid()
	{
		grid = new Cls_Node[gridSizeX, gridSizeY];
		Vector3 bottomLeft = transform.position - (Vector3.right * gridWorldSize.x / 2) - (Vector3.forward * gridWorldSize.y / 2);
		for (int y = 0; y < gridSizeY; y++){
			for (int x = 0; x < gridSizeX; x++) {
				Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
				bool isWall = Physics.CheckSphere(worldPoint, nodeRadius, wallMask);
				bool isPlayer = Physics.CheckSphere(worldPoint, nodeRadius, playerMask);

				grid[x,y] = new Cls_Node(isWall, isPlayer, worldPoint, x, y);
			}
		}
	}


	public Cls_Node NodeFromWorldPosition(Vector3 worldPosition) {
		float xPoint = ((worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x);
		float yPoint = ((worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y);

		xPoint = Mathf.Clamp01(xPoint);
		yPoint = Mathf.Clamp01(yPoint);

		int x = Mathf.RoundToInt((gridSizeX - 1) * xPoint);
		int y = Mathf.RoundToInt((gridSizeY - 1) * yPoint);

		return grid[x,y];
	}


	public List<Cls_Node> GetNeighboringNodes(Cls_Node node) {
		List<Cls_Node> neighboringNodes = new List<Cls_Node>();
		int xCheck;
		int yCheck;

		// Right Neighbor
		xCheck = node.gridX + 1;
		yCheck = node.gridY;
		neighboringNodes = CheckAndAddNeighborNode(xCheck, yCheck, neighboringNodes);

		// Left Neighbor
		xCheck = node.gridX - 1;
		yCheck = node.gridY;
		neighboringNodes = CheckAndAddNeighborNode(xCheck, yCheck, neighboringNodes);

		// Top Neighbor
		xCheck = node.gridX;
		yCheck = node.gridY + 1;
		neighboringNodes = CheckAndAddNeighborNode(xCheck, yCheck, neighboringNodes);

		// Bottom Neighbor
		xCheck = node.gridX;
		yCheck = node.gridY - 1;
		neighboringNodes = CheckAndAddNeighborNode(xCheck, yCheck, neighboringNodes);
		
		return neighboringNodes;
	}

	private List<Cls_Node> CheckAndAddNeighborNode(int xCheck, int yCheck, List<Cls_Node> neighboringNodes) {
		if (xCheck >= 0 && xCheck < gridSizeX) {
			if (yCheck >= 0 && yCheck < gridSizeY) {
				neighboringNodes.Add(grid[xCheck, yCheck]);
			}
		}

		return neighboringNodes;
	}


	private void OnDrawGizmos() {
		Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
		
		if (grid != null) {
			foreach (Cls_Node node in grid) {
				if( node.isWall ) {
					Gizmos.color = Color.red;
				}
				else if (node.isPlayer) {
					Gizmos.color = Color.magenta;
				}
				else {
					Gizmos.color = Color.gray;
				}

				if (finalPath != null && finalPath.Contains(node)) {
					Gizmos.color = Color.blue;
				}


				Gizmos.DrawCube(node.position, Vector3.one * (nodeDiameter - nodeDistance));
			}
		}
	}

}
