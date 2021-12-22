using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickColor : MonoBehaviour
{
    int colorInt;
    // Start is called before the first frame update
    void Start()
    {
        colorInt = GetComponentInParent<BrickScript>().blockHealth;
    }

    // Update is called once per frame
    void Update()
    {
        colorInt = GetComponentInParent<BrickScript>().blockHealth;
        GetComponent<Renderer>().material = GetComponentInParent<BrickScript>().colors[colorInt];
    }
}
