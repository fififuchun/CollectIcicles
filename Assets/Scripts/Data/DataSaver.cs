using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using TMPro;
using FuchunLibrary;

public class DataSaver : MonoBehaviour
{
    // json変換するデータのクラス
    [HideInInspector] public SaveData data;

    // jsonファイルのパス
    string filepath;

    // jsonファイル名
    string fileName = "Datas/Data.json";

    // icicleSO
    // [SerializeField] private IcicleSO icicleSO;

    //--------------------------------------------------------------------------------

    #region "Essentials"
    // 開始時にファイルチェック、読み込み
    async void Awake()
    {
        // パス名取得
#if UNITY_EDITOR
        filepath = Application.dataPath + "/" + fileName;

#elif UNITY_ANDROID
        filepath = Application.persistentDataPath + "/" + fileName;

#else
        filepath = Application.dataPath + "/" + fileName;

#endif

        // ファイルがないとき、ファイル作成
        if (!File.Exists(filepath)) Save();

        // ファイルを読み込んでdataに格納
        data = Load(filepath);

        // データを基に数値を初期化
        InitAppearance();

        await LoadAssets.WaitUntilLoadedAsync();

        unlockedIcicles2D = Library.ConvertDimOneToTwo(data.isUnlockedIcicles, Const.maxfreezerCount, Const.maxIcicleTypePerBook);
        Library.Print2DBoolArray(unlockedIcicles2D, "解放済みのつららリスト");

        RefreshCanGather();
    }

    // jsonとしてデータを保存
    private void Save(SaveData data)
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

    public void Save() { Save(data); }

    // jsonファイル読み込み
    private SaveData Load(string path)
    {
        // ファイル読み込み指定
        StreamReader rd = new StreamReader(path);

        // ファイル内容全て読み込む
        string json = rd.ReadToEnd();

        // ファイル閉じる
        rd.Close();

        // jsonファイルを型に戻して返す
        return JsonUtility.FromJson<SaveData>(json);
    }

    // ゲーム終了時に保存
    void OnDestroy()
    {
        Save();
    }

    #endregion

    //--------------------------------------------------------------------------------
    #region "Initialize"

    // ゲームそのものを初期化する
    public void InitializeGameData()
    {
        GetCoin(-TCoin);
        data.freezerNum = 0;
        data.isUnlockedIcicles = new bool[data.isUnlockedIcicles.Length];

        Save();
    }

    // ゲーム内数値の見た目を初期化する
    private void InitAppearance()
    {
        UpdateCoin();
    }

    #endregion

    //--------------------------------------------------------------------------------
    #region "Coin"

    // コインのテキスト
    [SerializeField] private TextMeshProUGUI t_coinText;

    public int TCoin { get => data.t_coin; }
    public void GetCoin(int i)
    {
        data.t_coin += i;
        UpdateCoin();
        Save();
    }

    public void UpdateCoin()
    {
        t_coinText.text = TCoin.ToString();
    }

    #endregion

    //--------------------------------------------------------------------------------
    #region "Unlocked Icicles"

    // 保存したつらら解放状況の一次元bool配列を変換するための二次元配列
    private bool[,] unlockedIcicles2D = new bool[Const.maxfreezerCount, Const.maxIcicleTypePerBook];

    // 現在の冷凍庫で収穫可能なつららのIndexリスト
    [SerializeField] private bool[] canGatherIcicleIndex = new bool[Const.maxIcicleTypePerBook];

    /// <summary>
    /// 現在の冷凍庫で収穫可能なつららのIndexリストを更新する
    /// </summary>
    public void RefreshCanGather()
    {
        for (int i = 0; i < Const.maxIcicleTypePerBook; i++)
        {
            int[] reqUnlockIndexes = Const.icicleSO_Array[Const.freezerNum].icicles[i].requiredUnlock;
            bool canUnlock_i = true;

            foreach (int reqIndex in reqUnlockIndexes)
            {
                if (!data.isUnlockedIcicles[data.freezerNum * Const.maxIcicleTypePerBook + reqIndex])
                {
                    canUnlock_i = false;
                    break;
                }
            }
            canGatherIcicleIndex[i] = canUnlock_i;
        }

        Debug.Log($"canGatherIcicleIndex: {String.Join(",", canGatherIcicleIndex)}");
        // Library.Print2DBoolArray
    }

    /// <summary>
    /// 解放したい冷凍庫番号・つらら番号を入力すると、対応したつららを解放する
    /// </summary>
    public void UnlockIcicle(int freezerNum, int icicleIndex)
    {
        if (freezerNum > Const.maxfreezerCount - 1) throw new IndexOutOfRangeException("不正な冷凍庫のIndexを受け取りました");
        if (icicleIndex > Const.maxIcicleTypePerBook - 1) throw new IndexOutOfRangeException("不正なつららのIndexを受け取りました");

        if (data.isUnlockedIcicles[freezerNum * Const.maxIcicleCount + icicleIndex]) { Debug.Log("already Unlocked"); return; }
        else
        {
            // 一次元配列のまま、直接解放
            data.isUnlockedIcicles[freezerNum * Const.maxIcicleCount + icicleIndex] = true;

            //収穫可能なつららを更新
            RefreshCanGather();

            Debug.Log($"freezerIndex: {freezerNum}, icicleIndex: {icicleIndex}のつららを解放しました");
            Save();
        }
    }

    #endregion
}
