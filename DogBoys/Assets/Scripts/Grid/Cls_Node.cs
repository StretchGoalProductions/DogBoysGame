using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cls_Node : IHeapItem<Cls_Node> {

	public int gridX;
	public int gridY;
	public Vector3 position;
	public Cls_Node parent;
	public int gCost;
	public int hCost;
	public int fCost { get { return gCost + hCost; } }

	public enum nodeState {empty, player, wall, cover, pickup};
	public nodeState currentState;

	public Scr_DogBase dog;
	public Scr_ExplosiveBarrel explosiveBarrel;
	public Scr_SqueakyGrenadePickup grenadePickup;

	private int heapIndex;

	public Cls_Node (nodeState currentState, Vector3 position, int gridX, int gridY) {
		this.currentState = currentState;
		this.position = position;
		this.gridX = gridX;
		this.gridY = gridY;
	}

	public int HeapIndex {
        get {
            return heapIndex;
        }
        set {
            heapIndex = value;
        }
    }

	public int CompareTo(Cls_Node nodeToCompare) {
        // CompareTo returns 1 if greater, 0 if equal, -1 if less
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0) {
            // If f costs are the same, h cost for the tie breaker
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        // Our pathfinding prioritizes the SMALLEST f cost instead of biggest value (default)
        // Reverse the result of the comparison
        return -compare;
    }

}
