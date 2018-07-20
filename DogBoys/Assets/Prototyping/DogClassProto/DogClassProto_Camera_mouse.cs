using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DogClassProto_Camera_mouse : MonoBehaviour {
	[SerializeField]
	private GameObject tileHighlight;
	[SerializeField]
	private GameObject nullHighlight;

	// Keep track of where the player clicks in the game world
	private RaycastHit hit;
	private bool highlighted;
	Transform currentTile;
	private GameObject currentHighlight;
	private GameObject nully;
	private DogClassProto_GameController gc;	//game controller

    public static DogClassProto_Camera_mouse instance { get; set; } //this makes it easier for other scripts to reference public variables here.
    public Transform GoalTile;

	void Start() {
        instance = this; //this sets the instance to make it easier for other scripts to reference public variables here.
		highlighted = false;
		gc = DogClassProto_GameController.Instance;
	}

	// Update is called once per frame
	void  FixedUpdate () {
		// Shoot a raycast from the camera to where player is pointing
		Ray tempRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		// If that raycast hit something...
		if (Physics.Raycast (tempRay, out hit, 10000)) {
			// Draw a line in the game that follows the ray
			Debug.DrawLine (tempRay.origin, hit.point, Color.cyan);
			//if you hit a tile
			//if (hit.transform.gameObject.tag == "Tile" || hit.transform.gameObject.tag == "FullCover" || hit.transform.gameObject.tag == "NoHitCover") {
			if(hit.transform.gameObject.tag != "DeadTile"){
				currentTile = hit.transform.gameObject.transform;
				//current tile not highlighted
				if (highlighted == false) {
					currentTile = hit.transform.gameObject.transform;
					currentHighlight = Instantiate (tileHighlight, currentTile);
					nully = Instantiate (nullHighlight, currentTile);
					nully.SetActive (false);
					highlighted = true;
				//current tile is highlighted
				} else if (currentHighlight.transform.position != currentTile.position) {
					if (gc.HasSelectedCharacter ()) {
						cls_DogBase chara = gc.currentlySelectedCharacter.GetComponent<cls_DogBase> ();
						Debug.Log(chara.getWeapon().getMoveRange());
						if(!chara.isInMoveRange(gc.currentlySelectedCharacter.transform.position, currentTile.position)){
							currentHighlight.SetActive (false);
							nully.SetActive(true);
						} else {
							currentHighlight.SetActive (true);
							nully.SetActive(false);
						}
					}
					//currentHighlight.transform.position = currentTile.position;
				currentHighlight.transform.position = new Vector3(currentTile.position.x, 0, currentTile.position.z);
				nully.transform.position = currentHighlight.transform.position;
				}
				//-------------------------------------------------------------
				//input bs
				if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {					//as of now, all this does is move, but other functionality should be easy to implement
                    if (hit.transform.gameObject.tag == "CoverTile") {
                        gc.MoveSelectedCharacter(currentTile.position, true, hit.transform.gameObject.GetComponent<CoverTile>().GetDirection());
                    }
                    else {
                        gc.MoveSelectedCharacter(currentTile.position, false);
                    }
				}
			}
		} else {
			if (currentHighlight != null) {
				currentHighlight.transform.position = new Vector3(currentHighlight.transform.position.x,-10,currentHighlight.transform.position.z);
			}
		}
        if (currentTile != null)//this part is for setting the tile for the NavAgent to move to. It's made into a separate variable so as to not interfere with the code above.
        { GoalTile = currentTile; }
	}
		
}
