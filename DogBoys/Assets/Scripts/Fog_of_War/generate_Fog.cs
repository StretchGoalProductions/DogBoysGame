using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generate_Fog : MonoBehaviour {

    #region Variables
    [SerializeField]
    private GameObject fogPrefab_;
    [SerializeField]
    private int xRange_;
    [SerializeField]
    private int yRange_;
    [SerializeField]
    private int prefabSize_;
    #endregion

    //Generate the simple square are made up of the selected prefab
    void Start () {
        for (int x = 0; x < xRange_; ++x) {
            for (int z = 0; z < xRange_; ++z) {
                //Generate a new prefab for each coordinate location between the given xRange_ and yRange_
                GameObject fogPiece = Instantiate(fogPrefab_, new Vector3(transform.position.x + (x * prefabSize_), transform.position.y, transform.position.z + (z * prefabSize_)), Quaternion.identity);

                fogPiece.name = "Fog_Piece_" + x + "_" + z; //Name each of the new gameobjects
                fogPiece.transform.SetParent(transform); //Set each of the new gameobjects as children of the current parent object which is Fog_Map
            }
        }
	}
}
