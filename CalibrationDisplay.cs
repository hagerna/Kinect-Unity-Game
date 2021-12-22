using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CalibrationDisplay : MonoBehaviour
{
    public Text calibrationText;
    public Texture[] figures;
    public RawImage image;
    public Image background;

    KinectClient kinect;

    int LEFTHAND = 7;
    int RIGHTHAND = 11;

    GameManager gm;


    // Start is called before the first frame update
    void Start()
    {
        kinect = FindObjectOfType<KinectClient>();
        StartCoroutine(PlayerCalibration());        //on transition to recalibration screen, begin recalibration routine
        gm = FindObjectOfType<GameManager>();
    }

    // initialize screen
    public void StartCalibration()
    {
        gameObject.SetActive(true);
        background.enabled = true;
        calibrationText.enabled = true;
        image.enabled = true;
    }

    // helper script for updating text
    public void UpdateText(string str)
    {
        calibrationText.text = str;
    }

    // helper script for updating image
    public void UpdateFigure(int index)
    {
        image.texture = figures[index];
    }

    // Recalibration routine
    public IEnumerator PlayerCalibration()
    {
        // Initialize vectors to be outside the range of the kinect so any value will adjust them
        Vector3 TopLeft = new Vector3(2, -2, -10);
        Vector3 TopRight = new Vector3(-2, -2, -10);
        Vector3 BotLeft = new Vector3(2, 2, -10);
        Vector3 BotRight = new Vector3(-2, 2, -10);

        StartCalibration();

        // Present initial screen for 5 seconds
        UpdateFigure(0);
        UpdateText("To properly calibrate the kinect to your range of motion, please follow the instructions: ");
        yield return new WaitForSeconds(5f);

        // Calibration phase: 1/4
        UpdateFigure(1);
        UpdateText("Wave your LEFT HAND as HIGH and AWAY from your body as you can (Calibration: 1/4)");

        // Find top left range
        for (int i = 0; i < 50; i++)
        {
            if (kinect.kd.GetJointPosition(LEFTHAND).x < TopLeft.x)
            {
                TopLeft.x = kinect.kd.GetJointPosition(LEFTHAND).x;
            }
            if (kinect.kd.GetJointPosition(LEFTHAND).y > TopLeft.y)
            {
                TopLeft.y = kinect.kd.GetJointPosition(LEFTHAND).y;
            }
            yield return new WaitForSeconds(0.1f);
        }

        Debug.Log("Top Left: " + TopLeft);

        // Calibration phase: 2/4
        UpdateFigure(2);
        UpdateText("Wave your RIGHT HAND hand as HIGH and AWAY from your body as you can (Calibration: 2/4)");

        // Find top right range
        for (int i = 0; i < 50; i++)
        {
            if (kinect.kd.GetJointPosition(RIGHTHAND).x > TopRight.x)
            {
                TopRight.x = kinect.kd.GetJointPosition(RIGHTHAND).x;
            }
            if (kinect.kd.GetJointPosition(RIGHTHAND).y > TopRight.y)
            {
                TopRight.y = kinect.kd.GetJointPosition(RIGHTHAND).y;
            }
            yield return new WaitForSeconds(0.1f);
        }

        Debug.Log("Top Right: " + TopRight);

        // Calibration phase: 3/4
        UpdateFigure(3);
        UpdateText("Wave your LEFT HAND as LOW and AWAY from your body as you can (Calibration: 3/4)");

        // Find bottom left range
        for (int i = 0; i < 50; i++)
        {
            if (kinect.kd.GetJointPosition(LEFTHAND).x < BotLeft.x)
            {
                BotLeft.x = kinect.kd.GetJointPosition(LEFTHAND).x;
            }
            if (kinect.kd.GetJointPosition(LEFTHAND).y < BotLeft.y)
            {
                BotLeft.y = kinect.kd.GetJointPosition(LEFTHAND).y;
            }
            yield return new WaitForSeconds(0.1f);
        }

        Debug.Log(BotLeft);

        // Calibration phase: 4/4
        UpdateFigure(4);
        UpdateText("Wave your Right HAND as LOW and AWAY from your body as you can (Calibration: 4/4)");


        // Find bottom right range
        for (int i = 0; i < 50; i++)
        {
            if (kinect.kd.GetJointPosition(RIGHTHAND).x > BotRight.x)
            {
                BotRight.x = kinect.kd.GetJointPosition(RIGHTHAND).x;
            }
            if (kinect.kd.GetJointPosition(RIGHTHAND).y < BotRight.y)
            {
                BotRight.y = kinect.kd.GetJointPosition(RIGHTHAND).y;
            }
            yield return new WaitForSeconds(0.1f);
        }

        Debug.Log(BotRight);

        UpdateFigure(0);
        UpdateText("Calibration Complete");
        gm.CalibrateRange(TopLeft, TopRight, BotLeft, BotRight);    //have GameManager calculate the approriate XRange and YRange based on four vectors


        yield return new WaitForSeconds(2f);

        gm.GeneratedFirst = false;
        if (gm.FirstFixation)
        {
            gm.FirstFixation = false;
            //  Go to main menu
            SceneManager.LoadScene(3);
        }
        //  Go back to the previous scene
        else { SceneManager.LoadScene(gm.LastScene); }
    }
}
