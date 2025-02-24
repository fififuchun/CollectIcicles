using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IcicleData", menuName = "Create IcicleData")]
public class IcicleData : ScriptableObject
{
    public List<Icicles> iciclesList;
}

[System.Serializable]
public class Icicles
{
    public string id; // つららのID
    public string icicleName; // つららの名前
    public int iciclePoint; // つららのポイント
    public int[] requiredUnlock; // つららをアンロックするために必要なナンバー
}
