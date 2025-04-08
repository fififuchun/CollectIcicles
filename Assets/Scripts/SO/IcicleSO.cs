using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
// using Shapes2D;
using UnityEngine.UI;



#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "IcicleSO", menuName = "Create IcicleSO")]
public class IcicleSO : ScriptableObject
{
    public Icicles[] icicles;

    // インスペクター上で変更があったときに自動更新
    private void OnValidate()
    {
        // インデックスをリストの位置と同期
        for (int i = 0; i < icicles.Length; i++) if (icicles[i] != null)
            {
                icicles[i].index = i;
                icicles[i].id = i.ToString("D3");
            }
    }
}

[System.Serializable]
public class Icicles
{
    public string icicleName; // つららの名前
    public int index; // つららのindex
    public string id; // つららのID
    public int iciclePoint; // つららのポイント
    public int rareGrade; // つららのレア度

    public float scale_x; // つららのスケール
    public float scale_y; // つららのスケール
    public int eyeId; // つららの目のID
    public int eye_y; // つららの目の位置

    public Sprite image; // つららの画像

    public int[] requiredUnlock; // つららをアンロックするために必要なナンバー
}


// SOとJsonの変換を行うクラス
public class ConvertIcicleSO : MonoBehaviour
{
    // [SerializeField] internal string m_dataPath = "SampleData.json";
    // [SerializeField] IcicleSO icicleSO;

    public static string GetFullPath(string _dataPath)
    {
#if !UNITY_EDITOR
        string appPath = Application.persistentDataPath;
#else
        string appPath = Application.dataPath;
#endif
        return $"{appPath}/{_dataPath}";
    }

    public static bool SaveToJSON(string _dataPath, IcicleSO _dataSO)
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

    string m_dataPath = "IcicleData.json";

    private void OnEnable()
    {
        icicleSO = target as IcicleSO;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Save SO in Json"))
        {
            ConvertIcicleSO.SaveToJSON(m_dataPath, icicleSO);
            Debug.Log("Save SO in Json");
        }

        if (GUILayout.Button("Load Json"))
        {
            StreamReader rd = new StreamReader(Application.dataPath + "/" + m_dataPath);
            string json = rd.ReadToEnd();
            rd.Close();
            JsonUtility.FromJsonOverwrite(json, icicleSO);

            Debug.Log($"Load Json");
        }
    }
}
#endif