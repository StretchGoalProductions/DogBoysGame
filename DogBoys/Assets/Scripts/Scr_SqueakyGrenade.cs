using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_SqueakyGrenade : MonoBehaviour {

	public Cls_Node currentNode;

	public int range;

	void Start () {
		currentNode = Scr_Grid.NodeFromWorldPosition(transform.position);
		currentNode.currentState = Cls_Node.nodeState.empty;
	}

	public void Explode() {
		List<Cls_Node> explosionNodes = new List<Cls_Node>();

		for (int i = 0; i < range; i++) {
			if (explosionNodes.Count == 0) {
				List<Cls_Node> thisNodeList = Scr_Grid.GetNeighboringNodes(currentNode);
				explosionNodes.AddRange(thisNodeList);
			}
			else {
				foreach (Cls_Node node in explosionNodes) {
					List<Cls_Node> thisNodeList = Scr_Grid.GetNeighboringNodes(currentNode);

					foreach (Cls_Node newNode in thisNodeList) {
						if (!explosionNodes.Contains(newNode)) {
							explosionNodes.Add(newNode);
						}
					}
				}
			}
		}

		foreach (Cls_Node node in explosionNodes) {
			if(node.currentState == Cls_Node.nodeState.player) {
				node.dog.currentState = Scr_DogBase.dogState.selected;
				node.dog.GetComponent<Scr_Pathfinding>().enabled = true;
				node.dog.GetComponent<Scr_Pathfinding>().targetPosition = currentNode.position;
				break;
			}
		}

	}
	
}
