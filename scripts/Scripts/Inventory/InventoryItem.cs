using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] private string itemName = "Potion";

    // Called from the Button's OnClick
    public void Equip()
    {
        Debug.Log($"Equipped: {itemName}");
    }
}
