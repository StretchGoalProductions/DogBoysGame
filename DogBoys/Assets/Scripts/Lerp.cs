using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lerp : MonoBehaviour {

    #region Variables and Declarations
    [SerializeField]
    private float lerpSpeed;

    private Vector3 startPosition;
    private Vector3 endPosition;
    private float timeLerpBegan;
    private bool isLerping;
    #endregion

    #region Lerp Functions
    public void StartLerping(Vector3 inStartPos, Vector3 inEndPos) {
        isLerping = true;
        timeLerpBegan = Time.time;

        startPosition = inStartPos;
        endPosition = inEndPos;
    }
    #endregion

    #region Unity Overrides
    //We do the actual interpolation in FixedUpdate(), since we're dealing with a rigidbody
    void FixedUpdate() {
        if (isLerping) {
            //We want percentage = 0.0 when Time.time = _timeStartedLerping
            //and percentage = 1.0 when Time.time = _timeStartedLerping + timeTakenDuringLerp
            //In other words, we want to know what percentage of "timeTakenDuringLerp" the value
            //"Time.time - _timeStartedLerping" is.
            float timeSinceStarted = Time.time - timeLerpBegan;
            float percentageComplete = timeSinceStarted / lerpSpeed;// timeTakenDuringLerp;

            //Perform the actual lerping.  Notice that the first two parameters will always be the same
            //throughout a single lerp-processs (ie. they won't change until we hit the space-bar again
            //to start another lerp)
            transform.position = Vector3.Lerp(startPosition, endPosition, percentageComplete);

            //When we've completed the lerp, we set _isLerping to false
            if (percentageComplete >= 1.0f) {
                isLerping = false;
            }
        }
    }
    #endregion
}
