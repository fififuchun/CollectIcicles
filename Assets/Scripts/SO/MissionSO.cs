using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

public enum MissionType
{
    Through,
    Separate,
}

public enum MissionState
{
    None,
    Achieved,
    NotAchieved,
    Received,
}

[CreateAssetMenu(fileName = "MissionSO", menuName = "Create MissionSO")]
public class MissionSO : ScriptableObject
{
    // このスクリプタブルオブジェクトにIDを課した時に通知  // が変更された場合に通知
    [HideInInspector] public UnityEvent onMissionChanged = new UnityEvent();

    public List<MissionGroupData> missionGroupDatas = new List<MissionGroupData>();

    public void AssignIDs()
    {
        for (int i = 0; i < missionGroupDatas.Count; i++)
        {
            missionGroupDatas[i].headId = $"{i:D2}";

            for (int j = 0; j < missionGroupDatas[i].missionDatas.Count; j++)
            {
                missionGroupDatas[i].missionDatas[j].bottomId = $"{j:D2}";
                missionGroupDatas[i].missionDatas[j].id = $"{missionGroupDatas[i].headId}{missionGroupDatas[i].missionDatas[j].bottomId}";
            }
        }

        // 通知
        onMissionChanged?.Invoke();
    }
}

[System.Serializable]
public class MissionGroupData
{
    [HideInInspector] public string headId; // 上2ケタのID

    // ミッションのタイプ: Through, Separate
    public MissionType missionType;

    public List<MissionData> missionDatas = new List<MissionData>();
}

[System.Serializable]
public class MissionData
{
    [HideInInspector] public string bottomId; // 下2ケタのID
    public string id; // 4ケタのID

    public int current;
    public int goal;

    public MissionState missionState;

    //関数
    //初期化
    public void InitializeMissionState()
    {
        if (missionState == MissionState.None) missionState = MissionState.NotAchieved;
    }

    //ミッションを達成したとき
    public void JudgeAchieveMissionState()
    {
        if (goal < current) missionState = MissionState.Achieved;
    }

    //ミッション報酬を受け取ったとき
    public void ReceiveMissionState()
    {
        missionState = MissionState.Received;
    }

    //IDからクラスの位置を特定
    public Vector2Int MissionDataIndex(MissionData missionData)
    {
        if (missionData.id.ToCharArray().Length == 4) return new Vector2Int(Mathf.FloorToInt(int.Parse(missionData.id) / 100), int.Parse(missionData.id) % 100);
        else return new Vector2Int(0, 0);
    }
}


#if UNITY_EDITOR
[CanEditMultipleObjects]
[CustomEditor(typeof(MissionSO), true)]
public class MissionSOEditor : Editor
{
    public MissionSO missionSO;

    private void OnEnable()
    {
        missionSO = target as MissionSO;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Assign IDs"))
        {
            missionSO.AssignIDs();
            EditorUtility.SetDirty(missionSO);
        }
    }
}
#endif