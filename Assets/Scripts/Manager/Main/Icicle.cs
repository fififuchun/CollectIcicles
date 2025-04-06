using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Icicle : MonoBehaviour
{
    public int point; // 生える位置
    public int id; // つららのID
    public int growGrade; // 成長段階

    [HideInInspector] public UnityEvent<int> OnGet; // 獲得時のイベント

    private bool isHolding = false;  // 長押し中か
    private RectTransform imageRect;  // Image の範囲情報

    void Start()
    {
        imageRect = transform.GetChild(0).GetComponent<RectTransform>(); // touchPanel の RectTransform を取得
    }

    void Update()
    {
        // if (transform.position.y < 600)
        // {
        //     Debug.Log($"Reset: {transform.parent.name.Split("_")[1]}");
        //     // GrowManager.growGrades[int.Parse(transform.parent.name.Split("_")[1])] = 0;
        //     Destroy(gameObject);
        // }

        // マウスボタン押す（タップ開始）
        if (Input.GetMouseButtonDown(0)) isHolding = true;

        // マウス移動中（ドラッグ）
        if (Input.GetMouseButton(0) && isHolding && growGrade > 1)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(imageRect, Input.mousePosition))
            {
                GetComponent<Rigidbody2D>().gravityScale = 8;
                OnGet.Invoke(point);
                enabled = false;
            }
        }

        // マウスボタン離す（タップ終了）
        if (Input.GetMouseButtonUp(0)) isHolding = false;
    }
}
