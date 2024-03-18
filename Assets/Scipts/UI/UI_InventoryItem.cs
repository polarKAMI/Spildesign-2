using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_InventoryItem : MonoBehaviour
{
    [SerializeField]
    private Image itemImage;

    [SerializeField]
    private Image itemBorder;

    public event Action<UI_InventoryItem> OnItemClick,
        OnItemDroppedOn, OnItemBeginDrag, OnItemEndDrag,
        OnRightMouseBtnClick;

    private bool empty = true;

    private void Awake()
    {
        ResetData();
        Deselect();
    }

    public void ResetData()
    {
        this.itemImage.gameObject.SetActive(false);
        empty = true;
    }

    public void Deselect()
    {
        itemBorder.enabled = false;
    }

    public void Select()
    {
        itemBorder.enabled = true;
    }

    public void SetData(Sprite sprite)
    {
        this.itemImage.gameObject.SetActive (true);
        this.itemImage.sprite = sprite;
        empty = false;
    }

    public void OnBeginDrag()
    {
        if (empty)
            return;
        OnItemBeginDrag?.Invoke(this);
    }

    public void OnDrop()
    {
        OnItemDroppedOn?.Invoke(this);
    }
  
    public void OnEndDrag()
    {
        OnItemEndDrag?.Invoke(this);
    }

    public void OnPointerClick(BaseEventData data)
    {
        PointerEventData pointerData = (PointerEventData)data;
        if(pointerData.button == PointerEventData.InputButton.Right)
        {
            OnRightMouseBtnClick?.Invoke(this);
        }
        else
        {
            OnItemClick?.Invoke(this);
        }
    }
}
