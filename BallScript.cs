using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    Rigidbody rb;
    private bool playerHitLast, hitBoundary;
    public bool FirstHit;
    private int bounceCount = 0;

    Vector3 startLoc = new Vector3(0, 3, -9.7f);
    public GameObject ball;
    public ParticleSystem ballEffect = null;
    GameManager gm;

    public Material[] colors = new Material[3];
    new Renderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        GetComponent<Renderer>().enabled = true;
        if (rb == null)
        {
            rb = GetComponentInParent<Rigidbody>();
        }
        renderer = GetComponentInParent<Renderer>();
        ResetColor();
        rb.isKinematic = false;
        FirstHit = true;
    }

    void FixedUpdate()
    {
        if (rb.velocity.magnitude < (5f + gm.Difficulty) && playerHitLast && rb.velocity.magnitude != 0)
        {
            rb.AddForce(Vector3.forward);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Brick"))
        {
            other.collider.GetComponent<BrickScript>().Hit();
            playerHitLast = false;
            bounceCount = 0;
        }
        else if (other.collider.CompareTag("BackWall"))
        {
            playerHitLast = false;
            bounceCount = 0;
        }
        else if (other.collider.CompareTag("Wall"))
        {
            if (bounceCount < 4) { bounceCount++; }
            else
            {
                if (playerHitLast) { rb.AddForce(Vector3.forward, ForceMode.Impulse); }
                else { rb.AddForce(-Vector3.forward, ForceMode.Impulse); }
            }
        }
        else if (other.collider.CompareTag("Player"))
        {
            if (other.gameObject.name == "LeftHand")
            {
                renderer.material = colors[1];
                Invoke(nameof(ResetColor), 0.5f);
            }
            if (other.gameObject.name == "RightHand")
            {
                renderer.material = colors[2];
                Invoke(nameof(ResetColor), 0.5f);
            }
            playerHitLast = true;
            bounceCount = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boundary") && !hitBoundary)
        {
            hitBoundary = true;
            ballEffect.Play();
            rb.isKinematic = true;
            GetComponent<Renderer>().enabled = false;   //make the ball disappear
            gm.ballsLeft--;
            FindObjectOfType<BallTrackScript>().Fade(gm.ballsLeft);
            Invoke(nameof(Deconstructor), 3f);
        }
    }

    private void Deconstructor()
    {
        if (gm.ballsLeft > 0)
        {
            Instantiate(ball, startLoc, Quaternion.Euler(new Vector3(0, Random.Range(-10, 10), Random.Range(-10, 10))));
        }
        Destroy(this.gameObject);
    }

    public void LevelReset()
    {
        ballEffect.Play();
        rb.isKinematic = true;
        GetComponent<Renderer>().enabled = false;   //make the ball disappear
        FindObjectOfType<BallTrackScript>().Reset();
        Invoke(nameof(Deconstructor), 3f);
    }

    public void FirstContact()
    {
        rb.AddForce(transform.forward * (6 + gm.stagesBeaten + gm.Difficulty), ForceMode.VelocityChange);
        hitBoundary = false;
        FirstHit = false;
        playerHitLast = true;
        bounceCount = 0;
    }

    void ResetColor()
    {
        renderer.material = colors[0];
    }
}
