using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.UI
{
    public class UIInventoryPage : MonoBehaviour
    {

        [SerializeField]
        private UIInventoryItem itemPrefab;

        [SerializeField]
        private RectTransform contentPanel;

        [SerializeField]
        private UIInventoryDescription itemDescription;

        [SerializeField]
        private MoseFollower mouseFollower;

        private List<UIInventoryItem> listOfUIItems = new List<UIInventoryItem>();

        private int currentlyDraggedItemIndex = -1;

        public event Action<int> OnDescriptionRequested, OnItemActionRequested, OnStartDragging;//estan obligados a tener el parametro de entrada int

        public event Action<int, int> OnSwapItems;

        [SerializeField]
        private ItemActionPanel actionPanel;

        private void Awake()
        {

            Hide();
            mouseFollower.Toogle(false);
            itemDescription.ResetDescription();

        }
 
        public void InitializeInventoryUI(int inventorysize)
        {
            //Con un bucle crea las instancias de los objetos/espacis del inventario
            for (int i = 0; i < inventorysize; i++)
            {

                UIInventoryItem uiItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
                uiItem.transform.SetParent(contentPanel);//le hace hijo al panel de unity que contiene el espacio del inventario
                listOfUIItems.Add(uiItem);//el objeto creado es almacenado en la lista uiItem para allmacenar todos los objetos
                uiItem.OnItemClicked += HandleItemSelection;//suscribe la funcion HandleItemSelection de la clase actual a la funcion OnItemClicked del objeto prviamente creado
                uiItem.OnItemBeingDrag += HandleBeginDrag;
                uiItem.OnItemDroppedOn += HandleSwap;
                uiItem.OnItemEndDrag += HandleEndDrag;
                uiItem.OnRightMouseBtnClick += HandleShowItemActions;
            }

        }

        public void ResetAllItems()
        {

            foreach (UIInventoryItem item in listOfUIItems)
            {
                item.ResetData();
                item.Deselect();
            }

        }

        

        public void UpdateDescription(int itemIndex, Sprite itemImage, string name, string description)
        {
            itemDescription.SetDescription(itemImage, name, description);
            DeselectAllItems();
            listOfUIItems[itemIndex].Select();
        }

        public void UpdateData(int itemIndex, Sprite itemImage, int itemQuantity)
        {

            if (listOfUIItems.Count > itemIndex)
            {

                listOfUIItems[itemIndex].SetData(itemImage, itemQuantity);
            }

        }

        private void HandleShowItemActions(UIInventoryItem InventoryItemUI)
        {
            int index = listOfUIItems.IndexOf(InventoryItemUI);
            if (index == -1)
            {

                return;

            }
            OnItemActionRequested?.Invoke(index);
        }

        private void HandleEndDrag(UIInventoryItem InventoryItemUI)
        {

            ResetDraggtedItem();

        }

        private void HandleSwap(UIInventoryItem InventoryItemUI)
        {

            int index = listOfUIItems.IndexOf(InventoryItemUI);
            if (index != -1)
            {

                OnSwapItems?.Invoke(currentlyDraggedItemIndex, index);
                HandleItemSelection(InventoryItemUI);

            }

            
 
        }

        private void ResetDraggtedItem()
        {
            mouseFollower.Toogle(false);
            currentlyDraggedItemIndex = -1;
        }

        private void HandleBeginDrag(UIInventoryItem InventoryItemUI)
        {
  
            int index = listOfUIItems.IndexOf(InventoryItemUI);
            if (index == -1)
                return;
            currentlyDraggedItemIndex = index;
            HandleItemSelection(InventoryItemUI);
            OnStartDragging?.Invoke(index);

        }

        public void CreateDraggedItem(Sprite sprite, int quantity)
        {

            mouseFollower.Toogle(true);
            mouseFollower.SetData(sprite, quantity);

        }

        private void HandleItemSelection(UIInventoryItem InventoryItemUI)
        {

            int index = listOfUIItems.IndexOf(InventoryItemUI);
            if (index == -1)
                return;
            OnDescriptionRequested?.Invoke(index);

        }

        public void Show()
        {

            gameObject.SetActive(true);
            ResetSelection();

        }

        public void ResetSelection()
        {
            itemDescription.ResetDescription();
            DeselectAllItems();
        }

        public void AddAction(string actionName, Action performAction)
        {

            actionPanel.AddButton(actionName, performAction);

        }
        public void RemoveAction()
        {

            actionPanel.RemoveOldButtons();

        }

        public void ShowItemAction(int itemIndex)
        {

            actionPanel.Toggle(true);
            actionPanel.transform.position = listOfUIItems[itemIndex].transform.position;

        }

        private void DeselectAllItems()
        {
            foreach (UIInventoryItem item in listOfUIItems)
            {
                item.Deselect();
            }
            actionPanel.Toggle(false);
        }

        public void Hide()
        {
            actionPanel.Toggle(false);
            gameObject.SetActive(false);
            ResetSelection();

        }

    }
}