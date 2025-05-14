using System;
using System.Collections.Generic;
using System.Linq;
using FuchunLibrary;

[System.Serializable]
public class SaveData
{
    #region "General"

    // つららコイン
    public int t_coin;
    public int T_coin() { return t_coin; }

    public int freezerNum;

    #endregion


    #region "RPG"

    // つららの解放状況
    public bool[] isUnlockedIcicles = new bool[Const.maxfreezerCount * Const.maxIcicleTypePerBook];

    #endregion


    #region "Mission"

    #endregion


    #region "Tutorial"

    #endregion


    // ミッションのIDと関数の対応
    public Dictionary<string, Action> missionActions;

    public SaveData()
    {
        missionActions = new Dictionary<string, Action>()
        {
            {"00", () => T_coin() },
        };
    }
}
