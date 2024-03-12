using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace Inventory.Model
{

    [CreateAssetMenu]
    public class InventorySO : ScriptableObject
    {

        [SerializeField]
        private List<InventoryItem> inventoryItems;

        [SerializeField]
        private int size = 10;
        public int Size { get => size; set => size = value; }

        public event Action<Dictionary<int, InventoryItem>> OnInventoryUpdated;

        public void Initialize()
        {
            inventoryItems = new List<InventoryItem>();
            for (int i = 0; i < size; i++)
            {
                inventoryItems.Add(InventoryItem.GetEmptyItem());
            }
        }

        public int AddItem(ItemSO item, int quantity/*, List<ItemParameter> itemState = null*/)
        {

            if(item.IsStackable == false)
            {

                for (int i = 0; i < inventoryItems.Count; i++)
                {
                    while(quantity > 0 && IsInventoryFull() == false)
                    {

                        quantity -= AddItemToFirstFreeSlot(item, 1/*, itemState*/);

                    }
                    InformationAboutChange();
                    return quantity;
                }

            }

            quantity = AddStackableItem(item, quantity);
            InformationAboutChange();
            return quantity;
        }

        private int AddItemToFirstFreeSlot(ItemSO item, int quantity/*, List<ItemParameter> itemState = null*/)
        {
            InventoryItem newItem = new InventoryItem { Item = item, Quantity = quantity/*, ItemState = new List<ItemParameter>(itemState == null? item.DefaultParameterList : itemState)*/ };
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

        private bool IsInventoryFull() => inventoryItems.Where(item => item.IsEmpty).Any() == false;

        private int AddStackableItem(ItemSO item, int quantity)
        {

            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].IsEmpty)
                    continue;
                if(inventoryItems[i].Item.ID== item.ID)
                {

                    int amoutPosibleToTake = inventoryItems[i].Item.MaxStackSize -inventoryItems[i].Quantity;

                    if (quantity > amoutPosibleToTake)
                    {

                        inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].Item.MaxStackSize);
                        quantity -= amoutPosibleToTake;

                    }
                    else
                    {

                        inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].Quantity + quantity);
                        InformationAboutChange();
                        return 0;
                    }

                }
            }
            while(quantity > 0 && IsInventoryFull() == false)
            {

                int newQuantity = Mathf.Clamp(quantity,0,item.MaxStackSize);
                quantity -= newQuantity;
                AddItemToFirstFreeSlot(item, newQuantity);

            }
            return quantity;
        }

        public void RemoveItem(int itemIndex, int amount)
        {
            
            if(inventoryItems.Count > itemIndex)
            {

                if (inventoryItems[itemIndex].IsEmpty)
                    return;
                int reminder = inventoryItems[itemIndex].Quantity - amount;
                if (reminder <= 0)
                    inventoryItems[itemIndex] = InventoryItem.GetEmptyItem();
                else
                    inventoryItems[itemIndex] = inventoryItems[itemIndex].ChangeQuantity(reminder);

                InformationAboutChange();

            }

        }

        public void AddItem(InventoryItem item)
        {

            AddItem(item.Item, item.Quantity);

        }

        public Dictionary<int, InventoryItem> GetCurrentIventoryState()
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

        public InventoryItem GetItemAt(int itemIndex)
        {
            return inventoryItems[itemIndex];
        }

        public void SwapItems(int itemIndex_1, int itemIndex_2)
        {
            Debug.Log(itemIndex_1);
            Debug.Log(itemIndex_2);
            InventoryItem item1 = inventoryItems[itemIndex_1];
            inventoryItems[itemIndex_1] = inventoryItems[itemIndex_2];
            inventoryItems[itemIndex_2] = item1;
            InformationAboutChange();
        }

        private void InformationAboutChange()
        {
            OnInventoryUpdated?.Invoke(GetCurrentIventoryState());
        }
    }

    [Serializable]
    public struct InventoryItem
    {
        [SerializeField]
        private int quantity;
        public int Quantity { get => quantity; set => quantity = value; }
        [SerializeField]
        private ItemSO item;
        public ItemSO Item { get => item; set => item = value; }
        //[SerializeField]
        //private List<ItemParameter> itemState;
        //public List<ItemParameter> ItemState { get => itemState; set => itemState = value; }
        [SerializeField]
        private bool isEmpty => item == null;
        public bool IsEmpty { get => isEmpty; }
       

        public InventoryItem ChangeQuantity(int newQuuantity)//funcion que debuelbe una variable del tipo de la clase
        {
            return new InventoryItem
            {
                item = item,//para mantener el mismo item y que no varie
                quantity = newQuuantity,
                //itemState = new List<ItemParameter>(itemState)
            };

        }

        public static InventoryItem GetEmptyItem() => new InventoryItem()//hace lo mismo que lo de antes pero simplifica codigo
        {

            item = null,
            quantity = 0,
            //itemState = new List<ItemParameter>()

        };

    }

}
