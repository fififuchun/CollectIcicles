using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class MatrixText : MonoBehaviour
{
    //状況ごとのチュートリアルメッセージ
    [SerializeField] private MatrixTextSO matrixTextSO;

    //自分自身
    private TextMeshProUGUI matrixElementText;

    //tutorialSOの位置
    //行
    [SerializeField] private int matrixRowNum;
    //列
    [SerializeField, ReadOnly] private int matrixColumnNum;

    // jsonファイルのパス
    string filepath;

    // jsonファイル名
    string fileName = "MatrixTextData.json";


    //==================================================(50)
    //関数
    // 開始時にファイルチェック、読み込み
    void Awake()
    {
        // パス名取得
        filepath = Application.dataPath + "/" + fileName;

        // ファイルがないとき、ファイル作成
        // if (!File.Exists(filepath)) Save(matrixTextSO);
        if (matrixTextSO.stringGroups.Length != 0) Save(matrixTextSO);

        // ファイルを読み込む
        // matrixTextSO.stringGroups = Load(filepath);
        // Initialize(matrixRowNum);
    }

    // jsonとしてデータを保存
    public void Save(MatrixTextSO data)
    {
        // jsonとして変換
        string json = JsonUtility.ToJson(data, true);

        // ファイル書き込み指定
        StreamWriter wr = new StreamWriter(filepath, false);

        // json変換した情報を書き込み
        wr.WriteLine(json);

        // ファイル閉じる
        wr.Close();
    }

    // jsonファイル読み込み
    private MatrixTextData[] Load(string path)
    {
        // ファイル読み込み指定
        StreamReader rd = new StreamReader(path);

        // ファイル内容全て読み込む
        string json = rd.ReadToEnd();
        Debug.Log(json);

        // ファイル閉じる
        rd.Close();

        // jsonファイルを型に戻して返す
        return JsonUtility.FromJson<MatrixTextData[]>(json);
    }

    /// <summary>
    /// matrixRowNumを初期化
    /// </summary>
    public void Initialize(int _matrixRowNum)
    {
        //指定された_matrixRowNumがtutorialSOの行数を超えてたらreturn
        if (_matrixRowNum < 0 || matrixTextSO.stringGroups.Count() <= _matrixRowNum)
        {
            gameObject.transform.parent.gameObject.SetActive(false);
            Debug.Log("入力されたmatrixRowNumは存在しません");
            return;
        }
        //指定された_matrixRowNum行目にテキストがないならreturn
        if (matrixTextSO.stringGroups[_matrixRowNum].strings.Length == 0)
        {
            gameObject.transform.parent.gameObject.SetActive(false);
            Debug.Log("入力されたmatrixRowNumにテキストは存在しません");
            return;
        }

        //指定された_matrixRowNumがtutorialSOの行数を超えてなかったら初期化
        matrixRowNum = _matrixRowNum;

        //MatrixTextがアタッチされているGameObjectにTextコンポーネントがないなら
        if (gameObject.GetComponent<TextMeshProUGUI>() == null)
        {
            Debug.Log("textコンポーネントをアタッチしてください");
            return;
        }
        matrixElementText = gameObject.GetComponent<TextMeshProUGUI>();

        //親のボタンをAppear
        gameObject.transform.parent.gameObject.SetActive(true);

        //表示すべきSOが存在し、Textコンポーネントもあるなら、それを初期化
        matrixElementText.text = matrixTextSO.stringGroups[matrixRowNum].strings[matrixColumnNum];

        //親のボタンにmatrixTextの列を次に進める関数を設定
        transform.parent.GetComponent<Button>()?.onClick.RemoveAllListeners();
        transform.parent.GetComponent<Button>()?.onClick.AddListener(PushGoNextButton);
    }

    /// <summary>
    /// matrixTextの列を次に進める
    /// </summary>
    private void PushGoNextButton()
    {
        // Debug.Log($"{matrixColumnNum}へ進む");
        matrixColumnNum++;
        if (matrixTextSO.stringGroups[matrixRowNum].strings.Length <= matrixColumnNum)
        {
            // Debug.Log($"{matrixColumnNum}, テキストがありません");
            matrixColumnNum = 0;
            gameObject.transform.parent.gameObject.SetActive(false);
            return;
        }

        matrixElementText.text = matrixTextSO.stringGroups[matrixRowNum].strings[matrixColumnNum];
    }
}
