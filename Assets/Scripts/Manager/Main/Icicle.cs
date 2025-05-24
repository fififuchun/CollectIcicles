using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Icicle : MonoBehaviour
{
    // インスペクターから静的に代入
    #region "Function staticly"

    public int point; // 生える位置
    public int index; // この冷凍庫でのつららのindex
    public int growGrade; // 成長段階

    // 落下中の目の画像
    public Sprite dropEye;

    #endregion

    // 以下は動的に代入
    #region "Function dynamically"

    // Canvas
    [HideInInspector] public Canvas canvas;

    public GameObject eyeObj;

    private bool isHolding = false;  // 長押し中か
    private RectTransform panelRect;  // Image の範囲情報

    private Transform eyesTran;

    #endregion

    void Start()
    {
        // touchPanel の RectTransform を取得
        panelRect = transform.GetChild(0).GetComponent<RectTransform>();

        // 目のTransform
        eyesTran = transform.GetChild(1);
    }

    void Update()
    {
        // Debug.Log(transform.position);

        // マウスボタン押す（タップ開始）
        if (Input.GetMouseButtonDown(0)) isHolding = true;

        // マウス移動中（ドラッグ）
        if (Input.GetMouseButton(0) && isHolding && growGrade > 1)
        {
            // Debug.Log(Input.mousePosition);Vector2 localPoint;
            Vector2 localPoint;
            bool isInside = RectTransformUtility.ScreenPointToLocalPointInRectangle(
                panelRect, Input.mousePosition, canvas.worldCamera, out localPoint);

            if (isInside && panelRect.rect.Contains(localPoint))
            {
                GetComponent<Rigidbody2D>().gravityScale = 0.1f;
                GatherIcicle();
                enabled = false;
            }
        }

        // マウスボタン離す（タップ終了）
        if (Input.GetMouseButtonUp(0)) isHolding = false;
    }

    // つらら生成時の初期化処理
    public void GenerateIcicle(int _index)
    {
        // 現在のつらら図鑑で何番目のつららか
        index = _index;

        // 情報を格納
        Icicles icicleInfo = Const.icicleSO_Array[Const.freezerNum].icicles[_index];

        GetComponent<Image>().sprite = icicleInfo.image;
        RectTransform thisRect = transform as RectTransform;

        // つららの大きさが変わる場合
        if (icicleInfo.scale_x != 1.0f || icicleInfo.scale_y != 1.0f)
        {
            Debug.Log($"Change Scale to x: {icicleInfo.scale_x}, y: {icicleInfo.scale_y} in point: {point}");
            thisRect.sizeDelta = new Vector2(
                thisRect.sizeDelta.x * icicleInfo.scale_x, thisRect.sizeDelta.y * icicleInfo.scale_y);
        }

        // つららの目が変わる場合
        if (icicleInfo.eyeId != 0)
        {
            Debug.Log($"Change Eye to {icicleInfo.eyeId} in {point}");
            Destroy(eyesTran.gameObject);

            GameObject _eyeObj = Instantiate(eyeObj, transform);
            _eyeObj.transform.localPosition = new Vector3(0, icicleInfo.eye_y);
        }
        // 変わらない場合、目の位置を調整
        else transform.GetChild(1).localPosition = new Vector3(0, icicleInfo.eye_y, 0);
    }

    // つららが収穫された時に呼び出される関数
    public void GatherIcicle()
    {
        ChangeEye();

        // 図鑑登録処理?
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
