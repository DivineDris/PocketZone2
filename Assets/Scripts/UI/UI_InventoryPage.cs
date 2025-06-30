using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_InventoryPage : MonoBehaviour
{
    [SerializeField]
    private UI_InventoryItem itemPrefab;
    [SerializeField]
    private RectTransform contentPanel;
    private UI_InventoryItem selectedItem;
    public event Action<int> OnItemDelete; 

    List<UI_InventoryItem> inventoryItemsList = new List<UI_InventoryItem>();

    

    public void InitializeInventoryUI(int inventorySize)
    {
        for (int i = 0; i < inventorySize; i++)
        {
            UI_InventoryItem uiItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
            uiItem.transform.SetParent(contentPanel, false);
            inventoryItemsList.Add(uiItem);
            uiItem.OnItemTapped += HandleItemSelection;
            uiItem.OnDeleteButtonTapped += HandleItemDeleted;
        }
    }

    private void HandleItemSelection(UI_InventoryItem item)
    {
        int index = inventoryItemsList.IndexOf(item);
        if (index == -1)
            return;
        if (selectedItem == item && selectedItem is not null)
            {
                selectedItem.Deselect();
                selectedItem = null;
                return;
            }
        item.Select();
        if (selectedItem != null)
            selectedItem.Deselect();
        selectedItem = item;
    }

    private void HandleItemDeleted(UI_InventoryItem item)
    {
        int index = inventoryItemsList.IndexOf(item);
        if (index == -1)
        {
            return;
        }
        item.Deselect();
        item.ResetData();
        OnItemDelete?.Invoke(index);
    }

    public void UpdateData(int itemIndex, Sprite itemImage, int itemQuantity)
    {
        if (inventoryItemsList.Count > itemIndex)
        {
            inventoryItemsList[itemIndex].SetData(itemImage, itemQuantity);
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
        if (selectedItem != null)
        {
            selectedItem.Deselect();
            selectedItem = null;
        }

    }

    internal void ResetAllItems()
    {
        foreach (var item in inventoryItemsList)
        {
            item.ResetData();
            item.Deselect();
        }
    }
}
