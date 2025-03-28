using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways]
public class Mission : MonoBehaviour
{
    [SerializeField] private MissionSO missionSO;
    [SerializeField] private DataSaver dataSaver;

    [SerializeField] internal List<UnityEventWrapper> missions = new List<UnityEventWrapper>();

    void OnEnable()
    {
        missionSO.onMissionChanged.AddListener(OnMissionChanged);
        Debug.Log("Attach");
    }

    void OnMissionChanged()
    {
        missions.Clear();

        for (int i = 0; i < missionSO.missionGroupDatas.Count; i++)
        {
            if (missionSO.missionGroupDatas[i].missionType == MissionType.Through)
            {
                missions.Add(new UnityEventWrapper());
                missions[missions.Count - 1].id = missionSO.missionGroupDatas[i].headId;
                // missions[missions.Count - 1].unityEvent = new UnityEvent();
            }
            else if (missionSO.missionGroupDatas[i].missionType == MissionType.Separate)
            {
                for (int j = 0; j < missionSO.missionGroupDatas[i].missionDatas.Count; j++)
                {
                    missions.Add(new UnityEventWrapper());
                    missions[missions.Count - 1].id = missionSO.missionGroupDatas[i].missionDatas[j].id;
                    // missions[missions.Count - 1].unityEvent = new UnityEvent();
                }
            }
        }

        Debug.Log("OnMissionChanged");
    }
}

[System.Serializable]
public class UnityEventWrapper
{
    [SerializeField, HideInInspector] internal string id = "----";
    [SerializeField] internal UnityEvent unityEvent;
}