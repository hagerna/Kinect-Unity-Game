using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class ViconClient : MonoBehaviour
{
    // the websocket object
    WebSocket ws;

    // the class that will be storing the Vicon data
    public ViconData vd;

    // Start is called before the first frame update
    void Start()
    {
        // create a new websocket and connect to "ws://[IP ADDRESS]:[PORT NUMBER]"
        ws = new WebSocket("ws://192.168.1.4:8080");
        ws.Connect();
        ws.OnMessage += (sender, e) =>
        {
            Debug.Log("Message Recieved from " + ((WebSocket)sender).Url);

            /* take the input json from server and translate it into a class that you can work with (e.Data is the JSON in string form) */
            vd = JsonUtility.FromJson<ViconData>(e.Data);

        };
    }

    // Update is called once per frame
    void Update()
    {
        // Loop that would called every frame while the websocket is connected, currently does nothing except return
        if (ws != null)
        {
            return;
        }
    }

    /* VICON DATA CLASS
     * - bodies: list of Body objects
     *      - Body: Currently Incomplete, if you wish to track full bodies you will need to update the Body class to include the various joints that it may consist of
     * - props: list of Prop objects
     *      - Prop: string name, position ("pos") as Vector object, quaternion ("quat") as Quat object, rotation ("rot") as Vector object
     *          - Vector: float for x, y, and z (same as Unity Vector3 but needed for the JSON to unpack into)
     *          - Quat: float w, x, y, and z (same as Unity Quaternion but needed for the JSON to unpack into)
     *          
     * - Methods:
     *      - GetPropPosition(propID#) --> return the Vector3 position of prop with index of propID#
     *      - GetPropPosition2D(propID#) --> return the Vector2 position of prop with index of propID#
     * 
     */

    [System.Serializable]
    public class ViconData
    {
        public Body[] bodies;
        public Prop[] props;

        // USAGE: Vector3 newPosition = GetPropPosition(0); --> Get the Vector3 position of props[0], i.e. get the position of the first prop
        public Vector3 GetPropPosititon(int propID)
        {
            Vector3 position;
            if (this.props[propID] == null)
            {
                Debug.Log("Error: no prop for GetPropPosition()");
                return Vector3.zero;
            }
            position.x = this.props[propID].pos.x;
            position.y = this.props[propID].pos.y;
            position.z = this.props[propID].pos.z;
            return position;
        }

        // USAGE: Vector2 newPosition = GetPropPosition2D(0); --> Get the Vector2 position of props[0], i.e. get the position of the first prop
        public Vector2 GetPropPosition2D(int propID)
        {
            Vector2 position;
            if (this.bodies == null)
            {
                Debug.Log("Error: no Joint for GetJointRotation()");
                return Vector2.zero;
            }
            position.x = this.props[propID].pos.x;
            position.y = this.props[propID].pos.y;
            return position;
        }

        // USAGE: transform.rotation = GetPropRotation(0); -->  --> Get the Quaternion rotation of props[0], i.e. get the position of the first prop
        public Quaternion GetPropRotation(int propID)
        {
            Vector3 rotation;
            if (this.props[propID] == null)
            {
                Debug.Log("Error: no prop for GetPropRotation()");
                return Quaternion.Euler(Vector3.zero);
            }
            rotation.x = this.props[propID].rot.x;
            rotation.y = this.props[propID].rot.y;
            rotation.z = this.props[propID].rot.z;
            return Quaternion.Euler(rotation);
        }
    }

    // BODY CLASS (Incomplete)
    [System.Serializable]
    public class Body
    {
        public string name;
        public Vector pos;
        public Quat quat;
        public Vector rot;
    }

    // PROP CLASS
    [System.Serializable]
    public class Prop
    {
        public string name;
        public Vector pos;
        public Quat quat;
        public Vector rot;
    }

    // VECTOR CLASS
    [System.Serializable]
    public class Vector
    {
        public float x;
        public float y;
        public float z;
    }

    // QUAT CLASS
    [System.Serializable]
    public class Quat
    {
        public float w;
        public float x;
        public float y;
        public float z;
    }

    // Ensure that the WebSocket closes upon exiting the game (including leaving play-mode)
    private void OnApplicationQuit()
    {
        ws.Close();
    }
}
