using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputControlUI : MonoBehaviour
{
    public bool left, right, FacingKinect;
    float XRange, YRange;

    readonly float xdisplaySize = 350f;
    readonly float ydisplaySize = 260f;

    Vector3 leftHand, rightHand;

    KinectClient kinect;
    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {

        kinect = FindObjectOfType<KinectClient>();
        gm = FindObjectOfType<GameManager>();

        XRange = gm.XRange;
        YRange = gm.YRange;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (left)
        {
            PositionLeftHand(kinect.kd.GetJointPosition2D(gm.LEFTHAND));
        }
        if (right)
        {
            PositionRightHand(kinect.kd.GetJointPosition2D(gm.RIGHTHAND));
        }
    }

    void PositionRightHand(Vector3 position)
    {
        int correction = 1;
        if (!FacingKinect) { correction = -1; }
        rightHand.x = (correction) * position.x / XRange * xdisplaySize; //(hand position / calibrated X height) * display height
        rightHand.y = position.y / YRange * ydisplaySize + 3f; //(hand position / calibrated Y height) * display height
        rightHand.z = 0;  //position the hand in front of the camera
        if (rightHand.x > 350f) { rightHand.x = 350f; }
        if (rightHand.y > 260f) { rightHand.y = 260f; }
        else if (rightHand.y < -260f) { rightHand.y = -260f; }
        transform.localPosition = rightHand;
    }

    void PositionLeftHand(Vector3 position)
    {
        int correction = 1;
        if (!FacingKinect) { correction = -1; }
        leftHand.x = (correction) * position.x / XRange * xdisplaySize; //(hand position / calibrated X height) * display height
        leftHand.y = position.y / YRange * ydisplaySize + 3f; //(hand position / calibrated Y height) * display height
        leftHand.z = 0;   //position the hand in front of the camera
        if (leftHand.x < -350f) { leftHand.x = -350f; }
        if (leftHand.y > 260f) { leftHand.y = 260f; }
        else if (leftHand.y < -260f) { leftHand.y = -260f; }
        transform.localPosition = leftHand;
    }
}
