using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IcicleData", menuName = "Create IcicleData")]
public class IcicleData : ScriptableObject
{
    public List<Icicles> icicles;

    // インスペクター上で変更があったときに自動更新
    private void OnValidate()
    {
        for (int i = 0; i < icicles.Count; i++) if (icicles[i] != null) icicles[i].index = i; // インデックスをリストの位置と同期
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
