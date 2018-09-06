using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Scr_ExplosiveBarrel : MonoBehaviour {

	public Cls_Node currentNode;

	public int damage;
	public int range;
    public GameObject spawnEffect;

	List<Cls_Node> explosionNodes = new List<Cls_Node>();

	void Start () {
		currentNode = Scr_Grid.NodeFromWorldPosition(transform.position);
		currentNode.currentState = Cls_Node.nodeState.cover;
	}
	

	void OnMouseOver() {
		if(!EventSystem.current.IsPointerOverGameObject())
		{
			if (Input.GetMouseButtonDown(0) && Scr_GameController.attackMode_) {
				GameObject attacker = Scr_GameController.selectedDog_;
				attacker.GetComponent<Scr_DogBase>().Fire(this);
			}
		}
	}

	public void Explode() {
		//explosionNodes = Scr_Grid.GetNeighboringNodes(currentNode);
		NodesInRange(range);
		
		StartCoroutine(WaitThenExplode());
	}

	public IEnumerator WaitThenExplode() {
		yield return new WaitForSeconds(0.4f);

		foreach(Cls_Node node in explosionNodes) {
			if (node.dog != null) {
				node.dog.TakeDamage(damage);
			}
			else if (node.explosiveBarrel != null && node.explosiveBarrel != this) {
				node.explosiveBarrel.Explode();
			}
		}

		currentNode.currentState = Cls_Node.nodeState.empty;
		currentNode.explosiveBarrel = null;

        Instantiate(spawnEffect, transform.position - new Vector3(0f, -0.5f, 0f), Quaternion.identity);
		Destroy(transform.parent.gameObject);
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
