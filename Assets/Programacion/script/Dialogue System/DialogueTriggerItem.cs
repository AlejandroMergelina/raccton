using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTriggerItem : DialogueTrigerAction
{
    //[SerializeField]
    //private Dictionary<ItemSO, int> items = new Dictionary<ItemSO, int>();

    [SerializeField]
    private ItemSO[] items;
    [SerializeField]
    private int[] itemsQuantity;

    [SerializeField]
    private InventorySO inventory;

    protected override void Interact()
    {
        if (playerInRange)
        {

            dialogueManager.EnterDialogueMode(inkJSON, this);

        }
    }

    public override void Action()
    {
        //foreach (KeyValuePair<ItemSO,int> item in items)
        //{

        //    inventory.AddItem(item.Key, item.Value);

        //}
        for (int i = 0; i < items.Length; i++)
        {
            inventory.AddItem(items[i], itemsQuantity[i]);
        }
    }
}
