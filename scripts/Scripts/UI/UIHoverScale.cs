using UnityEngine;
using UnityEngine.EventSystems;

public class UIHoverScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Range(1f, 1.2f)] public float hoverScale = 1.05f;
    [Range(0.01f, 0.5f)] public float duration = 0.1f;

    Vector3 _base;
    Coroutine _tween;

    void Awake() => _base = transform.localScale;

    public void OnPointerEnter(PointerEventData _) => StartTween(_base * hoverScale);
    public void OnPointerExit(PointerEventData _)  => StartTween(_base);

    void StartTween(Vector3 target)
    {
        if (_tween != null) StopCoroutine(_tween);
        _tween = StartCoroutine(TweenScale(target, duration));
    }

    System.Collections.IEnumerator TweenScale(Vector3 target, float t)
    {
        Vector3 start = transform.localScale;
        float time = 0f;
        while (time < t)
        {
            time += Time.unscaledDeltaTime;
            float k = time / t;
            // smoothstep for nicer easing
            k = k * k * (3f - 2f * k);
            transform.localScale = Vector3.LerpUnclamped(start, target, k);
            yield return null;
        }
        transform.localScale = target;
        _tween = null;
    }
}

