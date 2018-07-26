using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Update_Fog_of_War : MonoBehaviour {

    #region Variables
    [SerializeField]
    private GameObject fogMap_;
    [SerializeField]
    private float unitViewDistance_;

    private edit_FogPrefabs updateFogPiece;
    private bool movingUnit_ = true;
    #endregion

    #region Gets and Sets
    public float UnitViewDistance
    {
        get
        {
            return unitViewDistance_;
        }

        set
        {
            unitViewDistance_ = value;
        }
    }
    #endregion

    private void Update()
    {
        scaleFogPiece();
    }

    private void scaleFogPiece()
    {
        foreach(Transform fogPiece in fogMap_.transform) //Get each of the fog pieces in the map
        {
            float totalDistance = Vector3.Distance(fogPiece.position, transform.position); //Get the distance from each of the fog piece to the current object that this script is attached to
            updateFogPiece = fogPiece.GetComponent<edit_FogPrefabs>();
            if ((totalDistance > unitViewDistance_) && movingUnit_)
            {
                if (!updateFogPiece.IsVisable)
                {
                    updateFogPiece.doScaling(true);
                }
            }
            else if (totalDistance <= unitViewDistance_) //Start to scale the fog pieces up or down
            {
                if (updateFogPiece.IsVisable)
                {
                    updateFogPiece.doScaling(false);
                }
            }
            
        }
    }

     
}
