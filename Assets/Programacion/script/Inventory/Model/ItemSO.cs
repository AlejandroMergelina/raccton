using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{

    public abstract class ItemSO : ScriptableObject
    {

        [field: SerializeField]
        private bool isStackable;
        public bool IsStackable { get => isStackable; set => isStackable = value; }

        public int ID => GetInstanceID();

        [field: SerializeField]
        private int maxStackSize = 1;
        public int MaxStackSize { get => maxStackSize; set => maxStackSize = value; }

        [field: SerializeField]
        private string _name;
        public string Name { get => _name; set => _name = value; }

        [field: SerializeField]
        [field: TextArea]
        private string description;
        public string Description { get => description; set => description = value; }

        [field: SerializeField]
        private Sprite itemImage;
        public Sprite ItemImage { get => itemImage; set => itemImage = value; }
        
        [SerializeField]
        protected List<ModifierData> modifiersDatas = new List<ModifierData>();
        public List<ModifierData> ModifiersDatas { get => modifiersDatas; set => modifiersDatas = value; }

        //[SerializeField]
        //private List<ItemParameter> defaultParameterList;
        //public List<ItemParameter> DefaultParameterList { get => defaultParameterList; set => defaultParameterList = value; }


    }

    //[Serializable]
    //public struct ItemParameter : IEquatable<ItemParameter>
    //{
    //    [SerializeField]
    //    private ItemParameterSO itemParameter;
    //    public ItemParameterSO _ItemParameter { get => itemParameter; set => itemParameter = value; }
    //    [SerializeField]
    //    private float values;
    //    public float Values { get => values; set => values = value; }

    //    public bool Equals(ItemParameter other)
    //    {

    //        return other.itemParameter == itemParameter;

    //    }
    //}
}

