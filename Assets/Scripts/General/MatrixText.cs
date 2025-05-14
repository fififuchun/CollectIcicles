using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MatrixText : MonoBehaviour
{
    #region variables

    //状況ごとのチュートリアルメッセージ
    [SerializeField] private MatrixTextSO matrixTextSO;

    //自分自身
    private TextMeshProUGUI matrixElementText;

    //tutorialSOの位置
    //行
    [SerializeField] private int matrixRowNum;
    //列
    [SerializeField, ReadOnly] private int matrixColumnNum;

    // Buttonを消すかどうか
    [SerializeField] private bool isVanishButton = true;

    // jsonファイルのパス
    public static string filepath = Application.dataPath + "/" + "Datas/MatrixTextData.json";

    // jsonファイル名
    // string fileName = "MatrixTextData.json";

    #endregion

    #region methods
    //====================================================================================================100
    // 開始時にファイルチェック、読み込み
    void Awake()
    {
        // 初期化
        Initialize(matrixRowNum);
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

        // 列のラストのテキストなら返す
        if (matrixTextSO.stringGroups[matrixRowNum].strings.Length <= matrixColumnNum)
        {
            // Debug.Log($"{matrixColumnNum}, テキストがありません");
            matrixColumnNum = 0;
            matrixElementText.text = matrixTextSO.stringGroups[matrixRowNum].strings[matrixColumnNum];
            gameObject.transform.parent.gameObject.SetActive(!isVanishButton);
            return;
        }

        matrixElementText.text = matrixTextSO.stringGroups[matrixRowNum].strings[matrixColumnNum];
    }
    #endregion
}
