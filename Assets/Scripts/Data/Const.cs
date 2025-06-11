using System.Collections.Generic;
using FuchunLibrary;
using UnityEngine;

public static class Const
{
    /// <summary>
    /// 一回でつららを収穫できる最大数
    /// </summary>
    public static int maxIcicleCount = 20;

    /// <summary>
    /// つららの最大成長段階
    /// </summary>
    public static int maxGrowGrade = 3;

    /// <summary>
    /// 一つの冷凍庫におけるつららの最高出現数
    /// </summary>
    public static int maxIcicleTypePerBook = 20;

    /// <summary>
    /// 冷凍庫番号
    /// </summary>
    public static int freezerNum = 0;

    /// <summary>
    /// つらら図鑑の識別用文字列ID
    /// </summary>
    public static string[] icicleBookCodes = { "NRM", "PLT" };

    /// <summary>
    /// 冷凍庫の種類の最大数
    /// </summary>
    public static int maxfreezerCount = icicleBookCodes.Length;

    /// <summary>
    /// IcicleSOをLoadAssetで初期化
    /// </summary>
    public static IcicleSO[] icicleSO_Array = new IcicleSO[icicleBookCodes.Length];
}
