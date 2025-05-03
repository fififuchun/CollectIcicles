using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using TMPro;
using FuchunLibrary;
using System;
using Unity.VisualScripting;

public class DataSaver : MonoBehaviour
{
    // json変換するデータのクラス
    [HideInInspector] public static SaveData data;

    // jsonファイルのパス
    string filepath;

    // jsonファイル名
    string fileName = "Data.json";

    //--------------------------------------------------------------------------------

    #region "Essentials"
    // 開始時にファイルチェック、読み込み
    void Awake()
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

        // Debug.Log($"freezerIndex: {Const.freezerIndex.Length}");
        // Debug.Log($"map: {Const.map.Length}");
        // Debug.Log($"max freezer count: {Const.maxfreezerCount}");

        unlockedIcicles2D = Library.ConvertDimOneToTwo(data.isUnlockedIcicles, Const.maxfreezerCount, Const.maxIcicleTypePerBook);
        Library.Print2DBoolArray(unlockedIcicles2D);
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
    #region "Coin"

    // コインのテキスト
    [SerializeField] private TextMeshProUGUI t_coinText;


    // ゲーム内数値の見た目を初期化する
    private void InitAppearance()
    {
        UpdateCoin();
    }

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

    // 解放したい冷凍庫番号・つらら番号を入力すると、対応したつららを解放する
    public void UnlockIcicle(int freezerIndex, int icicleIndex)
    {
        if (freezerIndex > Const.maxfreezerCount - 1) throw new IndexOutOfRangeException("不正な冷凍庫のIndexを受け取りました");
        if (icicleIndex > Const.maxIcicleTypePerBook - 1) throw new IndexOutOfRangeException("不正なつららのIndexを受け取りました");

        // 一次元配列のまま直接解放
        data.isUnlockedIcicles[freezerIndex * Const.maxIcicleCount + icicleIndex] = true;
        Debug.Log($"freezerIndex: {freezerIndex}, icicleIndex: {icicleIndex}のつららを解放しました");
        Save();
    }

    #endregion
}
