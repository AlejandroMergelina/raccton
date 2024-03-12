using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwichModeControler : MonoBehaviour
{

    [SerializeField]
    private GameObject camera1;
    [SerializeField]
    private GameObject zoneNavigationMode;

    [SerializeField]
    private GameObject camera2;
    [SerializeField]
    private GameObject zoneCombatMode;
    [SerializeField]
    private GameManager gameManager;

    private void OnEnable()
    {

        gameManager.OnChange2CombatMode += ChangeCamera2CombatMode;
        gameManager.OnChange2NavigationMode += ChangeCamera2NavegationMode;
        gameManager.OnChageNavigationZone += ChageObjectForNavigationZone;
    }

    private void ChageObjectForNavigationZone(GameObject obj)
    {

        zoneNavigationMode = obj;

    }

    void ChangeCamera2NavegationMode()
    {

        camera1.SetActive(true);
        camera2.SetActive(false);

        zoneNavigationMode.SetActive(true);
        zoneCombatMode.SetActive(false);
                
    }

    public void ChangeCamera2CombatMode()
    {
        camera1.SetActive(false);
        camera2.SetActive(true);

        zoneNavigationMode.SetActive(false);
        zoneCombatMode.SetActive(true);
    }

}
