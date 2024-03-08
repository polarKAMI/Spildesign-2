using Inventory.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_InventoryPage : MonoBehaviour
{
    [SerializeField]
    private UI_InventoryItem itemPrefab;

    [SerializeField]
    private RectTransform contentPanel;

    [SerializeField]
    private UIInventoryDescription itemDescription;

    List<UI_InventoryItem> listofUIItems = new List<UI_InventoryItem>();

    public event Action<int> OnDescriptionRequested,
        OnItemActionRequested;

    private void Awake()
    {
        Hide();
        itemDescription.ResetDescription();
    }
    public void InitializeInventoryUI(int inventorysize)
    {
        for (int i = 0; i < inventorysize; i++)
        {
            UI_InventoryItem uiItem =
                Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
            uiItem.transform.SetParent(contentPanel);
            listofUIItems.Add(uiItem);
            uiItem.OnItemClick += HandleItemSelection;
            uiItem.OnItemBeginDrag += HandleBeginDrag;
            uiItem.OnItemDroppedOn += HandleSwap;
            uiItem.OnItemEndDrag += HandleEndDrag;
            uiItem.OnRightMouseBtnClick += HandleShowItemActions;
            
        }
    }

    public void UpdateData(int itemIndex, Sprite itemImage)
    {
        if (listofUIItems.Count > itemIndex)
        {
            listofUIItems[itemIndex].SetData(itemImage);
        }
    }

    private void HandleShowItemActions(UI_InventoryItem item)
    {

    }

    private void HandleEndDrag(UI_InventoryItem item)
    {

    }

    private void HandleSwap(UI_InventoryItem item)
    {

    }

    private void HandleBeginDrag(UI_InventoryItem item)
    {

    }

    private void HandleItemSelection(UI_InventoryItem item)
    {
       int index = listofUIItems.IndexOf(item);
        if (index == -1)
            return;
        OnDescriptionRequested?.Invoke(index);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        itemDescription.ResetDescription();
        ResetSelection();
    }

    private void ResetSelection()
    {
        itemDescription.ResetDescription();
        DeselectAllItems();
    }

    private void DeselectAllItems()
    {
       foreach(UI_InventoryItem item in listofUIItems)
        {
            item.Deselect();
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    internal void UpdateDescription(int itemIndex, Sprite itemImage, string name, string description)
    {
        itemDescription.SetDescription(itemImage, name, description);
        DeselectAllItems();
        listofUIItems[itemIndex].Select();
    }
}
