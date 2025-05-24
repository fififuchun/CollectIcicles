using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnSnowEffect : MonoBehaviour
{
    // シングルトン（複製防止）
    private static SpawnSnowEffect instance;

    // 
    private ParticleSystem onTapSnowEffect;
    private RectTransform canvas;

    // タップ時のprefab
    [SerializeField] private GameObject onTapSnowPrefab;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded; // イベント登録
        }
        else Destroy(gameObject);
    }

    void OnDestroy() { if (instance == this) SceneManager.sceneLoaded -= OnSceneLoaded; }

    // シーンがロードされたときに呼ばれる
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (canvas == null) canvas = GameObject.Find("Canvas")?.GetComponent<RectTransform>();

        onTapSnowEffect = Instantiate(onTapSnowPrefab).GetComponent<ParticleSystem>();
        onTapSnowEffect.transform.SetParent(canvas, false);
        Debug.Log("Scene Loaded: Snow effect ready");
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (onTapSnowEffect == null || canvas == null) return;

            Vector3 spawnPosition = Input.mousePosition;
            Vector3 localPos = spawnPosition - (Vector3)canvas.sizeDelta / 2;

            onTapSnowEffect.transform.localPosition = localPos;
            onTapSnowEffect.Play();
        }
    }
}
