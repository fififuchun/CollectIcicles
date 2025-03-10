using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class IcicleManager : MonoBehaviour
{
    // 成長するつららの位置、ここでアタッチされたGameObjectの子オブジェクトとしてつららが生成される
    [SerializeField] private GameObject[] growPoints = new GameObject[Const.maxIcicle];
    [SerializeField] private Icicle[] icicles = new Icicle[Const.maxIcicle];
    [SerializeField] private int[] growGrades = new int[Const.maxIcicle];


    [SerializeField] private GameObject babyIcicle;
    [SerializeField] private Sprite midIcicle; // 中つららの画像
    [SerializeField] private Sprite normalIcicle; // つららの画像

    void Start()
    {

    }

    void Update()
    {

    }

    // Buttonにアタッチ
    public void GrowIcicle()
    {
        List<int> canGrowPoints = CanGrowPoint();
        if (canGrowPoints.Count == 0) return;

        int growPoint = canGrowPoints[UnityEngine.Random.Range(0, canGrowPoints.Count)];
        Grow(growPoint);
        // icicles[growPoint].growGrade++;
        // growPoints[growPoint].//.GetComponent<Icicle>().GrowIcicle();
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
            GameObject _babyIcicle = Instantiate(babyIcicle, growPoints[growPoint].transform.position, Quaternion.identity);
            _babyIcicle.transform.SetParent(growPoints[growPoint].transform);
            _babyIcicle.transform.localPosition = new Vector3(0, -140, 0);

            icicles[growPoint] = _babyIcicle.GetComponent<Icicle>();
            icicles[growPoint].point = growPoint;
        }

        // 成長レベルを上げる
        icicles[growPoint].growGrade++;
        growGrades = GetAllGrades();

        // 成長段階によって処理を変える
        switch (icicles[growPoint].growGrade)
        {
            case 1:
                // Debug.Log("ちびつららを生成しました");
                break;
            case 2:
                // 中つららを生成
                icicles[growPoint].GetComponent<Image>().sprite = midIcicle;
                icicles[growPoint].transform.GetChild(0).localPosition = new Vector3(0, 100, 0); // 目の位置を調整
                break;
            case 3:
                // ノーマルつららを生成
                icicles[growPoint].GetComponent<Image>().sprite = normalIcicle;
                break;
            default:
                Debug.LogError("Invalid grow grade: inside switch");
                break;
        }
    }

    // 成長可能なつららの場所のindexのリストを返す
    public List<int> CanGrowPoint()
    {
        // 成長可能なつららの場所のindexのリスト
        List<int> canGrowPoints = new List<int>();
        for (int i = 0; i < Const.maxIcicle; i++)
        {
            if (icicles[i] == null) canGrowPoints.Add(i);
            else if (icicles[i].growGrade < Const.maxGrowGrade) canGrowPoints.Add(i);
        }

        return canGrowPoints;
    }

    public int[] GetAllGrades()
    {
        return icicles?.Select(icicle => icicle?.growGrade ?? 0).ToArray() ?? new int[0];
    }
}
