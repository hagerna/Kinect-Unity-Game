using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiffiucltyTracking : MonoBehaviour
{
    GameManager gm;
    Vector3 Easy, Medium, Hard;

    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        Easy = new Vector3(-200, -160, 0);
        Medium = new Vector3(0, -160, 0);
        Hard = new Vector3(200, -160, 0);
        StartCoroutine(CheckDifficulty());
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator CheckDifficulty()
    {
        for (; ; )
        {
            if (gm.Difficulty == -2)
            {
                transform.localPosition = Easy;
            }
            else if (gm.Difficulty == 0)
            {
                transform.localPosition = Medium;
            }
            else if (gm.Difficulty == 2)
            {
                transform.localPosition = Hard;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}
