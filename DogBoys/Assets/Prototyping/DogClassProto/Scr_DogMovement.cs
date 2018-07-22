using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Scr_DogMovement : MonoBehaviour {

	public Vector3 targetPos, currentPos;
	public List<Cls_Node> finalPath;

    public UnityEngine.AI.NavMeshAgent myAgent;
    public Animator myAnim;
	public Scr_DogBase dog;

	void Start () {
		myAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        myAnim = GetComponent<Animator>();
		dog = GetComponent<Scr_DogBase>();
	}
	
	void Update () {
		if (dog.currentState == Scr_DogBase.dogState.moving && myAgent.transform.position.x >= myAgent.destination.x - 0.05 && myAgent.transform.position.x <= myAgent.destination.x + 0.05 && myAgent.transform.position.z >= myAgent.destination.z - 0.05 && myAgent.transform.position.z <= myAgent.destination.z + 0.05)
            {
				myAgent.transform.position = new Vector3(myAgent.destination.x, myAgent.transform.position.y, myAgent.nextPosition.z);
				if(finalPath.Count == 1) {
					UpdateNodesAfterMove();
					dog.currentState = Scr_DogBase.dogState.unselected;
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
