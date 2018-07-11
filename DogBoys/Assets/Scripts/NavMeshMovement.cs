using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshMovement : MonoBehaviour
{ // code to be combined with Matt's movement script
    public Transform targetPos, currentPos, nextPos;

    public bool AtDestination;
    public UnityEngine.AI.NavMeshAgent myAgent;
    public Animator myAnim;

    public bool NavIsOnGround;
    float onMeshThreshold = 3;

    private void Start()
    {
        myAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        //IsAgentOnNavMesh(this.gameObject);
        NavIsOnGround = myAgent.isOnNavMesh; //Checks if NavMesh Agent is on ground. If this bool is false, the agent won't move and there will be tons of "Set Destination" errors
        AtDestination = true;
    }

    private void Update()
    {
        NavIsOnGround = myAgent.isOnNavMesh;
        if (currentPos != targetPos)
        {
            AtDestination = false;
        }
        //targetPos = Camera_mouse.instance.GoalTile;
        if (!AtDestination)
        {
            myAgent.destination = targetPos.position;
            currentPos = targetPos;
        }
        if (myAgent.transform.position.x == targetPos.position.x && myAgent.transform.position.z == targetPos.position.z)
        {
            AtDestination = true;
            currentPos = targetPos;
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
