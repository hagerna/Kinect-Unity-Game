using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputControl : MonoBehaviour
{
    //intialize variables
    public bool left, right, FacingKinect;
    float XRange, YRange;

    readonly float xdisplaySize = 2.45f;
    readonly float ydisplaySize = 2.25f;

    Vector3 worldPosition, leftHand, rightHand;

    public string interactMode = "mouse";

    KinectClient kinect;
    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {

        kinect = FindObjectOfType<KinectClient>();
        gm = FindObjectOfType<GameManager>();

        //retrieve the xRange and yRange calculated by the GameManager
        XRange = gm.XRange;
        YRange = gm.YRange;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (interactMode == "kinect")   // if in kinect mode, determine position of in-game hands based on position of actual hands
        {
            if (left)
            {
                PositionLeftHand(kinect.kd.GetJointPosition(gm.LEFTHAND));
            }
            if (right)
            {
                PositionRightHand(kinect.kd.GetJointPosition(gm.RIGHTHAND));
            }
        }
        else if (interactMode == "mouse")       // if in mouse mode, determine position of in-game hands based on position of mouse and button pressed
        {
            if (left && Input.GetMouseButton(0))
            {
                leftHand = Input.mousePosition;
                leftHand.z = Camera.main.nearClipPlane;
                worldPosition = Camera.main.ScreenToWorldPoint(leftHand);
                transform.position = worldPosition;
            }
            if (right && Input.GetMouseButton(1))
            {
                rightHand = Input.mousePosition;
                rightHand.z = Camera.main.nearClipPlane;
                worldPosition = Camera.main.ScreenToWorldPoint(rightHand);
                transform.position = worldPosition;
            }
        }
        else if (interactMode == "perfect")     // if in perfect mode, follow the position of the ball, used for testing later levels
        {
            Vector3 ball = FindObjectOfType<BallScript>().GetComponentInParent<Transform>().position;
            ball.x += 0.1f;
            ball.y += 0.1f;
            ball.z = -10;
            transform.position = ball;
        }
        if (FindObjectOfType<BallScript>() != null && FindObjectOfType<BallScript>().FirstHit)
        {
            FindObjectOfType<BallScript>().FirstContact();
        }
    }

    // Determine the correct position for the Right Hand
    void PositionRightHand(Vector3 position)
    {
        int correction = 1;
        if (!FacingKinect) { correction = -1; }
        rightHand.x = (correction) * position.x / XRange * xdisplaySize; //(hand position / calibrated X height) * display height
        rightHand.y = position.y / YRange * ydisplaySize + 3f; //(hand position / calibrated Y height) * display height
        rightHand.z = -10;  //position the hand in front of the camera

        // Make sure that the hand will not leave the boundary of the screen
        if (rightHand.x > 2.5f) { rightHand.x = 2.5f; }
        if (rightHand.y > 5.25f) { rightHand.y = 5.25f; }
        else if (rightHand.y < 0.75f) { rightHand.y = 0.75f; }
        transform.position = rightHand;
    }

    // Determine the correct position for the Left Hand
    void PositionLeftHand(Vector3 position)
    {
        int correction = 1;
        if (!FacingKinect) { correction = -1; }
        leftHand.x = (correction) * position.x / XRange * xdisplaySize; //(hand position / calibrated X height) * display height
        leftHand.y = position.y / YRange * ydisplaySize + 3f; //(hand position / calibrated Y height) * display height
        leftHand.z = -10;   //position the hand in front of the camera

        // Make sure that the hand will not leave the boundary of the screen
        if (leftHand.x < -2.5f) { leftHand.x = -2.5f; }
        if (leftHand.y > 5.25f) { leftHand.y = 5.25f; }
        else if (leftHand.y < 0.75f) { leftHand.y = 0.75f; }
        transform.position = leftHand;
    }
}
