using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
// using UnityEngine.Events;

public class CustomButton : MonoBehaviour,
    IPointerClickHandler,
    IPointerDownHandler,
    IPointerUpHandler
{
    //呼び出したい関数
    public System.Action onClickCallback;

    //アニメーション完了時に呼び出したい関数
    // public TweenCallback onComplete;

    //CustomButtonと同階層にあるCanvasGroup
    [SerializeField] private CanvasGroup _canvasGroup;

    public void OnPointerClick(PointerEventData eventData)
    {
        onClickCallback?.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.DOScale(0.95f, 0.24f).SetEase(Ease.OutCubic);
        _canvasGroup.DOFade(0.8f, 0.24f).SetEase(Ease.OutCubic);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        float animationTime = 0.24f;
        transform.DOScale(1f, animationTime).SetEase(Ease.OutCubic);
        _canvasGroup.DOFade(1f, animationTime).SetEase(Ease.OutCubic);

        // if (onComplete == null) return;
        // else DOVirtual.DelayedCall(animationTime, () => onComplete?.Invoke(), false);
    }
}
