using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter2 : MainCharacter
{

    //funcion de elejier tecla aleatoria
    //funcion que seejecute al llamar aesa tecka

    private KeyCode currentRandomKey;

    private KeyCode[] key2Select;

    protected override void Update()
    {
        base.Update();

        if (canAttack && Input.GetKeyDown(currentRandomKey))
        {
            print("lanzo objeto");
            currentRandomKey = RandomKey();

        }

    }

    private void StaratAttack()
    {

        currentRandomKey = RandomKey();

    }

    private KeyCode RandomKey()
    {

        return key2Select[Random.Range(0, key2Select.Length)];

    }

}
