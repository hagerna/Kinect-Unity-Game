using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickScript : MonoBehaviour
{
    public Material[] colors = new Material[5];
    public int blockHealth;
    int scoreMultiplier = 1;
    // Start is called before the first frame update
    void Start()
    {
        blockHealth = FindObjectOfType<GameManager>().blockHealth;
    }

    public void Hit()
    {
        FindObjectOfType<ScoreScript>().UpdateScore(scoreMultiplier);
        scoreMultiplier++;
        blockHealth -= 1;
        if (blockHealth < 0)
        {
            Destroy(this.gameObject);
        }
    }
}
