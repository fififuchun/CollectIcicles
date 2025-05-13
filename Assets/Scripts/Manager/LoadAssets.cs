using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;

public class LoadAssets : MonoBehaviour
{
    // ロード終了を通知
    private static UniTaskCompletionSource<bool> loadCompletionSource = new();
    public static UniTask WaitUntilLoadedAsync() => loadCompletionSource.Task;

    [SerializeField] private Image image;

    async void Start()
    {
        Debug.Log($"Start: {Time.time}");
        image.color = Color.white;

        // NotFoundのSprite読み込み
        Sprite sprite = await Addressables.LoadAssetAsync<Sprite>("Assets/Images/Icicles/NotFound.png");
        image.sprite = Instantiate(sprite);

        Debug.Log($"SO_Length: {Const.icicleSO_Array.Length}");

        // IcicleSOの読み込み
        for (int i = 0; i < Const.icicleBookCodes.Length; i++)
        {
            int index = i;
            IcicleSO so = await Addressables.LoadAssetAsync<IcicleSO>($"Assets/Scripts/SO/IcicleSO_{index}.asset");
            Const.icicleSO_Array[index] = Instantiate(so);
        }

        // ロード完了後の処理
        Debug.Log(Const.icicleSO_Array[0].icicles.Length);
        Debug.Log($"Completed Load: {Time.time}");

        loadCompletionSource.TrySetResult(true);
    }
}
