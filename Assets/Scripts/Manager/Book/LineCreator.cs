using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCreator : MonoBehaviour
{
    // Lineを描画する親のcontent(ScrollView)
    [SerializeField] private GameObject content;

    // 図鑑の各cellの情報をCellCreatorから取得
    private int[] cellInfo = new int[4];

    // Lineの余裕;
    private int lineOffset = 10;

    // 
    private Vector3[] positions = new Vector3[2];

    void Start()
    {
        // Debug.Log(String.Join(",", cellInfo));
        // SetPos();

        // LineRendererコンポーネントをゲームオブジェクトにアタッチする
        LineRenderer lineRenderer = GetComponent<LineRenderer>();

        // 点の数を指定する
        lineRenderer.positionCount = positions.Length;

        // 線を引く場所を指定する
        lineRenderer.SetPositions(positions);
    }

    public void SetPos(Vector2Int start, Vector2Int end)
    {
        // if (start.Length != 2 || end.Length != 2) return;

        // Debug.Log(String.Join(",", start));
        // Debug.Log(String.Join(",", end));

        float content_H = transform.parent.GetComponent<RectTransform>().sizeDelta.y;

        positions[0] = new Vector3(cellInfo[3] * (start.x - 1), content_H / 2 + lineOffset - (cellInfo[0] + cellInfo[2] + (cellInfo[0] + cellInfo[1]) * (start.y + 2)));
        positions[1] = new Vector3(cellInfo[3] * (end.x - 1), content_H / 2 - lineOffset - (cellInfo[0] + cellInfo[1] + cellInfo[2] + (cellInfo[0] + cellInfo[1]) * (end.y + 1)));

        // Debug.Log($"{positions[0]}, {positions[1]}");
    }

    public void SetCellInfo(int cellHeight, int lineHeight, int viewPadding, int cellWidth)
    {
        cellInfo[0] = cellHeight;
        cellInfo[1] = lineHeight;
        cellInfo[2] = viewPadding;
        cellInfo[3] = cellWidth;
    }
}
