using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshMovement : MonoBehaviour
{ // code to be combined with Matt's movement script and probably both combined into Character script.

    /*
     Note:
         The following is for when starting a new scene...
         Be sure to set a plane just at level with the top of the ground to where it's not visible but is is reachable as a NavMesh.
         Open the Navigation window and go to object tab
         Select the plane and hit Navigation Static in the Navigation Window and make sure the dropdown is set to Walkable
         Select the buildings and obstacles in the scene next, hit static like before but make sure these are set to Not Walkable
         Now go to Bake tab and bake it but make sure Agent Radius in bake is set to 0.1
         
     Note:
         This script doesn't take turns or anything into account, 
         as it will be merged with Matt's script and added to the Character controller,
         this will hopefully not be a problem as I imagine the code will be edited when they're combined.
         */
    public Vector3 targetPos, currentPos;
    public Scr_Grid testGrid;

    public bool AtDestination;
    public UnityEngine.AI.NavMeshAgent myAgent;
    public Animator myAnim;

    public bool NavIsOnGround;
    float onMeshThreshold = 3;

    [Tooltip("Set this active to disable relying on tiles. Only used for testing the movement system, turn off in actual project.")]
    public bool TestingScript;

    private void Start()
    {
        myAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        myAnim = GetComponent<Animator>();
        AtDestination = true;
    }

    private void Update()
    {
            if (!AtDestination && myAgent.transform.position.x == myAgent.destination.x && myAgent.transform.position.z == myAgent.destination.z)
            {
                AtDestination = true;
                myAnim.SetBool("a_isRunning", false);
                testGrid.CreateGrid();
            }
    }

    public void setDestination(Vector3 destination) {
        AtDestination = false;
        myAgent.destination = destination;
        myAnim.SetBool("a_isRunning", true);
    }
}
