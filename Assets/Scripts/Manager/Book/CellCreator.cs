using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// 図鑑におけるセルの生成を行う
// [ExecuteAlways]
public class CellCreator : MonoBehaviour
{
    // 線を生成する
    [SerializeField] private LineCreator[] lineCreators;
    [SerializeField] private GameObject lineObj;

    // どの冷蔵庫を使うか
    [SerializeField] private int freezerNum = 0;

    // 図鑑の1セルの高さ
    private int cellHeight = 485;
    [SerializeField] private int lineHeight = 150;
    private int viewPadding = 100;
    private int cellWidth = 350;

    // つららのSO
    [SerializeField] private IcicleSO icicleSO;

    // スクロールビューのセルの親
    [SerializeField] private RectTransform content;

    // つららのセルの内部オブジェクト
    // [SerializeField] private GameObject[] cellPrefabs = new GameObject[20]; // つららのセルの大元のObject
    [SerializeField] private RectTransform[] _cellPrefabs = new RectTransform[20]; // つららのセルの大元のObject
    [SerializeField] private TextMeshProUGUI[] icicleNames = new TextMeshProUGUI[20]; // つららの名前
    [SerializeField] private Image[] images = new Image[20]; // つららの画像
    [SerializeField] private Slider[] sliders = new Slider[20]; // つららをどれくらい収穫したか
    [SerializeField] private Image[,] rares = new Image[20, 6]; // つららのレア度
    [SerializeField] private TextMeshProUGUI[] coinTexts = new TextMeshProUGUI[20]; // つらら収穫時のコイン

    void Awake()
    {
        lineObj.transform.position = new Vector3(0, lineObj.transform.position.y, lineObj.transform.position.z);
        // List<int[]> connectList = ConnectCellList();
        // lineCreators = new LineCreator[connectList.Count()];
        // for (int i = 0; i < lineCreators.Length; i++)
        // {
        //     GameObject gameObject = Instantiate(lineObj, content.transform);
        //     lineCreators[i] = gameObject.GetComponent<LineCreator>();
        //     lineCreators[i].SetCellInfo(cellHeight, lineHeight, viewPadding);
        // }
    }

    [SerializeField] private List<int[]> connectList = new List<int[]>();

    void Start()
    {
        // ScrollViewの高さを元にcellを生成
        // int contentHeight = ContentHeight();
        CreateCell();

        // cell同士を繋げるためのIndexのリスト
        connectList = ConnectCellList();
        // foreach (int[] line in connectList) Debug.Log(String.Join(",", line));

        // リストを基にLineを生成
        lineCreators = new LineCreator[connectList.Count()];
        for (int i = 0; i < lineCreators.Length; i++)
        {
            GameObject gameObject = Instantiate(lineObj, content.transform);
            lineCreators[i] = gameObject.GetComponent<LineCreator>();
            lineCreators[i].SetCellInfo(cellHeight, lineHeight, viewPadding, cellWidth);
            lineCreators[i].SetPos(
                new int[] { Const.map[freezerNum][connectList[i][0], 0], Const.map[freezerNum][connectList[i][0], 1] },
                new int[] { Const.map[freezerNum][connectList[i][1], 0], Const.map[freezerNum][connectList[i][1], 1] }
                );
        }

        lineObj.SetActive(false);

        // Debug.Log();
        // foreach (int[] line in ConnectCell()) Debug.Log(String.Join(",", line));

        // アタッチ自動で
        // for (int i = 0; i < 20; i++)
        // {
        //     cellPrefabs[i] = content.transform.GetChild(i).gameObject;
        //     icicleNames[i] = cellPrefabs[i].transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        //     images[i] = cellPrefabs[i].transform.GetChild(1).GetChild(0).GetComponent<Image>();
        //     sliders[i] = cellPrefabs[i].transform.GetChild(2).GetComponent<Slider>();
        //     for (int j = 0; j < 6; j++)
        //     {
        //         rares[i, j] = cellPrefabs[i].transform.GetChild(3).GetChild(0).GetChild(j).GetComponent<Image>();
        //     }
        //     coinTexts[i] = cellPrefabs[i].transform.GetChild(3).GetChild(2).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        // }
    }

    // icicleSOとGameObjectを連携
    public void CreateCell()
    {
        for (int i = 0; i < Const.freezerIndex[freezerNum].Length; i++)
        {
            _cellPrefabs[i].anchoredPosition =
                new Vector3((
                    Const.map[freezerNum][i, 0] - 1) * 350,
                    -((2 + Const.map[freezerNum][i, 1]) * (cellHeight + lineHeight) + viewPadding + cellHeight / 2),
                    0);

            // Debug.Log((2 - Const.map[freezerNum][i, 1]) * (cellHeight + lineHeight) + viewPadding + cellHeight / 2);

            icicleNames[i].text = icicleSO.icicles[Const.freezerIndex[freezerNum][i]].icicleName;

            Sprite icicleSprite = icicleSO.icicles[Const.freezerIndex[freezerNum][i]].image;
            if (icicleSprite != null) images[i].sprite = icicleSprite;

            coinTexts[i].text = icicleSO.icicles[Const.freezerIndex[freezerNum][i]].iciclePoint.ToString();
        }

        content.sizeDelta = new Vector2(content.sizeDelta.x, ContentHeight());
    }

    // つらら図鑑に要する高さを出力
    public int ContentHeight()
    {
        // 特定の冷凍庫番号で出現するつららの最大数
        int icicleType = Const.map[freezerNum].GetLength(0);

        int[] y_row = new int[icicleType];
        for (int i = 0; i < icicleType; i++) y_row[i] = Const.map[freezerNum][i, 1];

        int contentHeightCount = y_row.Max() - y_row.Min() + 1;

        return contentHeightCount * cellHeight + (contentHeightCount - 1) * lineHeight + viewPadding * 2;
    }

    public List<int[]> ConnectCellList()
    {
        // for (int i = 0; i < Const.freezerIndex[freezerNum].Length; i++)

        List<int[]> connects = new List<int[]>();

        foreach (int i in Const.freezerIndex[freezerNum])
        {
            foreach (int j in icicleSO.icicles[i].requiredUnlock)
            {
                connects.Add(new int[2] { Array.IndexOf(Const.freezerIndex[freezerNum], j), Array.IndexOf(Const.freezerIndex[freezerNum], i) });
            }
        }

        return connects;
    }
}
