using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterStatModifierSO : ScriptableObject
{
    public abstract void AffectCharacter(CharacterSO character, int val);
    public abstract void RemubeModifiers(CharacterSO character, int val);
}
