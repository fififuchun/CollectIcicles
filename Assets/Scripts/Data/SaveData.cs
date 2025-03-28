using System;
using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    #region "General"

    public int t_coin;
    public int T_coin() { return t_coin; }

    #endregion


    #region "RPG"

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
