using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventaryController : MonoBehaviour
{
    [SerializeField]
    private UI_InventoryPage inventoryUI;

    [SerializeField]
    private InventorySO inventoryData;

    public List<InventoryItem> initialItems = new List<InventoryItem>();


    private void Start()
    {
        inventoryUI.InitializeInventoryUI(inventoryData.Size);
        inventoryData.Initialize();
        inventoryData.OnInventoryUpdated += UpdateInventoryUI;
        foreach (InventoryItem item in initialItems)
        {
            if (item.IsEmpty)
                continue;
            inventoryData.AddItem(item);
        }
    }

    private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
    {
        inventoryUI.ResetAllItems();
        foreach (var item in inventoryState)
        {
            inventoryUI.UpdateData(item.Key,
                                item.Value.Item.ItemImage,
                                item.Value.Quantity);
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventoryUI.isActiveAndEnabled == false)
            {
                inventoryUI.Show();
                foreach (var item in inventoryData.GetCurrentInventoryState())
                {
                    inventoryUI.UpdateData(item.Key,
                                            item.Value.Item.ItemImage,
                                            item.Value.Quantity);
                }
            }
            else
            {
                inventoryUI.Hide();
            }
        }
    }
}
