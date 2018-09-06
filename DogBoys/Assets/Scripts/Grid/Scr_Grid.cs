using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Grid : MonoBehaviour {

	public LayerMask wallMask;
	public LayerMask playerMask;
	public LayerMask coverMask;
	public LayerMask pickUpMask;
	public Vector2 inspectGridWorldSize;
	public static Vector2 gridWorldSize;
	public float nodeRadius;
	public float nodeDistance;

	public bool drawGizmos;
	public bool onlyDrawPath;

	public static Cls_Node[,] grid;
	public static List<Cls_Node> finalPath;

	private float nodeDiameter;
	private static int gridSizeX, gridSizeY;

	private void Awake() {
		wallMask = LayerMask.GetMask("Wall");
		playerMask = LayerMask.GetMask("Player");
		coverMask = LayerMask.GetMask("Cover");
		pickUpMask = LayerMask.GetMask("PickUp");
		gridWorldSize = inspectGridWorldSize;
		nodeDiameter = nodeRadius * 2;
		gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
		gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
		CreateGrid();
	}


	public void CreateGrid()
	{
		grid = new Cls_Node[gridSizeX, gridSizeY];
		Vector3 bottomLeft = transform.position - (Vector3.right * gridWorldSize.x / 2) - (Vector3.forward * gridWorldSize.y / 2);
		for (int y = 0; y < gridSizeY; y++){
			for (int x = 0; x < gridSizeX; x++) {
				Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
				bool isWall = Physics.CheckSphere(worldPoint, nodeRadius, wallMask);
				bool isPlayer = Physics.CheckSphere(worldPoint, nodeRadius, playerMask);
				bool isCover = Physics.CheckSphere(worldPoint, nodeRadius, coverMask);
				bool isPickUp = Physics.CheckSphere(worldPoint, nodeRadius, pickUpMask);

				Cls_Node.nodeState currentState = Cls_Node.nodeState.empty;
				if (isWall) {
					currentState = Cls_Node.nodeState.wall;
					grid[x,y] = new Cls_Node(currentState, worldPoint, x, y);
				}
				else if (isCover) {
					currentState = Cls_Node.nodeState.cover;
					grid[x,y] = new Cls_Node(currentState, worldPoint, x, y);

					Scr_ExplosiveBarrel checkBarrel = Physics.OverlapSphere(worldPoint, nodeRadius, coverMask)[0].gameObject.GetComponent<Scr_ExplosiveBarrel>();
					if (checkBarrel != null) {
						grid[x,y].explosiveBarrel = checkBarrel;
					}
				}
				else if (isPickUp) {
					currentState = Cls_Node.nodeState.pickup;
					grid[x,y] = new Cls_Node(currentState, worldPoint, x, y);

					
				}
				else if (isPlayer) {
					currentState = Cls_Node.nodeState.player;
					grid[x,y] = new Cls_Node(currentState, worldPoint, x, y);

					Scr_DogBase thisDog = Physics.OverlapSphere(worldPoint, nodeRadius, playerMask)[0].gameObject.GetComponent<Scr_DogBase>();
					thisDog.currentNode = grid[x,y];
					grid[x,y].dog = thisDog;
				}
				else {
					grid[x,y] = new Cls_Node(currentState, worldPoint, x, y);
				}
			}
		}
	}


	public static Cls_Node NodeFromWorldPosition(Vector3 worldPosition) {
		float xPoint = ((worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x);
		float yPoint = ((worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y);

		xPoint = Mathf.Clamp01(xPoint);
		yPoint = Mathf.Clamp01(yPoint);

		int x = Mathf.RoundToInt((gridSizeX - 1) * xPoint);
		int y = Mathf.RoundToInt((gridSizeY - 1) * yPoint);

		return grid[x,y];
	}


	public static List<Cls_Node> GetNeighboringNodes(Cls_Node node) {
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

		// Diagonals
		// Top Left
		xCheck = node.gridX - 1;
		yCheck = node.gridY + 1;
		neighboringNodes = CheckAndAddNeighborNode(xCheck, yCheck, neighboringNodes);

		// Top Right
		xCheck = node.gridX + 1;
		yCheck = node.gridY + 1;
		neighboringNodes = CheckAndAddNeighborNode(xCheck, yCheck, neighboringNodes);

		// Bottom Left
		xCheck = node.gridX - 1;
		yCheck = node.gridY - 1;
		neighboringNodes = CheckAndAddNeighborNode(xCheck, yCheck, neighboringNodes);

		// Bottom Right
		xCheck = node.gridX + 1;
		yCheck = node.gridY - 1;
		neighboringNodes = CheckAndAddNeighborNode(xCheck, yCheck, neighboringNodes);
		
		return neighboringNodes;
	}

	private static List<Cls_Node> CheckAndAddNeighborNode(int xCheck, int yCheck, List<Cls_Node> neighboringNodes) {
		if (xCheck >= 0 && xCheck < gridSizeX) {
			if (yCheck >= 0 && yCheck < gridSizeY) {
				neighboringNodes.Add(grid[xCheck, yCheck]);
			}
		}

		return neighboringNodes;
	}


	private void OnDrawGizmos() {
		if (drawGizmos) { 
			Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
			
			if (finalPath != null && onlyDrawPath) {
				foreach (Cls_Node node in finalPath) {
							Gizmos.color = Color.blue;
							Gizmos.DrawCube(node.position, Vector3.one * (nodeDiameter - nodeDistance));
				}
			}
			else if (grid != null && !onlyDrawPath) {
				foreach (Cls_Node node in grid) {
					if( node.currentState == Cls_Node.nodeState.wall) {
						Gizmos.color = Color.red;
					}
					else if (node.currentState == Cls_Node.nodeState.cover) {
						Gizmos.color = Color.yellow;
					}
					else if (node.currentState == Cls_Node.nodeState.player) {
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

}
