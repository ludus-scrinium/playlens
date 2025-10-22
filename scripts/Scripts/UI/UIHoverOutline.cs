using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIHoverOutline : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Outline _outline;

    void Awake()
    {
        _outline = GetComponent<Outline>();
        if (_outline) _outline.enabled = false;
    }

    public void OnPointerEnter(PointerEventData _)
    {
        if (_outline) _outline.enabled = true;
    }

    public void OnPointerExit(PointerEventData _)
    {
        if (_outline) _outline.enabled = false;
    }
}

