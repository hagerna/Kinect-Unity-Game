using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Initialized Variables
    [HideInInspector]
    public float score, totalScore, ballsMissed, XRange, YRange;
    [HideInInspector]
    public float ballsLeft = 3;
    [HideInInspector]
    public int blockHealth, LastScene, stagesBeaten, LEFTHAND, RIGHTHAND, Difficulty;

    GameObject levelComplete;
    public GameObject[] brickPatterns;
    Vector3 pos1, pos2, pos3, pos4;
    bool gameEnded;

    [HideInInspector]
    public bool GeneratedFirst, FirstFixation;
    public bool FacingKinect = false;

    private static GameManager gm;
    AudioManager am;

    private void Awake()
    {
        // Ensure that only a single copy of GameManager exists and persists
        DontDestroyOnLoad(this);
        if(gm == null) { gm = this; }
        else { Destroy(gameObject);  }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Set positions for brick patterns
        pos1 = Vector3.zero;
        pos2 = -Vector3.forward * 2f;
        pos3 = -Vector3.forward * 4f;
        pos4 = -Vector3.forward * 6f;
        Difficulty = 0; //  Go to medium difficulty

        Reset();    // Reset relevant initialized variables

        FirstFixation = true;

        if (FacingKinect)   // Establish LEFTHAND and RIGHTHAND to be used for Input Control
        {
            LEFTHAND = 7;
            RIGHTHAND = 11;
        }
        else
        {
            LEFTHAND = 11;
            RIGHTHAND = 7;
        }

        am = FindObjectOfType<AudioManager>();
        am.Play("Menu Music");
        SceneManager.LoadScene(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Game" && !GeneratedFirst)    // On first load of Game scene, create the brick patterns
        {
            GeneratedFirst = true;
            GenerateLevel();
        }
        if (ballsLeft <= 0 && !gameEnded)       // if player runs out of balls, end game
        {
            gameEnded = true;
            Invoke(nameof(GameOver), 2f);
            return;
        }
        else if (FindObjectOfType<BrickScript>() == null && score > 0 && ballsLeft > 0)     // if no bricks left, continue to next level or end on level 5
        {
            Debug.Log("Level Complete");
            NextLevel();
        }

    }

    // Hide "Level Complete" text and create 3 brick patterns for the player to destroy
    public void GenerateLevel()
    {
        levelComplete = GameObject.Find("Level Complete");
        levelComplete.SetActive(false);
        Instantiate(brickPatterns[Random.Range(0, brickPatterns.Length)], pos1, Quaternion.Euler(0, 0, 0));
        Instantiate(brickPatterns[Random.Range(0, brickPatterns.Length)], pos2, Quaternion.Euler(0, 0, 0));
        Instantiate(brickPatterns[Random.Range(0, brickPatterns.Length)], pos3, Quaternion.Euler(0, 0, 0));
        if (stagesBeaten > 4)
        {
            Instantiate(brickPatterns[Random.Range(0, brickPatterns.Length)], pos4, Quaternion.Euler(0, 0, 0));     // potential for infinite levels, not used
        }
    }

    // End game by adjusting variables and transition to EndScreen Scene
    void GameOver()
    {
        ballsMissed = 3 - ballsLeft;
        totalScore += score;
        ballsLeft = -1;
        am.Stop("Game Music");
        am.Play("Menu Music");
        SceneManager.LoadScene(2);
    }

    // Adjust variables before creating new level with increased difficulty i.e. bricks take an additional hit to destroy
    void NextLevel()
    {
        if (stagesBeaten != 4)
        {
            levelComplete.SetActive(true);
            totalScore += score;
            score = 0;
            ballsMissed += 3 - ballsLeft;
            ballsLeft = 3;
            stagesBeaten++;
            blockHealth++;
            FindObjectOfType<BallScript>().LevelReset();
            Invoke(nameof(GenerateLevel), 2f);
            return;
        }
        else
        {
            GameOver();
        }
    }

    //to get x,y in game: (hand position / calibrated X height) * display height aka normalized * display --> calibrate the range for Input Control
    public void CalibrateRange(Vector3 TopLeft, Vector3 TopRight, Vector3 BotLeft, Vector3 BotRight)
    {
        XRange = (Vector3.Distance(TopLeft, TopRight) + Vector3.Distance(BotLeft, BotRight)) / 4f;  // get average X range
        YRange = (Vector3.Distance(TopLeft, BotLeft) + Vector3.Distance(TopRight, BotRight)) / 4f;  // get average Y range
        Debug.Log("(XRange, Yrange): (" + XRange + "," + YRange + ")");
    }

    // Reset variables for initial game state
    private void Reset()
    {
        stagesBeaten = 0;
        ballsLeft = 3;
        score = 0;
        totalScore = 0;
        ballsMissed = 0;
        blockHealth = 0;
        GeneratedFirst = false;
        gameEnded = false;
    }

    // Single function to handle all fixations, perform certain action based on result recieved, each unique fixation has a unique result #
    public void fixationResult(int result)
    {
        LastScene = SceneManager.GetActiveScene().buildIndex;
        switch (result)
        {
            case 8: //Difficulty: Hard
                Difficulty = 2;
                break;
            case 7: //Difficulty: Medium
                Difficulty = 0;
                break;
            case 6: //Difficulty: Easy
                Difficulty = -2;
                break;
            case 5:     //Main Menu
                SceneManager.LoadScene(3);
                break;
            case 4:     //In settings if player selects "NO"
                FacingKinect = false;
                LEFTHAND = 11;
                RIGHTHAND = 7;
                break;
            case 3:     //In settings if player selects "YES"
                FacingKinect = true;
                LEFTHAND = 7;
                RIGHTHAND = 11;
                break;
            case 2:     //Settings
                SceneManager.LoadScene(4);
                break;
            case 1:     //Play or Play Again
                Reset();
                SceneManager.LoadScene(1);
                am.Stop("Menu Music");
                am.Play("Game Music");
                break;
            case 0:     //Recalibrate
                SceneManager.LoadScene(0);
                break;
            default:
                break;
        }
    }
}
