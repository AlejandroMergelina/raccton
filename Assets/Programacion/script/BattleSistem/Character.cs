using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{

    protected bool stoon;

    [SerializeField]
    protected CharacterSO characterData;
    public CharacterSO CharacterData { get => characterData; set => characterData = value; }

    protected virtual void Start()
    {

        characterData.CurrentHP = characterData.MaxHP;

    }

    public void HealHP(int val)
    {
        characterData.HealHP(val);

    }


    //([{(2 * Nv. / 5 + 2) * Ataque* Poder / Defensa} / 50]+2)
    public abstract void Attack(Character it);



    public void TakeDamage(int dmg)
    {

        characterData.CurrentHP -= dmg;
        if (characterData.CurrentHP < 0)
            characterData.CurrentHP = 0;

    }

    protected abstract void FinishAnimationAtack();
    
    public string GetName()
    {
        return characterData.Name;
    }

    public int GetMaxHP()
    {
        return characterData.MaxHP;
    }



    public int GetHP()
    {

        return characterData.CurrentHP;

    }


    public int GetID()
    {

        return characterData.ID;

    }
    public void SetID(int iD)
    {
        characterData.ID = iD;

    }

}
