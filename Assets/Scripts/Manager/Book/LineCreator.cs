using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCreator : MonoBehaviour
{
    // Lineを描画する親のcontent(ScrollView)
    [SerializeField] private RectTransform contentRect;

    // 図鑑の各cellの情報をCellCreatorから取得
    private int[] cellInfo = new int[4];

    // Lineの余裕;
    private int lineOffset = 10;

    // 
    private Vector2[] positions = new Vector2[2];

    //
    // private Vector3 midPos;

    void Start()
    {

    }

    /// <summary>
    /// SOに入力した座標をもとにつらら図鑑のセルの位置を計算する
    /// </summary>
    public void SetPos(Vector2Int start, Vector2Int end)
    {
        // Debug.Log(String.Join(",", start));
        // Debug.Log(String.Join(",", end));

        float content_H = contentRect.sizeDelta.y;

        float start_x = cellInfo[3] * (start.x - 1);
        float start_y = content_H / 2 + lineOffset - (cellInfo[0] + cellInfo[2] + (cellInfo[0] + cellInfo[1]) * (start.y + 2));

        float end_x = cellInfo[3] * (end.x - 1);
        float end_y = content_H / 2 - lineOffset - (cellInfo[0] + cellInfo[1] + cellInfo[2] + (cellInfo[0] + cellInfo[1]) * (end.y + 1));

        positions[0] = new Vector3(start_x, start_y);
        positions[1] = new Vector3(end_x, end_y);

        Vector2 midPos = (positions[0] + positions[1]) / 2;
        this.transform.localPosition = midPos;

        RectTransform lineRect = transform as RectTransform;
        lineRect.sizeDelta = new Vector2(30, (positions[1] - positions[0]).magnitude);

        // Debug.Log($"{positions[0]}, {positions[1]}");
    }

    /// <summary>
    /// セルの位置を計算する上で必要なパラメータを渡す
    /// </summary>
    public void SetCellInfo(int cellHeight, int lineHeight, int viewPadding, int cellWidth)
    {
        cellInfo[0] = cellHeight;
        cellInfo[1] = lineHeight;
        cellInfo[2] = viewPadding;
        cellInfo[3] = cellWidth;
    }
}
