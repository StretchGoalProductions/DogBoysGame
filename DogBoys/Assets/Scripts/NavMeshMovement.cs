﻿using System.Collections;
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
    public Transform targetPos, currentPos;

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
        //IsAgentOnNavMesh(this.gameObject);
        NavIsOnGround = myAgent.isOnNavMesh; //Checks if NavMesh Agent is on ground. If this bool is false, the agent won't move and there will be tons of "Set Destination" errors
        AtDestination = true;
        myAnim = GetComponent<Animator>();
    }

    private void Update()
    {
        NavIsOnGround = myAgent.isOnNavMesh; //Checks if NavMesh Agent is on ground. If this bool is false, the agent won't move and there will be tons of "Set Destination" errors
        if (!TestingScript)
        {
            if (Camera_mouse.instance.GoalTile != null) //If the goal tile isn't null, sets the target position to move to that position
            { targetPos = Camera_mouse.instance.GoalTile; }
        }
        if (NavIsOnGround)
        {
            if (currentPos != targetPos)
            {
                AtDestination = false;
            }
            //targetPos = Camera_mouse.instance.GoalTile;
            if (!AtDestination)
            {
                myAgent.isStopped = false;
                myAgent.destination = targetPos.position;
                myAnim.SetBool("a_isRunning", true);
                //currentPos = targetPos;
            }
            if (myAgent.transform.position.x == targetPos.position.x && myAgent.transform.position.z == targetPos.position.z)
            {
                AtDestination = true;
                myAgent.isStopped = true;
                myAnim.SetBool("a_isRunning", false);
                currentPos = targetPos;
            }
        }
    }

    /*public bool IsAgentOnNavMesh(GameObject agentObject)
    {
        Vector3 agentPosition = agentObject.transform.position;
        NavMeshHit hit;

        // Check for nearest point on navmesh to agent, within onMeshThreshold
        if (NavMesh.SamplePosition(agentPosition, out hit, onMeshThreshold, NavMesh.AllAreas))
        {
            // Check if the positions are vertically aligned
            if (Mathf.Approximately(agentPosition.x, hit.position.x)
                && Mathf.Approximately(agentPosition.z, hit.position.z))
            {
                // Lastly, check if object is below navmesh
                return agentPosition.y >= hit.position.y;
            }
        }

        return false;
    }*/
}