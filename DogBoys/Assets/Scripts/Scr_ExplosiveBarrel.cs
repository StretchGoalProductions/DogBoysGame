using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Scr_ExplosiveBarrel : MonoBehaviour {

	public Cls_Node currentNode;

	public int damage;

	void Start () {
		currentNode = Scr_Grid.NodeFromWorldPosition(transform.position);
		currentNode.currentState = Cls_Node.nodeState.cover;
	}
	

	void OnMouseOver() {
		if(!EventSystem.current.IsPointerOverGameObject())
		{
			if (Input.GetMouseButtonDown(0) && Scr_GameController.attackMode_) {
				GameObject attacker = Scr_GameController.selectedDog_;
				Debug.Log("ATTACK");
				attacker.GetComponent<Scr_DogBase>().Fire(this);
			}
		}
	}

	public void Explode() {
		List<Cls_Node> neighborNodes = Scr_Grid.GetNeighboringNodes(currentNode);
		
		foreach(Cls_Node node in neighborNodes) {
			if (node.dog != null) {
				node.dog.TakeDamage(damage);
			}
		}

		currentNode.currentState = Cls_Node.nodeState.empty;
		Destroy(transform.parent.gameObject);
	}
}
