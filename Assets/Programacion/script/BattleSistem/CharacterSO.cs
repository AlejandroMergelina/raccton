using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Stads { MaxHP, Power, Defense, Speed}

[CreateAssetMenu]
public class CharacterSO : ScriptableObject
{
    

    [SerializeField]
    private string _name;
    public string Name { get => _name;}

    [SerializeField]
    private int maxHP;
    public int MaxHP { get => maxHP;}

    [SerializeField]
    private int currentHP;
    public int CurrentHP { get => currentHP;set => currentHP = value; }

    [SerializeField]
    private Stad power;
    public Stad Power { get => power;set => power = value; }

    [SerializeField]
    private Stad defense;
    public Stad Defense { get => defense; set => defense = value;}

    [SerializeField]
    private Stad speed;
    public Stad Speed { get => speed; set => speed = value;}
    
    [SerializeField]
    private int iD;
    public int ID { get => iD; set => iD = value; }

    [SerializeField]
    private AgentWeaponSO agentWeapon;
    public AgentWeaponSO AgentWeapon { get => agentWeapon; set => agentWeapon = value; }


    private Dictionary<AlteredEffect, int> continuosAlteredEffects = new Dictionary<AlteredEffect, int>();
    public Dictionary<AlteredEffect, int> ContinuosAlteredEffects { get => continuosAlteredEffects; set => continuosAlteredEffects = value; }

    private Dictionary<staticAlteredEffect, int> alteredEffects = new Dictionary<staticAlteredEffect, int>();
    public Dictionary<staticAlteredEffect, int> AlteredEffects { get => alteredEffects; set => alteredEffects = value; }

    private void OnEnable()
    {

        agentWeapon.OnAddModifiers += WeaponAddModifiers;
        agentWeapon.OnRemuveModifiers += WeaponRemuveModifiers;

    }
    public void AddAlteredEffect(staticAlteredEffect effectData2Add, int turns)
    {

        if (continuosAlteredEffects.ContainsKey(effectData2Add))
        {

            continuosAlteredEffects[effectData2Add] += turns;

        }
        else
        {

            continuosAlteredEffects.Add(effectData2Add, turns);
            effectData2Add.Aplay(this);
        }

    }

    public void AddAlteredEffect(AlteredEffect effectData2Add, int turns)
    {

        if (continuosAlteredEffects.ContainsKey(effectData2Add))
        {

            continuosAlteredEffects[effectData2Add] += turns;

        }
        else
        {

            continuosAlteredEffects.Add(effectData2Add, turns);
                                    
        }

    }

    public void AplayContinuosAlteredEfects()
    {
        {
            List<AlteredEffect> alteredEffects2Remove = new List<AlteredEffect>();
            List<AlteredEffect> alteredEffects2DecresValue = new List<AlteredEffect>();

            foreach (KeyValuePair<AlteredEffect, int> effect in continuosAlteredEffects)
            {
                if (effect.Value > 0)
                {

                    effect.Key.Aplay(this);

                    alteredEffects2DecresValue.Add(effect.Key);

                }
                else
                {

                    alteredEffects2Remove.Add(effect.Key);

                }


            }

            foreach (AlteredEffect effect in alteredEffects2DecresValue)
            {

                if (continuosAlteredEffects.ContainsKey(effect))
                {

                    continuosAlteredEffects[effect]--;

                }

            }


            foreach (AlteredEffect effect in alteredEffects2Remove)
            {
                continuosAlteredEffects.Remove(effect);
            }
        }

        {
            List<staticAlteredEffect> alteredEffects2Remove = new List<staticAlteredEffect>();
            List<staticAlteredEffect> alteredEffects2DecresValue = new List<staticAlteredEffect>();

            foreach (KeyValuePair<staticAlteredEffect, int> effect in AlteredEffects)
            {
                if (effect.Value > 0)
                {

                    alteredEffects2DecresValue.Add(effect.Key);

                }
                else
                {

                    alteredEffects2Remove.Add(effect.Key);

                }


            }

            foreach (staticAlteredEffect effect in alteredEffects2DecresValue)
            {

                if (continuosAlteredEffects.ContainsKey(effect))
                {

                    continuosAlteredEffects[effect]--;

                }

            }


            foreach (staticAlteredEffect effect in alteredEffects2Remove)
            {
                effect.RemubeEffect(this);
                alteredEffects.Remove(effect);
            }
        }

    }

    public void HealHP(int val)
    {
        if (maxHP > currentHP + val)
            currentHP += val;
        else if (maxHP <= currentHP + val)
            currentHP = maxHP;

    }


    private void WeaponRemuveModifiers()
    {
        agentWeapon.Weapon.RemuveModifiers(this);
    }

    private void WeaponAddModifiers()
    {
        agentWeapon.Weapon.AplayModifiers(this);
    }



}
