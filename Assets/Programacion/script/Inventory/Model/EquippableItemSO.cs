using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class EquippableItemSO : ItemSO, IDestroyableItem, IItemAction
    {
        public string ActionName => "Equip";

        public void PerformAction(CharacterSO character/*, List<ItemParameter> itemState = null*/)
        {
            AgentWeaponSO weaponSystem = character.AgentWeapon;

            if (weaponSystem != null)
            {

                weaponSystem.SetWeapon(this/*, itemState == null ? DefaultParameterList : itemState*/);

            }
        }

        public void AplayModifiers(CharacterSO character)
        {

            AgentWeaponSO weaponSystem = character.AgentWeapon;

            foreach (ModifierData data in modifiersDatas)
            {
                Debug.Log("arma");
                data.StatModifier.AffectCharacter(character, data.Value);

            }
            //if (weaponSystem != null)
            //{

            //    foreach (ModifierData data in weaponSystem.Weapon.modifiersDatas)
            //    {

            //        data.StatModifier.RemubeModifiers(character, data.Value);

            //    }

            //}


        }

        public void RemuveModifiers(CharacterSO character)
        {

            AgentWeaponSO weaponSystem = character.AgentWeapon;

            foreach (ModifierData data in modifiersDatas)
            {
                Debug.Log("arma");
                data.StatModifier.RemubeModifiers(character, data.Value);

            }
            //if (weaponSystem != null)
            //{

            //    foreach (ModifierData data in weaponSystem.Weapon.modifiersDatas)
            //    {

            //        data.StatModifier.RemubeModifiers(character, data.Value);

            //    }

            //}

        }
    }
}