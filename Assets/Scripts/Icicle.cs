using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icicle : MonoBehaviour
{
    public int iciclePoint;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localPosition.y < -400)
        {
            Destroy(gameObject);
        }
    }
}
