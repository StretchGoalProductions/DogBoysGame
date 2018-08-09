using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World_Camera : MonoBehaviour
{
    [SerializeField]
    private float cameraSpeed_;
    [SerializeField]
    private float cameraZoomSpeed_;
    [SerializeField]
    private float cameraSmooth_;
    [SerializeField]
    private float cameraMovementRangeXMin_, cameraMovementRangeXMax_;
    [SerializeField]
    private float cameraMovementRangeZMin_, cameraMovementRangeZMax_;
    [SerializeField]
    private float cameraRotationRangeXMin_, cameraRotationRangeXMax_;
    [SerializeField]
    private float cameraZoomRangeYMin_, cameraZoomRangeYMax_;
    [SerializeField]
    private Transform targetCameraFollow_;
    [SerializeField]
    private Vector3 cameraOffset_;
    [SerializeField]
    private bool cameraFollowing_ = false;
    [SerializeField]
    private float cameraRoatationSmoother_;
    [SerializeField]
    private float cameraRotationSpeed_;

    private float horizontalInput_;
    private float verticalInput_;
    private float mouseScrollWheelInput_;
    private float playerMouseMoveX_;
    private float playerMouseMoveY_;
    private float playetKeyRotation_;
    private Vector2 totalCameraMovement;



    private void Update()
    {
        getUserInput();
    }

    private void LateUpdate()
    {
        if (cameraFollowing_)
        {
            cameraFollow();
        }
        else
        {
            cameraFreeMovement();
            rotateCamera();
        }
    }

    private void cameraFollow()
    {
        Vector3 newPosition = targetCameraFollow_.position + cameraOffset_;
        Vector3 smoothed = Vector3.Lerp(transform.position, newPosition, cameraSmooth_ * Time.deltaTime);
        targetCameraFollow_.position = newPosition;
        transform.LookAt(targetCameraFollow_);
    }

    private void cameraFreeMovement()
    {
        if ((horizontalInput_ != 0.0f) && (verticalInput_ != 0.0f))
        {
            Vector3 cameraForward = transform.forward;
            Vector3 cameraRight = transform.right;
            cameraForward.y = 0.0f;
            cameraRight.y = 0.0f;
            cameraForward.Normalize();
            cameraRight.Normalize();
            Vector3 moveDirection = cameraForward * verticalInput_ + cameraRight * horizontalInput_;
            moveDirection.Normalize();
            moveDirection = transform.position + moveDirection * cameraSpeed_ * Time.fixedDeltaTime;
            moveDirection.z = Mathf.Clamp(moveDirection.z, cameraMovementRangeZMin_, cameraMovementRangeZMax_);
            moveDirection.x = Mathf.Clamp(moveDirection.x, cameraMovementRangeXMin_, cameraMovementRangeXMax_);
            transform.position = moveDirection;

        }
        else if (horizontalInput_ != 0.0f)
        {
            Vector3 cameraForward = transform.forward;
            Vector3 cameraRight = transform.right;
            cameraForward.y = 0.0f;
            cameraRight.y = 0.0f;
            cameraForward.Normalize();
            cameraRight.Normalize();
            Vector3 moveDirection = cameraForward * 0.0f + cameraRight * horizontalInput_;
            moveDirection.Normalize();
            moveDirection = transform.position + moveDirection * cameraSpeed_ * Time.fixedDeltaTime;
            moveDirection.z = Mathf.Clamp(moveDirection.z, cameraMovementRangeZMin_, cameraMovementRangeZMax_);
            moveDirection.x = Mathf.Clamp(moveDirection.x, cameraMovementRangeXMin_, cameraMovementRangeXMax_);
            transform.position = moveDirection;
        }
        else if (verticalInput_ != 0.0f)
        {
            Vector3 cameraForward = transform.forward;
            Vector3 cameraRight = transform.right;
            cameraForward.y = 0.0f;
            cameraRight.y = 0.0f;
            cameraForward.Normalize();
            cameraRight.Normalize();
            Vector3 moveDirection = cameraForward * verticalInput_ + cameraRight * 0.0f;
            moveDirection.Normalize();
            moveDirection = transform.position + moveDirection * cameraSpeed_ * Time.fixedDeltaTime;
            moveDirection.z = Mathf.Clamp(moveDirection.z, cameraMovementRangeZMin_, cameraMovementRangeZMax_);
            moveDirection.x = Mathf.Clamp(moveDirection.x, cameraMovementRangeXMin_, cameraMovementRangeXMax_);
            transform.position = moveDirection;
        }
        cameraZoom();
    }

    private void cameraZoom()
    {
        if (mouseScrollWheelInput_ != 0.0f)
        {
            float y = (transform.position.y + (cameraZoomSpeed_ * Time.deltaTime * mouseScrollWheelInput_));
            y = Mathf.Clamp(y, cameraZoomRangeYMin_, cameraZoomRangeYMax_);
            Vector3 movement = new Vector3(transform.position.x, y, transform.position.z);
            transform.position = movement;
        }
    }

    private void getUserInput()
    {
        mouseScrollWheelInput_ = Input.GetAxisRaw("Mouse ScrollWheel");
        verticalInput_ = Input.GetAxisRaw("Vertical");
        horizontalInput_ = Input.GetAxisRaw("Horizontal");

        //Key Rotation
        playetKeyRotation_ = Input.GetAxisRaw("Camera Rotation");

        //Free Rotation Camera
        /*
        playerMouseMoveX_ = Input.GetAxisRaw("Mouse X");
        playerMouseMoveY_ = Input.GetAxisRaw("Mouse Y");
        */
    }

    private void rotateCamera()
    {
        //Key Rotation
        Vector2 userInput = new Vector2(playetKeyRotation_, 0.0f);
        userInput = Vector2.Scale(userInput, new Vector2(cameraRotationSpeed_ * cameraRoatationSmoother_, cameraRotationSpeed_ * cameraRoatationSmoother_));
        Vector2 smoothingVector = new Vector2();
        smoothingVector.y = Mathf.Lerp(smoothingVector.y, userInput.x, 1.0f / cameraRoatationSmoother_);
        totalCameraMovement += smoothingVector;
        Vector3 newRotation = new Vector3(35.0f, totalCameraMovement.y, transform.eulerAngles.z);
        transform.eulerAngles = newRotation;

        //Free Rotation Camera
        /*
        Vector2 userInput = new Vector2(playerMouseMoveX_, playerMouseMoveY_);
        userInput = Vector2.Scale(userInput, new Vector2(cameraRotationSpeed_ * cameraRoatationSmoother_, cameraRotationSpeed_ * cameraRoatationSmoother_));
        Vector2 smoothingVector = new Vector2();
        smoothingVector.x = Mathf.Lerp(smoothingVector.x, userInput.x, 1.0f / cameraRoatationSmoother_);
        smoothingVector.y = Mathf.Lerp(smoothingVector.y, userInput.y, 1.0f / cameraRoatationSmoother_);
        totalCameraMovement += smoothingVector;
        Vector3 temp = new Vector3(-1 * totalCameraMovement.y, totalCameraMovement.x, transform.eulerAngles.z);
        temp.x = Mathf.Clamp(temp.x, cameraRotationRangeXMin_, cameraRotationRangeXMax_);
        transform.eulerAngles = temp;
        */
    }
}