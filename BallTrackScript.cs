using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallTrackScript : MonoBehaviour
{
    // the white circles to represent Number of balls the player has left
    public RawImage[] Balls;

    // Make the next ball disappear
    public void Fade(float ballNumber)
    {
        RawImage ball = Balls[(int)ballNumber];
        ball.CrossFadeAlpha(0f, 1f, true);
    }

    // Reset: make all the balls visible again
    public void Reset()
    {
        for (int i = 0; i < Balls.Length; i++)
        {
            Balls[i].CrossFadeAlpha(1f, 1f, true);
        }
    }

}
