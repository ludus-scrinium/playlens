using UnityEngine;

public class TabSwitcher : MonoBehaviour
{
    public GameObject scrollWeapons;
    public GameObject scrollPotions;
    public GameObject scrollMisc;

    public void ShowWeapons()
    {
        SetActive(scrollWeapons, true);
        SetActive(scrollPotions, false);
        SetActive(scrollMisc, false);
    }

    public void ShowPotions()
    {
        SetActive(scrollWeapons, false);
        SetActive(scrollPotions, true);
        SetActive(scrollMisc, false);
    }

    public void ShowMisc()
    {
        SetActive(scrollWeapons, false);
        SetActive(scrollPotions, false);
        SetActive(scrollMisc, true);
    }

    private void SetActive(GameObject go, bool value)
    {
        if (go != null) go.SetActive(value);
    }
}
