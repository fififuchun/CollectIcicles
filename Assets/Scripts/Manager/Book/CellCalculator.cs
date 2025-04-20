using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// どの冷凍庫かを入力すると、その冷凍庫に出現するつららの図鑑における座標を計算する
public class CellCalculator : MonoBehaviour
{
    [SerializeField] private IcicleSO icicleSO;

    void Start()
    {
        // row: つららの解放列
        // foreach (var row in LastIndex().Select((value, index) => new { value, index }))
        // {
        //     // item: つららの解放順のインデックス
        //     foreach (var item in IcicleIndexRow(row.value).Select((value, index) => new { value, index }))
        //     {
        //         Debug.Log(item.value + " : " + row.index + ", " + item.index);
        //     }
        // }

        // Debug.Log("RequireMultiIndex: " + string.Join(",", RequireMultiIndex()));
        // foreach (int i in RequireMultiIndex())
        // {
        //     Debug.Log(string.Join(",", icicleSO.icicles[i].requiredUnlock));
        // }
    }

    // これ以上解放されない、1番最後のつららのインデックスを返す
    public List<int> LastIndex(/*int freezerNum*/)
    {
        HashSet<int> releasedIndex = new HashSet<int>();
        for (int i = 0; i < Const.maxIcicleCount; i++)
        {
            foreach (int unlock in icicleSO.icicles[Const.normalFreezerIndex[i]].requiredUnlock)
            {
                releasedIndex.Add(unlock);
            }
        }

        return Const.normalFreezerIndex.ToList().Except(releasedIndex).ToList();
    }

    // LastIndexを元に「つらら」までの列を作る
    public List<int> IcicleIndexRow(int lastIndex)
    {
        List<int> icicleList = new List<int>();

        int currentIndex = lastIndex;
        while (currentIndex != 2)
        {
            icicleList.Add(currentIndex);
            currentIndex = icicleSO.icicles[currentIndex].requiredUnlock[0];
        }
        // icicleList.Add(currentIndex);
        icicleList.Reverse();

        return icicleList;
    }

    // つららを解放する際に、2種類以上のつららの収穫が必要なつららのインデックスを返す
    public List<int> RequireMultiIndex()
    {
        List<int> icicleList = new List<int>();

        // while (currentIndex != 2)
        // {
        //     icicleList.Add(currentIndex);
        //     currentIndex = icicleSO.icicles[currentIndex].requiredUnlock[0];
        // }
        // icicleList.Add(currentIndex);
        // icicleList.Reverse();

        foreach (int i in Const.normalFreezerIndex)
        {
            if (icicleSO.icicles[i].requiredUnlock.Length > 1)
            {
                icicleList.Add(i);
            }
        }

        return icicleList;
    }

    // public 
}
