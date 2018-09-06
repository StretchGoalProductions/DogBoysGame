using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_SqueakyGrenade : MonoBehaviour {

	public Cls_Node currentNode;

	public int damage;

	public int dogEffectRange;
	public int explosionRange;
	List<Cls_Node> dogEffectedNodes = new List<Cls_Node>();
	List<Cls_Node> explosionNodes = new List<Cls_Node>();
	List<Cls_Node> walkHereNodes = new List<Cls_Node>();

	public GameObject spawnEffect;

	void Start () {
		currentNode = Scr_Grid.NodeFromWorldPosition(transform.position);
		currentNode.currentState = Cls_Node.nodeState.empty;

		dogEffectedNodes = NodesInRange(dogEffectRange);
		explosionNodes = NodesInRange(explosionRange);
		walkHereNodes = NodesInRange(1);

		EffectDogs();

		StartCoroutine(WaitSeconds(3.5f));
	}

	public void EffectDogs() {

		dogEffectedNodes = NodesInRange(dogEffectRange);
		explosionNodes = NodesInRange(explosionRange);
		walkHereNodes = NodesInRange(1);

		foreach (Cls_Node node in dogEffectedNodes) {
			if(node.currentState == Cls_Node.nodeState.player && walkHereNodes.Count > 0) {
				node.dog.currentState = Scr_DogBase.dogState.selected;
				node.dog.GetComponent<Scr_Pathfinding>().enabled = true;
				foreach (Cls_Node targetNode in walkHereNodes) {
					if (targetNode.currentState == Cls_Node.nodeState.empty) {
						node.dog.GetComponent<Scr_Pathfinding>().targetPosition = targetNode.position;
						walkHereNodes.Remove(targetNode);
						break;
					}
					else {
						//walkHereNodes.Remove(targetNode);
					}
				}
			}
			else if(walkHereNodes.Count <= 0) {
				break;
			}
		}
	}

	public void Explode() {

		dogEffectedNodes = NodesInRange(dogEffectRange);
		explosionNodes = NodesInRange(explosionRange);
		walkHereNodes = NodesInRange(1);

		foreach (Cls_Node node in explosionNodes) {
			if (node.currentState == Cls_Node.nodeState.player) {
				node.dog.TakeDamage(damage);
			}
			else if (node.currentState == Cls_Node.nodeState.cover && node.explosiveBarrel != null) {
				node.explosiveBarrel.Explode();
			}
		}

		Instantiate(spawnEffect, transform.position - new Vector3(0f, -0.5f, 0f), Quaternion.identity);
		Destroy(gameObject);
	}

	public List<Cls_Node> NodesInRange(int squareRange) {

		List<Cls_Node> nodeList = new List<Cls_Node>();

		for (int i = 0; i < squareRange; i++) {
			if (i == 0) {
				List<Cls_Node> thisNodeList = Scr_Grid.GetNeighboringNodes(currentNode);
				foreach (Cls_Node node in thisNodeList) {
					nodeList.Add(node);
				}
			}
			else {
				List<Cls_Node> tempNodes = new List<Cls_Node>(nodeList);
				foreach (Cls_Node node in tempNodes) {
					List<Cls_Node> thisNodeList = Scr_Grid.GetNeighboringNodes(node);

					foreach (Cls_Node newNode in thisNodeList) {
						if (!nodeList.Contains(newNode)) {
							nodeList.Add(newNode);
						}
					}
				}
			}
		}

		return nodeList;
	}

	private IEnumerator WaitSeconds(float seconds) {
		yield return new WaitForSeconds(seconds);
		Explode();
	}
	
}
