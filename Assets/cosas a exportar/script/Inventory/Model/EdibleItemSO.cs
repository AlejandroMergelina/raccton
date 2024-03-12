using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class EdibleItemSO : ItemSO, IDestroyableItem, IItemAction
    {

        

        public string ActionName => "Consume";

        public void PerformAction(CharacterSO character/*, List<ItemParameter> itemState = null*/)
        {
            foreach(ModifierData data in modifiersDatas)
            {

                data.StatModifier.AffectCharacter(character, data.Value);

            }
            
        }
    }

    public interface IDestroyableItem
    {



    }

    public interface IItemAction
    {
        public string ActionName { get;}

        void PerformAction(CharacterSO character/*, List<ItemParameter> itemState*/);
    }

    [Serializable]
    public class ModifierData
    {
        [SerializeField]
        private CharacterStatModifierSO statModifier;
        public CharacterStatModifierSO StatModifier { get => statModifier; set => statModifier = value; }
        [SerializeField]
        private int value;
        public int Value { get => value; set => this.value = value; }
    }

}