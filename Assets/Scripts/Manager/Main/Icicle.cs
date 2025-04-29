using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Icicle : MonoBehaviour
{
    public int point; // 生える位置
    public int id; // つららのID
    public int growGrade; // 成長段階

    // つららSO
    public IcicleSO icicleSO;

    // 落下中の目の画像
    public Sprite dropEye;

    public GameObject eyeObj;

    private bool isHolding = false;  // 長押し中か
    private RectTransform panelRect;  // Image の範囲情報

    private Transform eyesTran;

    void Start()
    {
        // touchPanel の RectTransform を取得
        panelRect = transform.GetChild(0).GetComponent<RectTransform>();

        // 目のTranform
        eyesTran = transform.GetChild(1);
    }

    void Update()
    {
        // マウスボタン押す（タップ開始）
        if (Input.GetMouseButtonDown(0)) isHolding = true;

        // マウス移動中（ドラッグ）
        if (Input.GetMouseButton(0) && isHolding && growGrade > 1)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(panelRect, Input.mousePosition))
            {
                GetComponent<Rigidbody2D>().gravityScale = 8;
                OnGet();
                enabled = false;
            }
        }

        // マウスボタン離す（タップ終了）
        if (Input.GetMouseButtonUp(0)) isHolding = false;
    }

    // つらら生成時の初期化処理
    public void GenerateIcicle(int _id)
    {
        id = _id;
        GetComponent<Image>().sprite = icicleSO.icicles[_id].image;
        RectTransform thisRect = transform as RectTransform;

        // つららの大きさが変わる場合
        if (icicleSO.icicles[_id].scale_x != 1.0f || icicleSO.icicles[_id].scale_y != 1.0f)
        {
            Debug.Log($"Change Scale to x: {icicleSO.icicles[_id].scale_x}, y: {icicleSO.icicles[_id].scale_y} in point: {point}");
            thisRect.sizeDelta = new Vector2(
                thisRect.sizeDelta.x * icicleSO.icicles[_id].scale_x, thisRect.sizeDelta.y * icicleSO.icicles[_id].scale_y);
        }

        // つららの目が変わる場合
        if (icicleSO.icicles[_id].eyeId != 0)
        {
            Debug.Log($"Change Eye to {icicleSO.icicles[_id].eyeId} in {point}");
            Destroy(eyesTran.gameObject);

            GameObject _eyeObj = Instantiate(eyeObj, transform);
            _eyeObj.transform.localPosition = new Vector3(0, icicleSO.icicles[_id].eye_y);
        }
        // 変わらない場合、目の位置を調整
        else transform.GetChild(1).localPosition = new Vector3(0, icicleSO.icicles[_id].eye_y, 0);
    }

    // つららが収穫された時に呼び出される関数
    public void OnGet()
    {
        ChangeEye();

        // 図鑑登録処理
    }

    // 落下時の目を変える処理
    public void ChangeEye()
    {
        Transform eyesTran = transform.GetChild(1);
        RectTransform[] eyesRect = new RectTransform[eyesTran.childCount];

        for (int i = 0; i < eyesRect.Length; i++)
        {
            eyesRect[i] = eyesTran.GetChild(i) as RectTransform;
            eyesRect[i].sizeDelta *= 1.6f;
            eyesRect[i].eulerAngles = Vector3.zero;
            eyesRect[i].GetComponent<Image>().sprite = dropEye;
        }
    }
}
