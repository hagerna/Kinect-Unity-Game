using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SampleScript : MonoBehaviour
{
    //integer values corresponding to specific joints (Relevant only for Kinect)
    readonly int LEFTHAND = 11;
    readonly int RIGHTHAND = 7;

    KinectClient kinect;
    ViconClient vicon;

    // Start is called before the first frame update
    void Start()
    {
        //Set the "kinect" object to an existing instance of the KinectClient
        kinect = FindObjectOfType<KinectClient>();
        //Set the "vicon" object to an existing instance of the ViconClient
        vicon = FindObjectOfType<ViconClient>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (kinect != null)
        {
            //set the transform of the gameObject to the position of the left hand, multiplied by ten to make it more noticeable
            transform.position = kinect.kd.GetJointPosition(LEFTHAND, 0) * 10;

            //set the rotation of the gameObject to the rotation of the right hand, hard to control, would reccomend smoothing rotation somehow
            transform.rotation = kinect.kd.GetJointRotation(RIGHTHAND, 0);
        }

        if (vicon != null)
        {
            //set the transform of the gameObject to the position of prop 0
            transform.position = vicon.vd.GetPropPosititon(0);

            //set the rotation of the gameObject to the rotation prop 0
            transform.rotation = kinect.kd.GetJointRotation(RIGHTHAND, 0);
        }

    }
}
