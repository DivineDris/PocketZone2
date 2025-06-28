using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class InventorySO : ScriptableObject
{
    [SerializeField]
    private List<InventoryItem> inventoryItems;
    [field: SerializeField]
    public int Size { get; private set; } = 16;

    public event Action<Dictionary<int, InventoryItem>> OnInventoryUpdated;

    public void Initialize()
    {
        inventoryItems = new List<InventoryItem>();
        for (int i = 0; i < Size; i++)
        {
            inventoryItems.Add(InventoryItem.GetEmptyItem());
        }
    }

    public int AddItem(ItemSO item, int quantity)
    {
        if (!item.IsStackable)
        {
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                while (quantity > 0 && !IsInventoryFull())
                {
                    quantity -= AddItemToFirstFreeSlot(item, 1);
                }
                InformAboutChange();
                return quantity;
            }
        }
        quantity = AddStackableItem(item, quantity);
        InformAboutChange();
        return quantity;
    }

    public InventoryItem GetItemAt(int index)
    {
        return inventoryItems.ElementAt(index);
    }

    private int AddItemToFirstFreeSlot(ItemSO item, int quantity)
    {
        InventoryItem newItem = new InventoryItem
        {
            Item = item,
            Quantity = quantity
        };

        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].IsEmpty)
            {
                inventoryItems[i] = newItem;
                return quantity;
            }
        }
        return 0;
    }

    private bool IsInventoryFull()
    => inventoryItems.Where(item => item.IsEmpty).Any() == false;


    private int AddStackableItem(ItemSO item, int quantity)
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].IsEmpty)
                continue;
            if (inventoryItems[i].Item.Id == item.Id)
            {
                int amountPossibleToTake = inventoryItems[i].Item.MaxStackSize - inventoryItems[i].Quantity;
                if (quantity > amountPossibleToTake)
                {
                    inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].Item.MaxStackSize);
                    quantity -= amountPossibleToTake;
                }
                else
                {
                    inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].Quantity + quantity);
                    InformAboutChange();
                    return 0;
                }
            }
        }
        while (quantity > 0 && !IsInventoryFull())
        {
            int newQuantity = Mathf.Clamp(quantity, 0, item.MaxStackSize);
            quantity -= newQuantity;
            AddItemToFirstFreeSlot(item, newQuantity);
        }
        return quantity;

    }

    public Dictionary<int, InventoryItem> GetCurrentInventoryState()
    {
        Dictionary<int, InventoryItem> returnValue = new Dictionary<int, InventoryItem>();
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].IsEmpty)
                continue;
            returnValue[i] = inventoryItems[i];
        }
        return returnValue;
    }

    public void AddItem(InventoryItem item)
    {
        AddItem(item.Item, item.Quantity);
    }

    private void InformAboutChange()
    {
        OnInventoryUpdated?.Invoke(GetCurrentInventoryState());
    }

    public void RemoveItem(int itemIndex, int amount)
    {
        if (inventoryItems.Count > itemIndex)
        {
            if (inventoryItems.Count > itemIndex)
                return;
            int reminder = inventoryItems[itemIndex].Quantity - amount;
            if (reminder <= 0)
                inventoryItems[itemIndex] = InventoryItem.GetEmptyItem();
            else
                inventoryItems[itemIndex] = inventoryItems[itemIndex].ChangeQuantity(reminder);
            InformAboutChange();
        }
    }
}

[Serializable]
public struct InventoryItem
{
    public int Quantity;
    public ItemSO Item;
    public bool IsEmpty => Item == null;

    public InventoryItem ChangeQuantity(int newQuantity)
    {
        return new InventoryItem
        {
            Item = this.Item,
            Quantity = newQuantity
        };

    }

    public static InventoryItem GetEmptyItem()
        => new InventoryItem
        {
            Item = null,
            Quantity = 0
        };
}