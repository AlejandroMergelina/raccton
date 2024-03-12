using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Stad
{

    [SerializeField]
    private int baseValue;

    private List<int> modifiers = new List<int>();

    public int GetValue()
    {
        int finalValeu = baseValue;
        modifiers.ForEach(x => finalValeu += x);

        return finalValeu;
    }

    public void AddModifier(int modifier)
    {

        if(modifier!=0)
        {

            modifiers.Add(modifier);

        }

    }

    public void RemoveModifier(int modifier)
    {

        if (modifier != 0)
        {

            modifiers.Remove(modifier);

        }

    }


}
