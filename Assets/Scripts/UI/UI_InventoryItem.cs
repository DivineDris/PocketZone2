using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class UI_InventoryItem : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Image itemImage;
    [SerializeField]
    private UnityEngine.UI.Image quantityImage;
    [SerializeField]
    private TMP_Text quantityTxt;
    [SerializeField]
    private UnityEngine.UI.Button deleteButton;
    public event Action<UI_InventoryItem> OnItemTapped, OnDeleteButtonTaped;

    private bool empty = true;

    public void Awake()
    {
        ResetData();
        Deselect();
    }
    public void Select()
    {
        deleteButton.interactable = true;
        deleteButton.gameObject.SetActive(true);
    }
    public void Deselect()
    {
        deleteButton.interactable = false;
        deleteButton.gameObject.SetActive(false);
    }
    public void ResetData()
    {
        this.itemImage.gameObject.SetActive(false);
        empty = true;
    }
    public void SetData(Sprite sprite, int quantity)
    {
        this.itemImage.gameObject.SetActive(true);
        this.itemImage.sprite = sprite;
        if (quantity > 1)
        {
            this.quantityImage.gameObject.SetActive(true);
            this.quantityTxt.text = quantity + "";
        }
        else
            this.quantityImage.gameObject.SetActive(false);
        empty = false;
    }
    public void OnPointerClicked(BaseEventData data)
    {
        if (!empty)
            OnItemTapped?.Invoke(this);
    }

    public void OnDeleteButtonClicked(BaseEventData data)
    {
            if (!empty)
            OnDeleteButtonTaped?.Invoke(this);
    }

}
