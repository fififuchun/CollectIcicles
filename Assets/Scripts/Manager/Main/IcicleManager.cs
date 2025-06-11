using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class IcicleManager : MonoBehaviour
{
    // 現在設定中の冷凍庫の番号
    // public int freezerNum;

    // Canvas
    [SerializeField] private Canvas canvas;

    // インスタンス
    [SerializeField] private DataSaver dataSaver;
    // [SerializeField] private IcicleSO icicleSO;
    [SerializeField] private Icicle[] icicles = new Icicle[Const.maxIcicleCount];


    // 成長するつららの位置、ここでアタッチされたGameObjectの子オブジェクトとしてつららが生成される
    [SerializeField] private GameObject[] growPoints = new GameObject[Const.maxIcicleCount];
    [SerializeField] private int[] growGrades = new int[Const.maxIcicleCount];

    // 小つららのPrefab
    [SerializeField] private GameObject babyIcicle;
    [SerializeField] private GameObject[] eyeObjects;

    // 落下中の目の画像
    [SerializeField] private Sprite dropEye;

    void Start()
    {
        // freezerNum = dataSaver.data.freezerNum;
        Debug.Log($"現在使用中のfreezerIndexは: {Const.freezerNum}");

        // これはStart時点でSOを読み込んでいないため、呼び出せない
        // Debug.Log($"{Const.icicleSO_Array[Const.freezerNum].icicles[2].icicleName}");
        // GrowIcicle();

        // ConfirmProp(new int[5] { 0, 1, 3, 6, 0 });
    }

    void Update()
    {
        // つららをクーラーボックスで回収
        for (int point = 0; point < icicles.Length; point++)
            if (icicles[point] == null) continue;
            else if (icicles[point].transform.localPosition.y < -300)
            {
                Debug.Log($"Reset: {point}");
                dataSaver.GetCoin(Const.icicleSO_Array[Const.freezerNum].icicles[icicles[point].index].iciclePoint);
                dataSaver.UnlockIcicle(Const.freezerNum, icicles[point].index);

                Destroy(icicles[point].gameObject);
                icicles[point] = null;
                growGrades = GrowGrades();
            }
    }

    // Buttonにアタッチ
    public void GrowIcicle()
    {
        List<int> canGrowPoints = CanGrowPoint();
        if (canGrowPoints.Count == 0) return;

        int growPoint = canGrowPoints[UnityEngine.Random.Range(0, canGrowPoints.Count)];
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
            if (icicles[growPoint].growGrade >= Const.maxGrowGrade) { Debug.LogError("これ以上成長できません"); return; }
            if (icicles[growPoint].growGrade < 0) { Debug.LogError("Invalid grow grade"); return; }
        }
        else // 初生成ならオブジェクトを生成
        {
            GameObject _babyIcicle = Instantiate(babyIcicle, growPoints[growPoint].transform);
            _babyIcicle.transform.localPosition = new Vector3(0, 10, 0);

            icicles[growPoint] = _babyIcicle.GetComponent<Icicle>();
            icicles[growPoint].point = growPoint;
            icicles[growPoint].index = -1;
            icicles[growPoint].canvas = canvas;
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
                icicles[growPoint].GenerateIcicle(0);
                break;
            case 3:
                if (UnityEngine.Random.Range(0, 2) == 0) // レアつらら生成時の処理
                {
                    // int rareIndex = 6;
                    int rareIndex = ChooseRareIcicle(dataSaver.CanCollectIcicleProp());
                    Debug.Log($"rareIndex: {rareIndex}");
                    if (rareIndex < 0) // 初期状態: rareIndex = -1 なら返す
                    {
                        icicles[growPoint].GenerateIcicle(1);
                        return;
                    }

                    icicles[growPoint].eyeObj = eyeObjects[Const.icicleSO_Array[Const.freezerNum].icicles[rareIndex].eyeId];
                    icicles[growPoint].GenerateIcicle(rareIndex);

                    Debug.Log($"Grow Icicle: {rareIndex} in {icicles[growPoint].point}");
                }
                else
                {
                    icicles[growPoint].GenerateIcicle(1);
                    Debug.Log($"Grow Icicle: {1} in {icicles[growPoint].point}");
                }

                break;
            default:
                Debug.LogError("Invalid grow grade: inside switch");
                break;
        }
    }

    // つららの成長具合を長さ20のint配列で返す
    public int[] GrowGrades() { return icicles?.Select(icicle => icicle?.growGrade ?? 0).ToArray() ?? new int[0]; }

    /// <summary>
    /// レア度の列を入力すると、その高さに応じた確率で入力のindexを返す
    /// 例えば、[1, 3, 6]を入力した場合、10%で0, 30%で1, 60%で2 を返す
    /// </summary>
    /// <param name="rareGradeArray">つららのレア度配列</param>
    /// <returns></returns>
    public int ChooseRareIcicle(int[] rareGradeArray)
    {
        // 不正な入力なら-1を返す
        if (rareGradeArray.Length != Const.maxIcicleTypePerBook) return -2;

        int sum = rareGradeArray.Sum();
        int randomNum = UnityEngine.Random.Range(0, sum);

        for (int i = 0; i < rareGradeArray.Length; i++)
        {
            randomNum -= rareGradeArray[i];
            if (randomNum < 0) return i;
        }

        return -1;
    }

    // Propが本当か確かめる
    public void ConfirmProp(int[] array)
    {
        int[] propArray = new int[array.Length];

        for (int i = 0; i < 10000; i++)
        {
            int prop = ChooseRareIcicle(array);
            propArray[prop]++;
        }

        Debug.Log(String.Join(",", propArray));
    }
}
