using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icicle : MonoBehaviour
{
    // public int iciclePoint;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < 600)
        {
            Debug.Log($"Reset: {transform.parent.name.Split("_")[1]}");
            GrowManager.growGrades[int.Parse(transform.parent.name.Split("_")[1])] = 0;
            Destroy(gameObject);
        }
    }
}
