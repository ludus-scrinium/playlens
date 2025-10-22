using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MenuToggle : MonoBehaviour
{
    [Header("Menu Root")]
    public GameObject menuRoot;        // panel that contains the UI
    public CanvasGroup menuGroup;      // on the same object as menuRoot
    public RectTransform menuRect;     // same or child rect

    [Header("Animation")]
    public Vector2 hiddenOffset = new Vector2(0f, -150f);
    [Range(0.05f, 0.4f)] public float duration = 0.15f;

    [Header("Reset on Close (choose ONE pattern)")]
    // Pattern A: three separate Scroll Views
    public GameObject scrollWeaponsGO;
    public GameObject scrollPotionsGO;
    public GameObject scrollMiscGO;
    public ScrollRect scrollWeapons;
    public ScrollRect scrollPotions;
    public ScrollRect scrollMisc;

    // Pattern B: one Scroll View with three panels
    public ScrollRect singleScroll;
    public GameObject panelWeapons;
    public GameObject panelPotions;
    public GameObject panelMisc;

    public enum DefaultTab { Weapons, Potions, Misc }
    public DefaultTab defaultTab = DefaultTab.Potions;

    Vector2 _shownPos;
    bool _visible;

    void Awake()
    {
        _shownPos = menuRect.anchoredPosition;

        // start hidden
        menuRoot.SetActive(false);
        menuGroup.alpha = 0f;
        menuRect.anchoredPosition = _shownPos + hiddenOffset;
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.iKey.wasPressedThisFrame)
            Toggle();
    }

    public void Toggle()
    {
        StopAllCoroutines();
        _visible = !_visible;
        if (_visible) menuRoot.SetActive(true);
        StartCoroutine(SlideFade(_visible));
    }

    System.Collections.IEnumerator SlideFade(bool show)
    {
        float t = 0f;
        float startA = menuGroup.alpha;
        float endA   = show ? 1f : 0f;

        Vector2 startP = menuRect.anchoredPosition;
        Vector2 endP   = show ? _shownPos : _shownPos + hiddenOffset;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float k = t / duration;
            k = k * k * (3 - 2 * k); // smoothstep
            menuGroup.alpha = Mathf.Lerp(startA, endA, k);
            menuRect.anchoredPosition = Vector2.LerpUnclamped(startP, endP, k);
            yield return null;
        }

        // finalize state
        menuGroup.alpha = endA;
        menuRect.anchoredPosition = endP;

        if (!show)
        {
            // Reset elements RIGHT BEFORE hiding so layout is alive
            ResetUIImmediately();
            // clear selection to avoid sticky highlights on next open
            if (EventSystem.current) EventSystem.current.SetSelectedGameObject(null);

            // now hide
            menuRoot.SetActive(false);
        }
    }

    void ResetUIImmediately()
    {
        // Pattern A: three Scroll Views
        if (scrollWeaponsGO || scrollPotionsGO || scrollMiscGO)
        {
            bool w = defaultTab == DefaultTab.Weapons;
            bool p = defaultTab == DefaultTab.Potions;
            bool m = defaultTab == DefaultTab.Misc;

            SetActiveSafe(scrollWeaponsGO, w);
            SetActiveSafe(scrollPotionsGO, p);
            SetActiveSafe(scrollMiscGO,    m);

            ResetScroll(scrollWeapons);
            ResetScroll(scrollPotions);
            ResetScroll(scrollMisc);
        }

        // Pattern B: one Scroll View with three panels
        if (singleScroll)
        {
            bool w = defaultTab == DefaultTab.Weapons;
            bool p = defaultTab == DefaultTab.Potions;
            bool m = defaultTab == DefaultTab.Misc;

            SetActiveSafe(panelWeapons, w);
            SetActiveSafe(panelPotions, p);
            SetActiveSafe(panelMisc,    m);

            ResetScroll(singleScroll);
        }
    }

    void SetActiveSafe(GameObject go, bool on)
    {
        if (!go) return;
        // Only toggle if needed to avoid redundant layout rebuilds
        if (go.activeSelf != on) go.SetActive(on);
    }

    void ResetScroll(ScrollRect sr)
    {
        if (sr == null) return;

        // Force layout so content size is correct before resetting positions
        Canvas.ForceUpdateCanvases();

        // For vertical lists: 1 = top, 0 = bottom
        if (sr.vertical)
            sr.verticalNormalizedPosition = 1f;

        if (sr.horizontal)
            sr.horizontalNormalizedPosition = 0f;

        if (sr.content)
            sr.content.anchoredPosition = Vector2.zero;

        // Optional: clear inertia so it doesn't "slide" on next open
        sr.velocity = Vector2.zero;
    }
}
