using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class Scr_DogMovement : MonoBehaviour {

	public Vector3 targetPos, currentPos;
	public List<Cls_Node> finalPath;

    public UnityEngine.AI.NavMeshAgent myAgent;
    public Animator myAnim;
	public Scr_DogBase dog;
	public Scr_Pathfinding pathfinder;
	public Camera mainCamera;
	public LayerMask hitLayers;

	public int maxMoveRange;

	void Start () {
		myAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        myAnim = GetComponent<Animator>();
		dog = GetComponent<Scr_DogBase>();
		pathfinder = GetComponent<Scr_Pathfinding>();
		mainCamera = GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponentInChildren<Camera>();
		hitLayers = LayerMask.GetMask("Environment");
		
		pathfinder.maxMoveRange = maxMoveRange;
	}
	
	void Update () {
		if (dog.currentState == Scr_DogBase.dogState.moving && myAgent.transform.position.x >= myAgent.destination.x - 0.05 && myAgent.transform.position.x <= myAgent.destination.x + 0.05 && myAgent.transform.position.z >= myAgent.destination.z - 0.05 && myAgent.transform.position.z <= myAgent.destination.z + 0.05) {
			myAgent.transform.position = new Vector3(myAgent.destination.x, myAgent.transform.position.y, myAgent.nextPosition.z);
			if(finalPath.Count == 1) {
				UpdateNodesAfterMove();
				dog.UnselectCharacter();
				pathfinder.targetPosition = transform.position;
				myAnim.SetBool("a_isRunning", false);
				//myAgent.autoBraking = true;
			}
			else if (finalPath.Count > 1){
				if(finalPath[0] == Scr_Grid.NodeFromWorldPosition(myAgent.transform.position)) {
					UpdateNodesAfterMove();
					myAgent.destination = finalPath[0].position;
				}
			}
		}
		else if(Input.GetMouseButtonDown(0) && dog.currentState == Scr_DogBase.dogState.selected && dog.movesLeft > 0 && dog.selectCooldown <= 0 && !EventSystem.current.IsPointerOverGameObject()) {
			Vector3 mouse = Input.mousePosition;
			Ray castPoint = mainCamera.ScreenPointToRay(mouse);

			RaycastHit hit;
			
			if(Physics.Raycast(castPoint, out hit, Mathf.Infinity, hitLayers)) {
				Cls_Node thisNode = Scr_Grid.NodeFromWorldPosition(hit.point);

				if(thisNode.currentState == Cls_Node.nodeState.empty) {
					pathfinder.targetPosition = hit.point;
					dog.UseMove();
				}
			}
		}
	}



	public void SetDestination() {
		if(dog.currentState == Scr_DogBase.dogState.selected) { // Change to selected once Select Character is refactored
			finalPath = Scr_Grid.finalPath;

			dog.currentState = Scr_DogBase.dogState.moving;
			myAgent.destination = finalPath[0].position;
			//myAgent.autoBraking = false;
			myAnim.SetBool("a_isRunning", true);
		}
    }

	public void UpdateNodesAfterMove() {
		dog.currentNode.currentState = Cls_Node.nodeState.empty;
		dog.currentNode.dog = null;
		dog.currentNode = finalPath[0];
		dog.currentNode.currentState = Cls_Node.nodeState.player;
		dog.currentNode.dog = dog;
		finalPath.Remove(finalPath[0]);
	}
}
