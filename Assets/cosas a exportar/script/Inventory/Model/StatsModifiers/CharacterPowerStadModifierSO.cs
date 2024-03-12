using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterPowerStadModifierSO : CharacterStatModifierSO
{

    private void OnEnable()
    {
        
    }
    public override void AffectCharacter(CharacterSO character, int val)
    {

        if (character != null)
            character.Power.AddModifier(val);
    }

    public override void RemubeModifiers(CharacterSO character,int val)
    {
        if (character != null)
            character.Power.RemoveModifier(val);
    }

}
