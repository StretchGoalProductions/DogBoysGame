using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Pathfinding : MonoBehaviour {

	private Scr_Grid grid;
	public Transform startPosition;
	public Transform targetPosition;

	public int maxRange;
	public int currentRange;

	private void Awake() {
		grid = GetComponent<Scr_Grid>();
	}

	private void Update() {
		FindPath(startPosition.position, targetPosition.position);
	}


	private void FindPath(Vector3 a_startPosition, Vector3 b_targetPosition) {
		Cls_Node startNode = grid.NodeFromWorldPosition(a_startPosition);
		Cls_Node targetNode = grid.NodeFromWorldPosition(b_targetPosition);

		List<Cls_Node> OpenList = new List<Cls_Node>();
		List<Cls_Node> ClosedList = new List<Cls_Node>(); // Tutorial uses HashSet since you don't need to access the closed list anymore for A* algorithm, I'm going to use a List to avoid confusion
	
		OpenList.Add(startNode);

		while (OpenList.Count > 0) {
			Cls_Node currentNode = OpenList[0];

			for (int i = 0; i < OpenList.Count; i++) {
				if ((OpenList[i].fCost < currentNode.fCost || OpenList[i].fCost == currentNode.fCost) && OpenList[i].hCost < currentNode.hCost) {
					currentNode = OpenList[i];
				}
			}

			OpenList.Remove(currentNode);
			ClosedList.Add(currentNode);

			if(currentNode == targetNode) {
				GetFinalPath(startNode, targetNode);
			}

			foreach (Cls_Node neighborNode in grid.GetNeighboringNodes(currentNode)) {
				if((neighborNode.currentState == Cls_Node.nodeState.wall) || ClosedList.Contains(neighborNode)) {
					continue; // Skip if node is wall or in closed list
				}

				int moveCost = currentNode.gCost + GetDistance(currentNode, neighborNode);

				if (moveCost < neighborNode.gCost || !OpenList.Contains(neighborNode)) {
					neighborNode.gCost = moveCost;
					neighborNode.hCost = GetDistance(neighborNode, targetNode);
					neighborNode.parent = currentNode;

					if(!OpenList.Contains(neighborNode)) {
						OpenList.Add(neighborNode);
					}
				}
			}
		}
	}

	private int GetDistance(Cls_Node nodeA, Cls_Node nodeB) {
		int iX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int iY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

		const int DIAGONAL_DISTANCE = (int) 1.4 * 10;

		if( iX > iY ) {
			return DIAGONAL_DISTANCE * iY + (10 * (iX - iY));
		}
		else {
			return DIAGONAL_DISTANCE * iX + (10 * (iY - iX));
		}
	}

	private int GetManhattanDistance(Cls_Node nodeA, Cls_Node nodeB) { // Distance between 2 nodes, aparantly named after calculating distances of square blocks in Manhattan, NY
		int iX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int iY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

		return iX + iY;
	}


	private void GetFinalPath(Cls_Node a_startNode, Cls_Node b_targetNode) {
		List<Cls_Node> finalPath = new List<Cls_Node>();
		Cls_Node currentNode = b_targetNode;

		while (currentNode != a_startNode) {
			finalPath.Add(currentNode);
			currentNode = currentNode.parent;
		}

		finalPath.Reverse();

		grid.finalPath = finalPath;
		if(finalPath.Count > 0) {
			startPosition.GetComponent<NavMeshMovement>().setDestination(finalPath[0].position);
		}
	}
}
