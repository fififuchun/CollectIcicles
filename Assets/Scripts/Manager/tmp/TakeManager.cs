using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeManager : MonoBehaviour
{
    private bool isHolding = false;  // 長押し中か
    // private bool hasPassedThrough = false; // Image を通過したか
    private RectTransform imageRect;  // Image の範囲情報

    void Start()
    {
        imageRect = GetComponent<RectTransform>(); // Image の RectTransform を取得
    }

    void Update()
    {
        // Vector2 mousePosition = Input.mousePosition;

        // マウスボタン押下（タップ開始）
        if (Input.GetMouseButtonDown(0))
        {
            isHolding = true;
            // hasPassedThrough = false;
            // Debug.Log("Hold started at: " + mousePosition);
        }

        // マウス移動中（ドラッグ）
        if (Input.GetMouseButton(0) && isHolding)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(imageRect, Input.mousePosition))
            {
                // hasPassedThrough = true;
                // Debug.Log($"topped {transform.parent.parent.name}");
                Transform _parent = transform.parent;
                _parent.GetComponent<Rigidbody2D>().gravityScale = 8;
                enabled = false;
            }
        }

        // マウスボタン離す（タップ終了）
        if (Input.GetMouseButtonUp(0))
        {
            // if (hasPassedThrough) Debug.Log("Hold passed through Image before release!");            }
            isHolding = false;
        }
    }
}
