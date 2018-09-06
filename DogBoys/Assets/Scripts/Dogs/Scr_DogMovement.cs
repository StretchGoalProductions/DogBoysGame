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
	public LayerMask hitLayers;

	public int maxMoveRange;

    public GameObject moveRangeIndicator;
    public GameObject cantMoveIndicator;
    private bool displayed = false;
    private List<GameObject> rangeIndicators = new List<GameObject>();

	void Start () {
		myAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        myAnim = GetComponent<Animator>();
		dog = GetComponent<Scr_DogBase>();
		pathfinder = GetComponent<Scr_Pathfinding>();
		hitLayers = LayerMask.GetMask("Environment");
		
		pathfinder.maxMoveRange = maxMoveRange;
		//pathfinder.enabled = false;
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
			Ray castPoint = Camera.main.ScreenPointToRay(mouse);

			RaycastHit hit;
			
			if(Physics.Raycast(castPoint, out hit, Mathf.Infinity, hitLayers)) {
				Cls_Node targetNode = Scr_Grid.NodeFromWorldPosition(hit.point);

				if(targetNode.currentState == Cls_Node.nodeState.empty || targetNode.currentState == Cls_Node.nodeState.pickup) {
					Cls_Node currentNode = Scr_Grid.NodeFromWorldPosition(transform.position);
					int distance = pathfinder.GetDistance(currentNode, targetNode);

					if(distance <= maxMoveRange*10) {
						pathfinder.targetPosition = hit.point;
						dog.UseMove();
					}
				}
			}
		}

        // if (dog.currentState == Scr_DogBase.dogState.selected) {
        //     displayRange(maxMoveRange);
        // } else {
        //     removeRange();
        // }
	}

    public void displayRange(int range) {
        if (!displayed) {
            Cls_Node currentNode = Scr_Grid.NodeFromWorldPosition(transform.position);
            for (int i = 0; i < Scr_Grid.gridWorldSize.x; i++) {
                for (int j = 0; j < Scr_Grid.gridWorldSize.y; j++) {
                    int distance = pathfinder.GetDistance(currentNode, Scr_Grid.grid[i, j]);
                    if (distance <= range * 10 && Scr_Grid.grid[i, j].currentState != Cls_Node.nodeState.wall && Scr_Grid.grid[i, j].currentState != Cls_Node.nodeState.cover) {
                        //Debug.Log("(" + Scr_Grid.grid[i, j].gridX + ", " + Scr_Grid.grid[i, j].gridY + ")");
                        GameObject indicator = Instantiate(moveRangeIndicator, Scr_Grid.grid[i, j].position, Quaternion.identity);
                        rangeIndicators.Add(indicator);
                    } else if (distance <= range * 10) {
                        GameObject indicator = Instantiate(cantMoveIndicator, Scr_Grid.grid[i, j].position, Quaternion.identity);
                        rangeIndicators.Add(indicator);
                    }
                }
            }
            displayed = true;
        }
    }

    public void removeRange() {
        for (int i = 0; i < rangeIndicators.Count; i++) {
            Destroy(rangeIndicators[i]);
        }
        rangeIndicators.Clear();
        displayed = false;
    }

	public void SetDestination() {
		if(dog.currentState == Scr_DogBase.dogState.selected) { // Change to selected once Select Character is refactored
			finalPath = Scr_Grid.finalPath;

			dog.currentState = Scr_DogBase.dogState.moving;
			myAgent.destination = finalPath[0].position;
			//myAgent.autoBraking = false;
			myAnim.SetBool("a_isRunning", true);

			removeRange();
		}
    }

	public void UpdateNodesAfterMove() {
		dog.currentNode.currentState = Cls_Node.nodeState.empty;
		dog.currentNode.dog = null;
		dog.currentNode = finalPath[0];
		dog.currentNode.dog = dog;

		if(dog.currentNode.currentState == Cls_Node.nodeState.pickup) {
			if(dog.currentNode.grenadePickup != null) {
				dog.currentNode.grenadePickup.pickUp();
			}
		}

		dog.currentNode.currentState = Cls_Node.nodeState.player;
		finalPath.Remove(finalPath[0]);
	}
}
