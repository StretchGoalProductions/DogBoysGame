using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class edit_FogPrefabs : MonoBehaviour {

    #region Variables
    [SerializeField]
    private int scaleOverTime_; //The rate at which the prefab will increase or decease in size over Time.deltaTime
    [SerializeField]
    private float minScaleSize_; //When the prefab reachs this scale size, we will disable/enable it show it can be seen/hidden
    [SerializeField]
    private float maxScaleSize_; //This is the max size of the scale of a prefab 

    private bool doScalingOfPrefab_ = false; //Does and object need to be scaled
    private bool scaleDirection_; //If set to false then scale down the prefab down in size, else scale the prefab size up
    private MeshRenderer meshRendererForFogPiece_;
    private bool isVisable_;
    #endregion

    #region Gets and Sets
    public bool IsVisable
    {
        get
        {
            return isVisable_;
        }

        set
        {
            isVisable_ = value;
        }
    }
    #endregion
    private void Start()
    {
        isVisable_ = true;
        meshRendererForFogPiece_ = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if (doScalingOfPrefab_) //A unit is moving and we need to update the fog of war system
        { 
            if (scaleDirection_) //Scale the prefab up in size
            { 
                transform.localScale = new Vector3(transform.localScale.x + (Time.deltaTime * scaleOverTime_), transform.localScale.y + (Time.deltaTime * scaleOverTime_), transform.localScale.z + (Time.deltaTime * scaleOverTime_));
                if (transform.localScale.x >= maxScaleSize_) //When the prefab increase to a size larger than its orginal size end scaling and set size to mav value
                {
                    transform.localScale = new Vector3(maxScaleSize_, maxScaleSize_, maxScaleSize_);
                    doScalingOfPrefab_ = false; //The object is at its max size so there is no need to continue scaling the size up
                    handleDisplay(); //Show the prefab
                }
            }
            else //Scale the prefab down in size
            { 
                transform.localScale = new Vector3(transform.localScale.x - (Time.deltaTime * scaleOverTime_), transform.localScale.y - (Time.deltaTime * scaleOverTime_), transform.localScale.z - (Time.deltaTime * scaleOverTime_));
                if (transform.localScale.x < minScaleSize_)
                {
                    handleDisplay(); //Hide the prefab as it scales down in size
                    doScalingOfPrefab_ = false; //The object is hidden from game view so there is no need to continue scaling the size down
                }
            }
        }
    }

    //If a unit is moving in the game, then we need to check to see how we should edit the fog of war system 
    public void doScaling(bool scaleUpOrDown) {
        doScalingOfPrefab_ = true;
        scaleDirection_ = scaleUpOrDown;
    }

    private void handleDisplay()
    {
        if (meshRendererForFogPiece_.isVisible)
        {
            meshRendererForFogPiece_.enabled = false;
            isVisable_ = false;
        }
        else
        {
            meshRendererForFogPiece_.enabled = true;
            isVisable_ = true;
        }
        
    }
}
