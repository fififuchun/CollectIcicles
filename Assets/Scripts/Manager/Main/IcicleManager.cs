using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class IcicleManager : MonoBehaviour
{
    [SerializeField] private DataSaver dataSaver;
    [SerializeField] private IcicleSO icicleSO;
    [SerializeField] private Icicle[] icicles = new Icicle[Const.maxIcicleCount];


    // 成長するつららの位置、ここでアタッチされたGameObjectの子オブジェクトとしてつららが生成される
    [SerializeField] private GameObject[] growPoints = new GameObject[Const.maxIcicleCount];
    [SerializeField] private int[] growGrades = new int[Const.maxIcicleCount];

    // 小つららのPrefab
    [SerializeField] private GameObject babyIcicle;
    [SerializeField] private GameObject[] eyeObjects;

    // 落下中の目の画像
    [SerializeField] private Sprite dropEye;

    void Update()
    {
        // つららをクーラーボックスで回収
        for (int i = 0; i < icicles.Length; i++)
            if (icicles[i] == null) continue;
            else if (icicles[i].transform.position.y < 600)
            {
                Debug.Log($"Reset: {i}");
                dataSaver.GetCoin(icicleSO.icicles[icicles[i].id].iciclePoint);
                Destroy(icicles[i].gameObject);
                icicles[i] = null;
                growGrades = GrowGrades();
            }
    }

    // Buttonにアタッチ
    public void GrowIcicle()
    {
        List<int> canGrowPoints = CanGrowPoint();
        if (canGrowPoints.Count == 0) return;

        int growPoint = canGrowPoints[Random.Range(0, canGrowPoints.Count)];
        Grow(growPoint);
    }

    // 成長可能なつららの場所のindexのリストを返す
    public List<int> CanGrowPoint()
    {
        // 成長可能なつららの場所のindexのリスト
        List<int> canGrowPoints = new List<int>();
        for (int i = 0; i < Const.maxIcicleCount; i++)
        {
            if (icicles[i] == null) canGrowPoints.Add(i);
            else if (icicles[i].growGrade < Const.maxGrowGrade) canGrowPoints.Add(i);
        }

        return canGrowPoints;
    }

    // つららを成長させる
    public void Grow(int growPoint)
    {
        if (icicles[growPoint] != null) // 不正な入力を検知
        {
            // 成長させようとしたポイントの成長段階が0未満、または最大成長段階以上の場合はエラーを出力
            if (icicles[growPoint].growGrade >= Const.maxGrowGrade)
            {
                Debug.LogError("これ以上成長できません");
                return;
            }
            if (icicles[growPoint].growGrade < 0)
            {
                Debug.LogError("Invalid grow grade");
                return;
            }
        }
        else // 初生成ならオブジェクトを生成
        {
            GameObject _babyIcicle = Instantiate(babyIcicle, growPoints[growPoint].transform);
            _babyIcicle.transform.localPosition = new Vector3(0, 10, 0);

            icicles[growPoint] = _babyIcicle.GetComponent<Icicle>();
            icicles[growPoint].point = growPoint;
            icicles[growPoint].id = 0;
            // icicles[growPoint].icicleSO = icicleSO;
            // icicles[growPoint].dropEye = dropEye;
        }

        // 成長レベルを上げる
        icicles[growPoint].growGrade++;
        growGrades = GrowGrades();

        // 成長段階によって処理を変える
        switch (icicles[growPoint].growGrade)
        {
            case 1:
                // Debug.Log("ちびつららを生成しました");
                break;
            case 2:
                icicles[growPoint].GenerateIcicle(1);
                break;
            case 3:
                if (Random.Range(0, 2) == 0) // レアつらら生成時の処理
                {
                    int rareId = 5;
                    icicles[growPoint].eyeObj = eyeObjects[icicleSO.icicles[rareId].eyeId];
                    icicles[growPoint].GenerateIcicle(rareId);
                }
                else icicles[growPoint].GenerateIcicle(2);

                break;
            default:
                Debug.LogError("Invalid grow grade: inside switch");
                break;
        }
    }

    // つららの成長具合を長さ20のint配列で返す
    public int[] GrowGrades() { return icicles?.Select(icicle => icicle?.growGrade ?? 0).ToArray() ?? new int[0]; }
}
