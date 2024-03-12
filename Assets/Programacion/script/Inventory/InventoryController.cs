using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory.UI;
using Inventory.Model;
using System.Text;
using System;

namespace Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField]
        private UIInventoryPage inventoryUI;

        [SerializeField]
        private InventorySO inventoryData;

        [SerializeField]
        private List<InventoryItem> initialItems = new List<InventoryItem>();//lista de structs InventoryItem

        [SerializeField]
        CharacterSO[] characters;

        [SerializeField]
        private BattleSistem battleSistem;

        private void Start()
        {
            PrepareUI();
            PrepareInventoryData();
        }

        private void PrepareInventoryData()
        {
            inventoryData.Initialize();
            inventoryData.OnInventoryUpdated += UpdateIventoryUI;//sucribe
            foreach (InventoryItem item in initialItems)
            {

                if (item.IsEmpty)
                    continue;
                inventoryData.AddItem(item);//llama a la funcion de la instscis de la clase UIInventoryPage mete los items en el invetario

            }
        }

        private void UpdateIventoryUI(Dictionary<int, InventoryItem> inventoryState)
        {

            inventoryUI.ResetAllItems();
            foreach (KeyValuePair<int, InventoryItem> item in inventoryState)
            {

                inventoryUI.UpdateData(item.Key, item.Value.Item.ItemImage, item.Value.Quantity);

            }
            
        }

        private void PrepareUI()
        {
            inventoryUI.InitializeInventoryUI(inventoryData.Size);//llama y le pasa el tamaño del inventario segun el scripteable objet de la clase InvetorySO
            inventoryUI.OnDescriptionRequested += HandleDescriptionRequest;//suscrive a la funcion OnDescriptionRequested del UIInventoryPage
            inventoryUI.OnSwapItems += HandleSawmpItem;//suscrive
            inventoryUI.OnStartDragging += HandleDragging;//suscrive
            inventoryUI.OnItemActionRequested += HandleItemActionRequest;//suscrive
        }

        private void HandleItemActionRequest(int itemIndex)
        {
            
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if(inventoryItem.IsEmpty)
                return;

            IItemAction itemAction = inventoryItem.Item as IItemAction;
            if(itemAction != null)
            {

                
                inventoryUI.ShowItemAction(itemIndex);
                inventoryUI.AddAction(itemAction.ActionName, () => PerformElection(itemIndex, itemAction));
                //llama a la funcion AddAction del UIInventoryPage le pasa por parametro un string con el texto del boton y la Action utilizando la funzion PerformAction

            }

            IDestroyableItem destroyableItem = inventoryItem.Item as IDestroyableItem;
            if (destroyableItem != null)
            {

                inventoryUI.AddAction("Drop", () => DropItem(itemIndex, inventoryItem.Quantity));

            }

        }

        private void DropItem(int itemIndex, int quantity)
        {
            inventoryData.RemoveItem(itemIndex, quantity);
            inventoryUI.ResetSelection();
        }

        public void PerformElection(int itemIndex, IItemAction itemAction)
        {
            inventoryUI.RemoveAction();
            foreach (CharacterSO character in characters)
            {
                
                inventoryUI.AddAction(character.Name, () => PerformAction(itemIndex, character)); 

            }

        }

        //primero resta en 1 del total de ese item porque lo consume lugo llama al PerformAction del personaje que se le pasa por parametro y ejecuta los StatsModifier
        public void PerformAction(int itemIndex, CharacterSO character)
        {


            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
                return;

            IDestroyableItem destroyableItem = inventoryItem.Item as IDestroyableItem;
            if (destroyableItem != null)
            {

                inventoryData.RemoveItem(itemIndex, 1);

            }

            IItemAction itemAction = inventoryItem.Item as IItemAction;
            if (itemAction != null)
            {

                itemAction.PerformAction(character/*aqui ira el jugador seleccionado*//*, inventoryItem.ItemState*/);
                if (inventoryData.GetItemAt(itemIndex).IsEmpty)
                    inventoryUI.ResetSelection();
                inventoryUI.Hide();
                battleSistem.MainChOrder1.Dequeue();
                battleSistem.CheckLive("All");
            }


        }

        private void HandleDragging(int itemIndex)
        {

            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
                return;
            inventoryUI.CreateDraggedItem(inventoryItem.Item.ItemImage, inventoryItem.Quantity);

        }

        private void HandleSawmpItem(int itemIndex_1, int itemIndex_2)
        {

            inventoryData.SwapItems(itemIndex_1, itemIndex_2);

        }

        private void HandleDescriptionRequest(int itemIndex)
        {

            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
            {

                inventoryUI.ResetSelection();
                return;

            }

            ItemSO item = inventoryItem.Item;
            string description = PrepareDescription(inventoryItem);
            inventoryUI.UpdateDescription(itemIndex, item.ItemImage, item.Name, description);

        }

        private string PrepareDescription(InventoryItem inventoryItem)
        {

            StringBuilder sb = new StringBuilder();
            sb.Append(inventoryItem.Item.Description);
            sb.AppendLine();
            //for(int i = 0; i < inventoryItem.ItemState.Count; i++)
            //{

            //    sb.Append($"{inventoryItem.ItemState[i]._ItemParameter.ParameterName}" + $":{inventoryItem.ItemState[i].Values} / {inventoryItem.Item.DefaultParameterList[i].Values}");
            //    sb.AppendLine();
            //}
            return sb.ToString();

        }

        // Implementar en el nuevo sisitema de input de unity solo para cuando no este en combate
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                if (inventoryUI.isActiveAndEnabled == false)
                {
                    inventoryUI.Show();
                    foreach (KeyValuePair<int, InventoryItem> item in inventoryData.GetCurrentIventoryState())
                    {

                        inventoryUI.UpdateData(item.Key, item.Value.Item.ItemImage, item.Value.Quantity);

                    }
                }
                else
                {

                    inventoryUI.Hide();

                }
            }
        }

        public void OppeInventoty()
        {
            battleSistem.OptionCanvas.SetActive(false);
            inventoryUI.Show();
            foreach (KeyValuePair<int, InventoryItem> item in inventoryData.GetCurrentIventoryState())
            {

                inventoryUI.UpdateData(item.Key, item.Value.Item.ItemImage, item.Value.Quantity);

            }

        }

    }
}