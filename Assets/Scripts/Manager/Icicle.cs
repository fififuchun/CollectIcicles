using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icicle : MonoBehaviour
{
    public int point; // 生える位置
    public int id; // つららのID
    public int growGrade; // 成長段階

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
