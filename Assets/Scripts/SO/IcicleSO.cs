using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "IcicleSO", menuName = "Create IcicleSO")]
public class IcicleSO : ScriptableObject
{
    public List<Icicles> icicles;

    // インスペクター上で変更があったときに自動更新
    private void OnValidate()
    {
        // インデックスをリストの位置と同期
        for (int i = 0; i < icicles.Count; i++) if (icicles[i] != null)
            {
                icicles[i].index = i;
                icicles[i].id = i.ToString("D3");
            }
    }
}

[System.Serializable]
public class Icicles
{
    public int index; // つららのindex
    public string id; // つららのID
    public string icicleName; // つららの名前
    public int iciclePoint; // つららのポイント
    public int rareGrade; // つららのレア度

    public int eyeId; // つららの目のID
    public int eye_y; // つららの目の位置
    public int[] requiredUnlock; // つららをアンロックするために必要なナンバー
}

#if UNITY_EDITOR
[CanEditMultipleObjects]
[CustomEditor(typeof(IcicleSO), true)]
public class IcicleSOEditor : Editor
{
    public IcicleSO icicleSO;

    private void OnEnable()
    {
        icicleSO = target as IcicleSO;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
#endif