﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Pathfinding : MonoBehaviour {

	public Vector3 targetPosition;

	public int maxMoveRange;
	public int currentRange;

	//public bool move;

	private Scr_DogMovement dogMovement;

	private void Start() {
		dogMovement = GetComponent<Scr_DogMovement>();
		targetPosition = transform.position;
		//move = false;
	}

	private void Update() {
		if (targetPosition != transform.position) {
			FindPath(transform.position, targetPosition);
		}
	}


	private void FindPath(Vector3 startPosition, Vector3 targetPosition) {
		Cls_Node startNode = Scr_Grid.NodeFromWorldPosition(startPosition);
		Cls_Node targetNode = Scr_Grid.NodeFromWorldPosition(targetPosition);

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

			foreach (Cls_Node neighborNode in Scr_Grid.GetNeighboringNodes(currentNode)) {
				if((neighborNode.currentState != Cls_Node.nodeState.empty) || ClosedList.Contains(neighborNode)) {
					continue; // Skip if node is wall or in closed list
				}

				int moveCost = currentNode.gCost + GetDistance(currentNode, neighborNode);

				if ( (moveCost < neighborNode.gCost || !OpenList.Contains(neighborNode))) {
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

	public int GetDistance(Cls_Node nodeA, Cls_Node nodeB) {
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


	private void GetFinalPath(Cls_Node startNode, Cls_Node targetNode) {
		List<Cls_Node> finalPath = new List<Cls_Node>();
		Cls_Node currentNode = targetNode;

		while (currentNode != startNode) {
			finalPath.Add(currentNode);
			currentNode = currentNode.parent;
		}

		finalPath.Reverse();

		Scr_Grid.finalPath = finalPath;
		if(finalPath.Count > 0 && dogMovement.dog.currentState == Scr_DogBase.dogState.selected) {
			dogMovement.SetDestination();
		}
	}
}