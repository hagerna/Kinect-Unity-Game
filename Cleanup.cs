using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleanup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CheckDestroy());
    }

    IEnumerator CheckDestroy()
    {
        for (; ; )
        {
            if (GetComponentInChildren<BrickColor>() == null)
            {
                Destroy(this.gameObject);
            }
            yield return new WaitForSeconds(1f);
        }
    }
}
