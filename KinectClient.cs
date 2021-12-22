using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;


public class KinectClient : MonoBehaviour
{
    // the websocket object
    WebSocket ws;

    // the class that will be storing the Kinect data
    public KinectData kd;

    private static KinectClient kc;

    // Ensure that only one instance of the KinectClient exists so that other scripts all acess the same one
    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (kc == null) { kc = this; }
        else { Destroy(gameObject); }
    }

    // Start is called before the first frame update
    void Start()
    {
        //create a new websocket and connect to "ws://[IP ADDRESS]:[PORT NUMBER]"
        ws = new WebSocket("ws://192.168.1.6:8080");
        ws.Connect();
        ws.OnMessage += (sender, e) =>
        {

            /* input json from server*/
            if (e.Data != null)
            {
                /* take the input json from server and translate it into a class that you can work with (e.Data is the JSON in string form) */
                kd = JsonUtility.FromJson<KinectData>(e.Data);

            }
            else { Debug.Log("Not receiving any data"); }
            
        };
    }

    // Update is called once per frame
    void Update()
    {
        // Loop that would be called every frame while the websocket is connected, currently does nothing except return
        if (ws != null)
        {
            return;
        }
    }

    /* KINECT DATA CLASS
     * consists of:
     * - bodies: list of Body objects
     *      - Body: integer id, list of Joint objects, bool for leftHandState and rightHandState (whether the hand is closed or not)
     *          -Joint: cameraX, cameraY and cameraZ, integer jointType, orientationW, orientationX, orientationY, and orientationZ
     *              - Methods:
     *                  - position() --> combines the cameraX, cameraY, and cameraZ into a Vector3 to be used for GetJointPosition()
     *                  - position2D() --> combines the cameraX and cameraY into a Vector2 to be used for GetJointPosition2D()
     *                  - rotation() --> combines the orientationX, orientationY, and orientationZ into a Vector3 to be used for GetJointRotation()
     * 
     * - Methods:
     *      - GetJointPosition(Joint#, Body#) --> returns Vector3 position of Joint# for Body#
     *      - GetJointPosition2D(Joint#, Body#) --> returns Vector2 position of Joint# for Body#
     *      - GetJointRotation(Joint#, Body#) --> returns Quaternion rotation of Joint# for Body#
     *      - GetJointDeltaPosition(Joint#, Previous_Position, Body#) --> returns distance between current position of Joint# for Body# and the Previous_Position
     */
    [System.Serializable]
    public class KinectData
    {
        public Body[] bodies;

        // USAGE: Vector3 newPosition = GetJointPosition(11); --> Get the position of the player's left hand assuming there is only one player.
        public Vector3 GetJointPosition(int Joint, int Body = 0)
        {
            Vector3 position;
            if (this.bodies.Length == 0)
            {
                Debug.Log("Error: no Joint for GetJointPosition()");
                return Vector3.zero;
            }
            position = this.bodies[Body].joints[Joint].Position();
            return position;
        }

        // USAGE: Vector2 newPosition = GetJointPosition2D(7); --> Get the (X,Y) position of the player's right hand assuming there is only one player.
        public Vector2 GetJointPosition2D(int Joint, int Body = 0)
        {
            Vector2 position;
            if (this.bodies.Length == 0)
            {
                Debug.Log("Error: no Joint for GetJointPosition2D()");
                return Vector2.zero;
            }
            position = this.bodies[Body].joints[Joint].Position2D();
            return position;
        }

        // USAGE: transform.rotation = GetJointRotation(7, 1); --> Get the rotation of the second players right hand.
        public Quaternion GetJointRotation(int Joint, int Body = 0)
        {
            Vector3 rotation = Vector3.zero;
            if (this.bodies.Length == 0)
            {
                Debug.Log("Error: no Joint for GetJointRotation()");
                return Quaternion.Euler(Vector3.zero);
            }
            rotation = this.bodies[Body].joints[Joint].Rotation() * 360;
            return Quaternion.Euler(rotation);
        }

        //USAGE:
        //  previous = GetJointPosition(7);
        //  float delta = GetJointDeltaPosition(7, previous); --> Get the distance between the current position of the right hand and the previous position.
        public float GetJointDeltaPosition(int Joint, Vector3 previous, int Body = 0)
        {
            Vector3 position;
            if (this.bodies.Length == 0)
            {
                Debug.Log("Error: no Joint for GetJointDeltaPosition()");
                return -1;
            }
            position = this.bodies[Body].joints[Joint].Position();
            return Vector3.Distance(previous, position);
        }
    }

    //BODY CLASS
    [System.Serializable]
    public class Body
    {
        public int id;
        public Joint[] joints;
        public int leftHandState;
        public int rightHandState;
    }


    //JOINT CLASS
    [System.Serializable]
    public class Joint
    {
        public float cameraX;
        public float cameraY;
        public float cameraZ;
        public int jointType;
        public float orientationW;
        public float orientationX;
        public float orientationY;
        public float orientationZ;

        public Vector3 Position()
        {
            Vector3 position;
            position.x = cameraX;
            position.y = cameraY;
            position.z = cameraZ;
            return position;
        }

        public Vector2 Position2D()
        {
            Vector2 position;
            position.x = cameraX;
            position.y = cameraY;
            return position;
        }

        public Vector3 Rotation()
        {
            Vector3 rotation;
            rotation.x = orientationX;
            rotation.y = orientationY;
            rotation.z = orientationZ;
            return rotation;
        }
    }

    // Ensure that the WebSocket closes upon exiting the game (including leaving play-mode)
    private void OnApplicationQuit()
    {
        ws.Close();
    }
}
