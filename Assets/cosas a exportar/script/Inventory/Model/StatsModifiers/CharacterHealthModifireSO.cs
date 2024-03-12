using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterHealthModifireSO : CharacterStatModifierSO
{
    public override void AffectCharacter(CharacterSO character, int val)
    {
        
        if(character != null)
            character.HealHP(val);
    }

    public override void RemubeModifiers(CharacterSO character, int val)
    {
        
    }
}
