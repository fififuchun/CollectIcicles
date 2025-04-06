using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class IcicleManager : MonoBehaviour
{
    [SerializeField] private DataSaver dataSaver;
    [SerializeField] private IcicleSO icicleData;
    [SerializeField] private Icicle[] icicles = new Icicle[Const.maxIcicle];


    // 成長するつららの位置、ここでアタッチされたGameObjectの子オブジェクトとしてつららが生成される
    [SerializeField] private GameObject[] growPoints = new GameObject[Const.maxIcicle];
    [SerializeField] private int[] growGrades = new int[Const.maxIcicle];


    [SerializeField] private GameObject babyIcicle;
    [SerializeField] private Sprite midIcicle; // 中つららの画像
    [SerializeField] private Sprite normalIcicle; // つららの画像

    void Update()
    {
        // つららをクーラーボックスで回収
        for (int i = 0; i < icicles.Length; i++)
            if (icicles[i] == null) continue;
            else if (icicles[i].transform.position.y < 600)
            {
                Debug.Log($"Reset: {i}");
                dataSaver.GetCoin(icicleData.icicles[icicles[i].id].iciclePoint);
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

        int growPoint = canGrowPoints[UnityEngine.Random.Range(0, canGrowPoints.Count)];
        Grow(growPoint);
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
            icicles[growPoint].OnGet.AddListener(growPoint => ChangeEye(growPoint));
            icicles[growPoint].point = growPoint;
            icicles[growPoint].id = 0;
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
                // 中つららを生成
                icicles[growPoint].id = 1;
                icicles[growPoint].GetComponent<Image>().sprite = midIcicle;
                // 目の位置を調整
                icicles[growPoint].transform.GetChild(1).localPosition = eyePos(icicles[growPoint].id);
                // icicles[growPoint].transform.GetChild(2).localPosition = new Vector3(0, 110, 0);
                break;
            case 3:
                // ノーマルつららを生成
                icicles[growPoint].id = 2;
                icicles[growPoint].GetComponent<Image>().sprite = normalIcicle;
                icicles[growPoint].transform.GetChild(1).localPosition = eyePos(icicles[growPoint].id);
                // icicles[growPoint].transform.GetChild(2).localPosition = new Vector3(0, 100, 0);
                break;
            default:
                Debug.LogError("Invalid grow grade: inside switch");
                break;
        }
    }

    private Vector3 eyePos(int id)
    {
        return new Vector3(0, icicleData.icicles[id].eye_y, 0);
    }

    public int[] GrowGrades()
    {
        return icicles?.Select(icicle => icicle?.growGrade ?? 0).ToArray() ?? new int[0];
    }

    public void ChangeEye(int growPoint)
    {
        if (icicles[growPoint] == null) return;

        Debug.Log($"Get Icicle in: {growPoint}");
        icicles[growPoint].transform.GetChild(1).gameObject.SetActive(false);

        Transform eyeTransform = icicles[growPoint].transform.GetChild(2);
        eyeTransform.gameObject.SetActive(true);
        eyeTransform.localPosition = eyePos(icicles[growPoint].id);
    }
}
