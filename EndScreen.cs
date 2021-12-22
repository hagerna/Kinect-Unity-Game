using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    public Text Score, StagesBeaten, BallsMissed, TotalScore;

    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        UpdateAll();
    }

    void UpdateAll()
    {
        Score.text = "Score ------------------------ " + gm.totalScore;
        StagesBeaten.text = "Stages Beaten ------ (" + gm.stagesBeaten + " x 50) = " + (gm.stagesBeaten * 50);
        BallsMissed.text = "Balls Missed -------- (" + gm.ballsMissed + " x -5) = " + ((gm.ballsMissed + gm.stagesBeaten) * -5);
        int CalculatedScore = Mathf.FloorToInt(gm.totalScore + (gm.stagesBeaten * 50) + ((gm.ballsMissed + gm.stagesBeaten) * -5));
        TotalScore.text = "Total Score:         " + CalculatedScore;
    }
}
