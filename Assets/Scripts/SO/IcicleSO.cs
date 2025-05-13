using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "IcicleSO", menuName = "Create IcicleSO")]
public class IcicleSO : ScriptableObject
{
    public int freezerIndex;
    [ReadOnly] public string code;

    public Icicles[] icicles = new Icicles[20];

    // インスペクター上で変更があったときに自動更新
    private void OnValidate()
    {
        // 冷凍庫コードを出力
        code = Const.icicleBookCodes[freezerIndex];

        // インデックスをリストの位置と同期
        for (int i = 0; i < icicles.Length; i++)
        {
            icicles[i].index = i;
            // icicles[i].id = i + 1;
            // icicles[i].book_x = Const.normalMap[i, 0];
            // icicles[i].book_y = Const.normalMap[i, 1];
        }

        if (icicles == null || icicles.Length != Const.maxIcicleTypePerBook)
        {
            Icicles[] newArray = new Icicles[Const.maxIcicleTypePerBook];
            for (int i = 0; i < Mathf.Min(Const.maxIcicleTypePerBook, icicles?.Length ?? 0); i++)
            {
                newArray[i] = icicles[i];
            }
            icicles = newArray;
        }
    }
}

[System.Serializable]
public class Icicles
{
    [Header("基本情報")]
    public string icicleName; // つららの名前
    public int index; // つららのindex
    [ReadOnly] public int id; // つららのID
    public int iciclePoint; // つららのポイント
    public int rareGrade; // つららのレア度

    [Header("つらら生成時に必要な情報")]
    public float scale_x; // つららのスケール
    public float scale_y; // つららのスケール
    public int eyeId; // つららの目のID
    public int eye_y; // つららの目の位置

    [Header("図鑑生成時に必要な情報")]
    public int book_x; // つらら図鑑のX座標
    public int book_y; // つらら図鑑のY座標

    [Header("その他情報")]
    public Sprite image; // つららの画像

    public int[] requiredUnlock; // つららをアンロックするために必要なナンバー
}


// SOとJsonの変換を行うクラス
public class ConvertIcicleSO : MonoBehaviour
{
    public static string GetFullPath(string _dataPath)
    {
#if !UNITY_EDITOR
        string appPath = Application.persistentDataPath;
#else
        string appPath = Application.dataPath;
#endif
        return $"{appPath}/Datas/{_dataPath}";
    }

    public static bool SaveToJson(string _dataPath, IcicleSO _dataSO)
    {
        bool result = false;
        string fullPath = GetFullPath(_dataPath);

        // ファイルにデータを書き込む
        string jsonStr = JsonUtility.ToJson(_dataSO, true);
        // Debug.Log("SaveSettings: " + fullPath);

        try
        {
            System.IO.File.WriteAllText(fullPath, jsonStr);
            result = true;
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to save settings data: " + e.Message);
        }
        return result;
    }
}


// これを入れることで、ウィンドウが見切れる問題・セーブロードの問題が解決される
#if UNITY_EDITOR
[CanEditMultipleObjects]
[CustomEditor(typeof(IcicleSO), true)]
public class IcicleSOEditor : Editor
{
    public IcicleSO icicleSO;
    public int freezerIndex;

    // string m_dataPath = $"IcicleBook_{freezerIndex}.json";

    private void OnEnable()
    {
        icicleSO = target as IcicleSO;
        freezerIndex = icicleSO.freezerIndex;

        // Debug.Log(freezerIndex);
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Save SO in Json"))
        {
            ConvertIcicleSO.SaveToJson($"IcicleBook_{freezerIndex}.json", icicleSO);
            Debug.Log("Save SO in Json");
        }

        if (GUILayout.Button("Load Json"))
        {
            StreamReader rd = new StreamReader(ConvertIcicleSO.GetFullPath($"IcicleBook_{freezerIndex}.json"));
            string json = rd.ReadToEnd();
            rd.Close();
            JsonUtility.FromJsonOverwrite(json, icicleSO);

            Debug.Log($"Load Json");
        }
    }
}
#endif