using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class DataManager : MonoBehaviour
{
    // json変換するデータのクラス
    [HideInInspector] public SaveData data;

    // jsonファイルのパス
    string filepath;

    // jsonファイル名
    string fileName = "Data.json";

    //-------------------------------------------------------------------
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
        if (!File.Exists(filepath)) Save(data);

        // ファイルを読み込んでdataに格納
        data = Load(filepath);

        // データを初期化
        InitData();
    }


    // jsonとしてデータを保存
    public void Save(SaveData data)
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
    SaveData Load(string path)
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
        Save(data);
    }

    //-------------------------------------------------------------------

    [SerializeField] private TextMeshProUGUI t_coinText;



    private void InitData()
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

    //update mission
    // public UnityEvent updateMissions = new UnityEvent();
    // public void ChangeMissionValue(int i, float changedValue)
    // {
    //     data.missionValues[i] = (int)changedValue;
    //     Debug.Log($"missionDataの{i}番目を{changedValue}に変更しました");

    //     Save(data);

    //     updateMissions?.Invoke();
    // }
}
