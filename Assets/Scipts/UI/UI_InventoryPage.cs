using Inventory.UI;
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

    [SerializeField]
    private UIInventoryDescription itemDescription;

    List<UI_InventoryItem> listofUIItems = new List<UI_InventoryItem>();


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
        Debug.Log("lets go");
    }

    public void Show()
    {
        gameObject.SetActive(true);
        itemDescription.ResetDescription(); 
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
