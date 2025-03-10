using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowManager : MonoBehaviour
{
    // 成長するつららの位置、ここでアタッチされたGameObjectの子オブジェクトとしてつららが生成される
    [SerializeField] private GameObject[] growPoints = new GameObject[Const.maxIcicle];

    // つららの成長段階、
    // 0: 生えていない
    // 1: ちびつらら、回収できない
    // 2: 中つらら、回収できるがポイントが低い
    // 3: つらら、レアつららとの抽選が入る
    public static int[] growGrades = new int[Const.maxIcicle];

    public GameObject babyIcicle;
    public GameObject icicle;


    // // Buttonにアタッチ
    // public void GrowIcicle()
    // {
    //     List<int> canGrowPoints = CanGrowPoint();
    //     if (canGrowPoints.Count == 0) return;

    //     int growPoint = canGrowPoints[UnityEngine.Random.Range(0, canGrowPoints.Count)];
    //     Grow(growPoint);
    //     // growGrades[growPoint]++;
    //     // growPoints[growPoint].//.GetComponent<Icicle>().GrowIcicle();
    // }

    // つららを成長させる
    public void Grow(int growPoint)
    {
        if (growGrades[growPoint] < 0 || growGrades[growPoint] >= Const.maxGrowGrade)
        {
            Debug.LogError("Invalid grow grade");
            return;
        }
        growGrades[growPoint]++;

        // 成長段階によって処理を変える
        switch (growGrades[growPoint])
        {
            case 1:
                // ちびつららを生成
                GameObject _babyIcicle = Instantiate(babyIcicle, growPoints[growPoint].transform.position, Quaternion.identity);
                _babyIcicle.transform.SetParent(growPoints[growPoint].transform);
                _babyIcicle.transform.localPosition = new Vector3(0, -140, 0);
                break;
            case 2:
                // 中つららを生成
                GameObject _icicle = Instantiate(icicle, growPoints[growPoint].transform.position, Quaternion.identity);
                _icicle.transform.SetParent(growPoints[growPoint].transform);
                _icicle.transform.localPosition = new Vector3(0, -140, 0);
                break;
            case 3:
                // レアつららを生成
                break;
            default:
                Debug.LogError("Invalid grow grade: inside switch");
                break;
        }
    }

    // 成長可能なつららの場所のindexのリストを返す
    // public List<int> CanGrowPoint()
    // {
    //     // 成長可能なつららの場所のindexのリスト
    //     List<int> canGrowPoints = new List<int>();
    //     for (int i = 0; i < Const.maxIcicle; i++) if (growGrades[i] < Const.maxGrowGrade) canGrowPoints.Add(i);

    //     return canGrowPoints;
    // }

    void Update()
    {
        // print(String.Join(",", CanGrowPoint()));
    }
}
