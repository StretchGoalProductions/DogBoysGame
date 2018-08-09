using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_SqueakyGrenade : MonoBehaviour {

	public Cls_Node currentNode;

	public int range;
	List<Cls_Node> explosionNodes = new List<Cls_Node>();

	void Start () {
		currentNode = Scr_Grid.NodeFromWorldPosition(transform.position);
		currentNode.currentState = Cls_Node.nodeState.empty;

		Explode();
	}

	public void Explode() {
		
		NodesInRange(range);

		foreach (Cls_Node node in explosionNodes) {
			if(node.currentState == Cls_Node.nodeState.player) {
				node.dog.currentState = Scr_DogBase.dogState.selected;
				node.dog.GetComponent<Scr_Pathfinding>().enabled = true;
				node.dog.GetComponent<Scr_Pathfinding>().targetPosition = currentNode.position;
				break;
			}
		}

	}

	public void NodesInRange(int squareRange) {

		for (int i = 0; i < squareRange; i++) {
			if (i == 0) {
				List<Cls_Node> thisNodeList = Scr_Grid.GetNeighboringNodes(currentNode);
				foreach (Cls_Node node in thisNodeList) {
					explosionNodes.Add(node);
				}
			}
			else {
				List<Cls_Node> tempNodes = new List<Cls_Node>(explosionNodes);
				foreach (Cls_Node node in tempNodes) {
					List<Cls_Node> thisNodeList = Scr_Grid.GetNeighboringNodes(node);

					foreach (Cls_Node newNode in thisNodeList) {
						if (!explosionNodes.Contains(newNode)) {
							explosionNodes.Add(newNode);
						}
					}
				}
			}
		}

	}
	
}
