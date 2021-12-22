using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FixationUI : MonoBehaviour
{
    // initialize variables
    public Image fixationCircle;
    GameObject LeftHand, RightHand;
    public int result;
    GameManager gm;

    bool checkingFixation;
    float distance;
    Vector2 position1;
    Vector2 position2;
    Vector2 target;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        // set the target x and y used to determine fixation to the center of the cirlce
        target.x = transform.localPosition.x;
        target.y = transform.localPosition.y;
        // get the left and right hands so that their position can be tracked to determine fixations
        LeftHand = GameObject.Find("LeftHand");
        RightHand = GameObject.Find("RightHand");
        checkingFixation = false;
    }

    // Update is called once per frame
    void Update()
    {
        // get the position of the left and right hands
        position1 = LeftHand.transform.localPosition;
        position2 = RightHand.transform.localPosition;
        distance = Vector2.Distance(position1, target);
        if (distance <= 100 && !checkingFixation)       // if the left hand is within 100 of the target and not currently checking for fixation, begin process to check for fixation
        {
            checkingFixation = true;
            StartCoroutine(CheckFixation2D(target, LeftHand));
        }
        distance = Vector2.Distance(position2, target);
        if (distance <= 100 && !checkingFixation)       // if the right hand is within 100 of the target and not currently checking for fixation, begin process to check for fixation
        {
            checkingFixation = true;
            StartCoroutine(CheckFixation2D(target, RightHand));
        }
    }

    // check for fixation by determining that the position of the hand remains within the target circle for a duration of 1 full second
    public IEnumerator CheckFixation2D(Vector2 target, GameObject hand)
    {
        Vector2 pos = hand.transform.localPosition;
        float dist = Vector2.Distance(pos, target);
        if (dist <= 100f)
        {
            for (int i = 0; i < 100; i++)
            {
                pos = hand.transform.localPosition;
                distance = Vector2.Distance(pos, target);
                if (distance > 100f)        // hand leaves the circle = fixation broken
                {
                    checkingFixation = false;
                    fixationCircle.rectTransform.sizeDelta = Vector2.zero;
                    yield break;
                }
                else
                {
                    yield return new WaitForSeconds(0.01f);
                    fixationCircle.rectTransform.sizeDelta = new Vector2(i * 3f, i * 2f);       // expand green indicator circle to provide visual feedback of fixation
                }
            }
            gm.fixationResult(result);      // once fixation is complete, have game manager determine result 
        }
        checkingFixation = false;
    }
}
