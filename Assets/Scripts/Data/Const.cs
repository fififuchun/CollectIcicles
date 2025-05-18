using System.Collections.Generic;
using FuchunLibrary;

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


    #region "delete finally"

    // 通常の冷凍庫に出現するつららのインデックス
    // public static int[] normalFreezerIndex = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 };
    // 冷凍庫に出現する固有のつららの種類
    // public static int[][] freezerIndex = new int[][] { normalFreezerIndex };

    // 普通の冷凍庫に出現するつららの座標
    // public static int[,] normalMap = new int[20, 2]{
    //     {1, -2},
    //     {1, -1},
    //     {0, 0},
    //     {0, 1},
    //     {0, 2},
    //     {0, 3},
    //     {1, 3},
    //     {2, 3},
    //     {0, 4},
    //     {1, 4},
    //     {2, 4},
    //     {0, 5},
    //     {1, 5},
    //     {2, 5},
    //     {1, 6},
    //     {2, 6},
    //     {1, 7},
    //     {2, 7},
    //     {1, 8},
    //     {2, 8},
    // };
    // つららの座標を冷凍庫ごとに
    // public static int[][,] map = new int[][,] { normalMap };

    #endregion
}
