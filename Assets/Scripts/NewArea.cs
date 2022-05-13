using DG.Tweening;
using TMPro;
using UnityEngine;

public class NewArea : MonoBehaviour
{
    [SerializeField] private RectTransform letterbox;
    [SerializeField] private CanvasGroup areaName;

    public void Enter()
    {
        DOVirtual.Float(0, 60, 1f, val =>
        {
            var offMax = letterbox.offsetMax;
            var offMin = letterbox.offsetMin;
            offMax.y = -val;
            offMin.y = val;
            letterbox.offsetMax = offMax;
            letterbox.offsetMin = offMin;
            letterbox.anchoredPosition = Vector2.zero;
        });
        areaName.DOFade(1, 1.5f);
    }

    public void Exit()
    {
        DOVirtual.Float(60, 0, 1f, val =>
        {
            var offMax = letterbox.offsetMax;
            var offMin = letterbox.offsetMin;
            offMax.y = -val;
            offMin.y = val;
            letterbox.offsetMax = offMax;
            letterbox.offsetMin = offMin;
            letterbox.anchoredPosition = Vector2.zero;
        });
        areaName.DOFade(0, 1.5f);
    }
}