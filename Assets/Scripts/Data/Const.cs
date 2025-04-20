using System.Collections.Generic;

public static class Const
{
    public static int maxIcicleCount = 20; // つららの生える最大数
    public static int maxGrowGrade = 3; // つららの最大成長段階

    // public static int maxIcicleType = 20; // つららの最高出現数


    // 通常の冷凍庫に出現するつららのインデックス
    public static int[] normalFreezerIndex = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 };
    // 冷凍庫に出現する固有のつららの種類
    public static int[][] freezerIndex = new int[][] { normalFreezerIndex };

    // 普通の冷凍庫に出現するつららの座標
    public static int[,] normalMap = new int[20, 2]{
        {1, -2},
        {1, -1},
        {0, 0},
        {0, 1},
        {0, 2},
        {0, 3},
        {1, 3},
        {2, 3},
        {0, 4},
        {1, 4},
        {2, 4},
        {0, 5},
        {1, 5},
        {2, 5},
        {1, 6},
        {2, 6},
        {1, 7},
        {2, 7},
        {1, 8},
        {2, 8},
    };
    // 冷凍庫の種類
    public static int[][,] map = new int[][,] { normalMap };
}
