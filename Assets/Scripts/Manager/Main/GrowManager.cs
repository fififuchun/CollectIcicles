using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class GrowManager : MonoBehaviour
{
    [SerializeField] private IcicleManager icicleManager;

    // デバッグ用
    [SerializeField] private bool canInfiniteGrow = true;

    CancellationTokenSource cts = new CancellationTokenSource();
    // CancellationToken ct;

    async void OnEnable()
    {
        // GameObjectが破棄された時にキャンセルを飛ばすトークン
        // cts.Token = this.GetCancellationTokenOnDestroy();

        // つららを育て続ける
        await InfiniteGrow(cts.Token);
    }

    void OnDestroy()
    {
        cts?.Cancel();
        cts?.Dispose();
    }

    async UniTask InfiniteGrow(CancellationToken ct)
    {
        try
        {
            while (canInfiniteGrow)
            {
                await UniTask.Delay(100, cancellationToken: ct);
                icicleManager.GrowIcicle();
                // Debug.Log("Grow icicle in Unitask Loop");
            }
        }
        catch (OperationCanceledException)
        {
            // キャンセルされたときの処理（無視してOK）
            Debug.Log("InfiniteGrow was cancelled.");
        }
    }
}
