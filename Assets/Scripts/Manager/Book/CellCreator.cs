using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cysharp.Threading.Tasks;

// 図鑑におけるセルの生成を行う
// [ExecuteAlways]
public class CellCreator : MonoBehaviour
{
    // 線を生成する
    [SerializeField] private LineCreator[] lineCreators;
    [SerializeField] private GameObject lineObj;

    // 図鑑の1セルの高さ
    private int cellHeight = 485;
    [SerializeField] private int lineHeight = 150;
    private int viewPadding = 100;
    private int cellWidth = 350;

    // スクロールビューのセルの親
    [SerializeField] private RectTransform content;
    [SerializeField] private Transform lineObjsTran;

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
    }

    [SerializeField] private List<Vector2Int> connectList = new List<Vector2Int>();

    async void Start()
    {
        await LoadAssets.WaitUntilLoadedAsync();

        // ScrollViewの高さを元にcellを生成
        // int contentHeight = ContentHeight();
        CreateCell();

        // cell同士を繋げるためのIndexのリスト
        connectList = ConnectCellList();
        // foreach (Vector2Int line in connectList) Debug.Log(String.Join(",", line));

        // リストを基にLineを生成
        lineCreators = new LineCreator[connectList.Count()];
        for (int i = 0; i < lineCreators.Length; i++)
        {
            GameObject gameObject = Instantiate(lineObj, lineObjsTran);
            lineCreators[i] = gameObject.GetComponent<LineCreator>();
            lineCreators[i].SetCellInfo(cellHeight, lineHeight, viewPadding, cellWidth);
            lineCreators[i].SetPos(BookPos(connectList[i][0]), BookPos(connectList[i][1]));
        }

        lineObj.SetActive(false);

        // OnStartCompleted();

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

    // Start完了後に呼びたい関数
    // private void OnStartCompleted()
    // {
    //     Debug.Log(Time.time);
    // }

    /// <summary>
    /// icicleSOとGameObjectを連携
    /// </summary>
    public void CreateCell()
    {
        for (int i = 0; i < Const.maxIcicleTypePerBook; i++)
        {
            _cellPrefabs[i].anchoredPosition =
                new Vector3((
                    Const.icicleSO_Array[Const.freezerNum].icicles[i].book_x - 1) * 350,
                    -((2 + Const.icicleSO_Array[Const.freezerNum].icicles[i].book_y) * (cellHeight + lineHeight) + viewPadding + cellHeight / 2),
                    0);

            icicleNames[i].text = Const.icicleSO_Array[Const.freezerNum].icicles[i].icicleName;

            Sprite icicleSprite = Const.icicleSO_Array[Const.freezerNum].icicles[i].image;
            if (icicleSprite != null) images[i].sprite = icicleSprite;

            coinTexts[i].text = Const.icicleSO_Array[Const.freezerNum].icicles[i].iciclePoint.ToString();
        }

        content.sizeDelta = new Vector2Int(Mathf.FloorToInt(content.sizeDelta.x), ContentHeight());
    }

    /// <summary>
    /// つらら図鑑に要する高さを出力
    /// </summary>
    public int ContentHeight()
    {
        // 特定の冷凍庫番号で出現するつららの最大数
        int[] y_row = new int[Const.maxIcicleTypePerBook];
        for (int i = 0; i < y_row.Length; i++) y_row[i] = Const.icicleSO_Array[Const.freezerNum].icicles[i].book_y;

        int contentHeightCount = y_row.Max() - y_row.Min() + 1;

        return contentHeightCount * cellHeight + (contentHeightCount - 1) * lineHeight + viewPadding * 2;
    }

    /// <summary>
    /// セルとセルをつなげるため、個別の冷凍庫のIndexを対にしたリストを返す
    /// </summary>
    public List<Vector2Int> ConnectCellList()
    {
        List<Vector2Int> connects = new List<Vector2Int>();

        for (int i = 0; i < Const.maxIcicleTypePerBook; i++)
        {
            // Debug.Log($"i: {i}");
            foreach (int j in Const.icicleSO_Array[Const.freezerNum].icicles[i].requiredUnlock)
            {
                connects.Add(new Vector2Int(j, i));
            }
        }

        return connects;
    }

    /// <summary>
    /// 図鑑のindexを指定すると、対応する図鑑のx,y座標を対にした配列を返す
    /// </summary>
    public Vector2Int BookPos(int index)
    {
        Icicles icicles = Const.icicleSO_Array[Const.freezerNum].icicles[index];
        return new Vector2Int(icicles.book_x, icicles.book_y);
    }
}
