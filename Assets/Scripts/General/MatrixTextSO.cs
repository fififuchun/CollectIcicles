using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "MatrixText", menuName = "Create MatrixTextSO"), System.Serializable]
public class MatrixTextSO : ScriptableObject
{
    public MatrixTextData[] stringGroups;
}

//表示用のクラス
[System.Serializable]
public class MatrixTextData
{
    [TextArea(3, 10)] public string[] strings;
}

#if UNITY_EDITOR
[CanEditMultipleObjects]
[CustomEditor(typeof(MatrixTextSO), true)]
public class MatrixTextSOEditor : Editor
{
    public MatrixTextSO matrixTextSO;

    private void OnEnable()
    {
        matrixTextSO = target as MatrixTextSO;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();//ボタンを表示

        if (GUILayout.Button("Load"))
        {
            Debug.Log($"{MatrixText.filepath}をロードします");

            Load(MatrixText.filepath);
        }

        if (GUILayout.Button("Save"))
        {
            Debug.Log($"{MatrixText.filepath}にセーブします");

            Save(matrixTextSO);
        }
    }

    // jsonとしてデータを保存
    public void Save(MatrixTextSO data)
    {
        // jsonとして変換
        string json = JsonUtility.ToJson(data, true);

        // ファイル書き込み指定
        StreamWriter wr = new StreamWriter(MatrixText.filepath, false);

        // json変換した情報を書き込み
        wr.WriteLine(json);

        // ファイル閉じる
        wr.Close();
    }

    // pathのjsonを読み込んでmatrixTextに反映する
    public void Load(string path)
    {
        // ファイル読み込み指定
        StreamReader rd = new StreamReader(path);

        // ファイル内容全て読み込む
        string json = rd.ReadToEnd();
        Debug.Log(json);

        // ファイル閉じる
        rd.Close();

        // JSONをオブジェクトに適用する
        JsonUtility.FromJsonOverwrite(json, matrixTextSO);
    }
}
#endif