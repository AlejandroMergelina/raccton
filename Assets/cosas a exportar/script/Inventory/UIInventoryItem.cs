using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;

namespace Inventory.UI
{
    public class UIInventoryItem : MonoBehaviour
    {

        [SerializeField]
        private Image ItemImage;
        [SerializeField]
        private TMP_Text quantityTxt;

        [SerializeField]
        private Image borderImagen;

        public event Action<UIInventoryItem> OnItemClicked, OnItemDroppedOn, OnItemBeingDrag, OnItemEndDrag, OnRightMouseBtnClick;

        private bool empty = true;

        public void Awake()
        {

            ResetData();
            Deselect();

        }

        public void ResetData()
        {

            ItemImage.gameObject.SetActive(false);
            empty = true;

        }

        public void Deselect()
        {

            borderImagen.enabled = false;

        }

        public void SetData(Sprite sprite, int quantity)
        {

            ItemImage.gameObject.SetActive(true);
            ItemImage.sprite = sprite;
            quantityTxt.text = quantity + "";
            empty = false;

        }

        public void Select()
        {

            borderImagen.enabled = true;

        }
        //se activan en el event triger
        public void OnPointerClick(BaseEventData data)
        {
            
            PointerEventData pointerData = (PointerEventData)data;
            
            if (pointerData.button == PointerEventData.InputButton.Right)
            {
                
                OnRightMouseBtnClick?.Invoke(this);//lanza la llamada para los subcriptores

            }
            else
            {

                OnItemClicked?.Invoke(this);//lanza la llamada para los subcriptores

            }

        }
        //se activan en el event triger
        public void OnEndDrag()
        {

            OnItemEndDrag?.Invoke(this);//lanza la llamada para los subcriptores

        }
        //se activan en el event triger
        public void OnBegingDrag()
        {

            if (empty)
                return;

            OnItemBeingDrag?.Invoke(this);//lanza la llamada para los subcriptores

        }
        //se activan en el event triger
        public void OnDrop()
        {

            OnItemDroppedOn?.Invoke(this);//lanza la llamada para los subcriptores

        }

        

       

    }
}