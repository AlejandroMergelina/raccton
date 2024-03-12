using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START,ORDER, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSistem : MonoBehaviour
{
    [SerializeField]
    private InputManager inputManager;

    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private GameObject enemySelectCanvas;
    [SerializeField]
    private GameObject optionCanvas;
    public GameObject OptionCanvas { get => optionCanvas; set => optionCanvas = value; }

    [SerializeField]
    private GameObject[] selectEnemyButtons;

    [SerializeField]
    private GameObject[] mainCharactersPrefab;
    [SerializeField]
    private GameObject[] enemyCharactersPrefab;

    [SerializeField]
    private Transform[] mainCharactersStation;
    [SerializeField]
    private Transform[] enemyCharactersStation;

    private GameObject enemyInWorld;

    private BattleState state;

    private LinkedList<int> order = new LinkedList<int>();

    private Queue<Character> EnemyOrder = new Queue<Character>();
    private Queue<Character> MainChOrder = new Queue<Character>();
    public Queue<Character> MainChOrder1 { get => MainChOrder; set => MainChOrder = value; }

    private Dictionary<int, MainCharacter> MainCharactersAlive = new Dictionary<int, MainCharacter>();
    private Dictionary<int, MainCharacter> MainCharacters = new Dictionary<int, MainCharacter>();
    private Dictionary<int, EnemyCharacter> EnemyCharacters = new Dictionary<int, EnemyCharacter>();

    [SerializeField]
    int tusTurnos, turnosEnemigo;

    public static BattleSistem Instance;

    private void OnEnable()
    {

        gameManager.OnStartCombat += StartCombat;

    }

    public void StartCombat(GameObject enemyInWorld)
    {
        print("comienzan las hostias");
        this.enemyInWorld = enemyInWorld;
        state = BattleState.START;

        StartCoroutine(SetupBattle());

        if (Instance != null && Instance != this)
        {

            Destroy(this);

        }
        else
        {

            Instance = this;
            DontDestroyOnLoad(this);

        }

    }

    IEnumerator SetupBattle()
    {
        
        for (int i = 0; i < enemyCharactersPrefab.Length; i++)
        {

            GameObject clon = Instantiate(enemyCharactersPrefab[i], enemyCharactersStation[i].position, enemyCharactersStation[i].rotation);
            EnemyCharacter clonInf = clon.GetComponent<EnemyCharacter>();
            EnemyCharacters.Add(i,clonInf);
            clonInf.SetID(i);
            

        }

        for (int i = 0; i < mainCharactersPrefab.Length; i++)
        {
            

            GameObject clon = Instantiate(mainCharactersPrefab[i], mainCharactersStation[i].position, mainCharactersStation[i].rotation);
            MainCharacter clonInf = clon.GetComponent<MainCharacter>();
            MainCharactersAlive.Add(i,clonInf);
            MainCharacters.Add(i,clonInf);
            clonInf.SetID(i);



        }
                
        yield return new WaitForSeconds(2f);
        
        state = BattleState.ORDER;
        
        Order();

    }

    void Order()
    {
        
        if (EnemyOrder.Count == 0 && MainChOrder.Count == 0)
        {
            Dictionary<int, Character> HelpDiccionary = new Dictionary<int, Character>();

            foreach (KeyValuePair<int, EnemyCharacter> main in EnemyCharacters)
            {

                HelpDiccionary.Add(main.Key, main.Value);

            }

            SubOrder(HelpDiccionary, EnemyOrder);

            HelpDiccionary.Clear();

            foreach(KeyValuePair<int, MainCharacter> main in MainCharactersAlive)
            {

                HelpDiccionary.Add(main.Key, main.Value);

            }

            SubOrder(HelpDiccionary, MainChOrder);
            
        }

        //print("mainCh; " + MainChOrder.Count + "\n" + "Enemy: " + EnemyOrder.Count);

        if (EnemyOrder.Count == 0)
        {
            state = BattleState.PLAYERTURN;

            PlayerTurn();

        }
        else if (MainChOrder.Count == 0)
        {
            //print("hola");
            state = BattleState.ENEMYTURN;

            StartCoroutine(EnemyTurn(EnemyOrder.Peek()));

        }
        else
        {
            
            if (EnemyOrder.Peek().CharacterData.Speed.GetValue() > MainChOrder.Peek().CharacterData.Speed.GetValue())
            {
                
                state = BattleState.ENEMYTURN;
                
                StartCoroutine(EnemyTurn(EnemyOrder.Peek()));

            }
            else
            {
                state = BattleState.PLAYERTURN;
                
                PlayerTurn();

            }

        }

    }

    void SubOrder(Dictionary<int, Character> c, Queue<Character> q)
    {

        foreach (KeyValuePair<int, Character> mains in c)
        {

            order.AddFirst(mains.Value.CharacterData.Speed.GetValue());

        }

        int[] a = new int[order.Count];

        order.CopyTo(a, 0);

        Array.Sort(a);

        Array.Reverse(a);

        int reserSpeed = -1;

        foreach (int n in a)
        {

            if (reserSpeed != n)
            {

                foreach (KeyValuePair<int, Character> mains in c)
                {
                    if (mains.Value.CharacterData.Speed.GetValue() == n)
                    {
                        reserSpeed = n;

                        q.Enqueue(mains.Value);


                    }

                }

            }

        }

        order.Clear();

    }

    void PlayerTurn()
    {
        
        inputManager.SwichActionMap(ActionMaps.CombatMode);
        tusTurnos++;

        optionCanvas.SetActive(true);

        int i = 0;

        foreach (GameObject b in selectEnemyButtons)
        {

            if (EnemyCharacters.ContainsKey(i))
                b.SetActive(true);
            else
                b.SetActive(false);

            i++;
        }

    }

    IEnumerator EnemyTurn(Character me)
    {
        
        EnemyOrder.Dequeue();
        turnosEnemigo++;

        print("te ataca el enemigo");

        yield return new WaitForSeconds(1f);

        //foreach(KeyValuePair<int, MainCharacter> main in MainCharactersAlive)
        //{

        //    main.Value.SetCanDodge(true);

        //}

        inputManager.SwichActionMap(ActionMaps.Doge);


        int rng;
        do
        {

            rng = UnityEngine.Random.Range(0, mainCharactersPrefab.Length);
            //print("comrpueba el numero random es = " + rng);
            //print("el rng existe" + MainCharactersAlive.ContainsKey(rng));
            //if (MainCharactersAlive.ContainsKey(rng) == true)
            //    print("contaiss ");

            //if (MainCharactersAlive != null)
            //    print("maincha" );

        } while (MainCharactersAlive.ContainsKey(rng) == false && MainCharactersAlive.Count != 0);

        //print("el numero random es = " + rng);

        MainCharactersAlive.TryGetValue(rng, out MainCharacter mainCharacterRandom);

        me.Attack(mainCharacterRandom);

        //this.it = mainCharacterRandom;
        //i = rng;

        //CheckLive("Main");//active at the end of enemy animation

        //if (it.GetHP() <= 0)

    }

    public void OnAtackButton()
    {

        enemySelectCanvas.SetActive(true);
        optionCanvas.SetActive(false);

    }

    public void OnSelectEnemy(int i)
    {

        Character me = MainChOrder.Peek();
                 
        EnemyCharacters.TryGetValue(i, out EnemyCharacter it);

        //this.it = it;
        //this.i = i;

        MainChOrder.Dequeue();

        enemySelectCanvas.SetActive(false);

        me.Attack(it);
  
    }

    public void CheckLive(string rival)
    {
        Dictionary<int, Character> HelpDiccionary = new Dictionary<int, Character>();
        
        foreach (KeyValuePair<int, MainCharacter> main in MainCharactersAlive)
        {

            main.Value.SetCanDodge(false);

        }

        int count = MainCharacters.Count;
        //foreach(KeyValuePair<int, MainCharacter> mainCh in MainCharacters)
        for (int i = 0; i < count; i++)
        {
            
            MainCharacters.TryGetValue(i, out MainCharacter mainCh);

            

            if (mainCh.GetHP() <= 0 && MainCharactersAlive.ContainsKey(mainCh.GetID()))
            {

                MainCharactersAlive.Remove(mainCh.GetID());

                foreach (KeyValuePair<int, MainCharacter> main in MainCharactersAlive)
                {
                    if (MainChOrder.Contains(main.Value))
                        HelpDiccionary.Add(main.Key, main.Value);

                }
                MainChOrder.Clear();
                SubOrder(HelpDiccionary, MainChOrder);

            }
            else if(mainCh.GetHP()> 0 && MainCharactersAlive.ContainsKey(mainCh.GetID()) == false)
            {
                print("intenta revivir");
                
                MainCharactersAlive.Add(mainCh.GetID(),mainCh);

                //foreach (KeyValuePair<int, MainCharacter> main in MainCharactersAlive)
                //{
                //    if (MainChOrder.Contains(mainCh.Value))
                //        HelpDiccionary.Add(mainCh.Key, mainCh.Value);

                //}
                //MainChOrder.Clear();
                //SubOrder(HelpDiccionary, MainChOrder);

            }

        }

        print("enemigos vivos" + EnemyCharacters.Count);

        count = EnemyCharacters.Count;

        for (int i = 0; i < count; i++)
        {
            EnemyCharacters.TryGetValue(i, out EnemyCharacter enemy);
            if (enemy.GetHP() <= 0 && MainCharactersAlive.ContainsKey(enemy.GetID()))
            {

                EnemyCharacters.Remove(enemy.GetID());
                

                if (EnemyCharacters.Count <= 0)
                {

                    foreach (KeyValuePair<int, EnemyCharacter> main in EnemyCharacters)
                    {
                        if (EnemyOrder.Contains(main.Value))
                            HelpDiccionary.Add(main.Key, main.Value);

                    }
                    EnemyOrder.Clear();
                    SubOrder(HelpDiccionary, EnemyOrder);

                    SubOrder(HelpDiccionary, EnemyOrder);

                }

            }
        }

        


        //SOLUCIONAR EL IT
        //if (it.GetHP() <= 0)
        //{

        //    if (rival == "Main")
        //    {

        //        MainCharactersAlive.Remove(i);


        //        foreach (KeyValuePair<int, MainCharacter> mainCh in MainCharactersAlive)
        //        {
        //            if (MainChOrder.Contains(mainCh.Value))
        //                HelpDiccionary.Add(mainCh.Key, mainCh.Value);

        //        }
        //        MainChOrder.Clear();

        //        SubOrder(HelpDiccionary, MainChOrder);

        //    }

        //    else
        //        EnemyCharacters.Remove(i);
        //}

        
        
        if (MainCharactersAlive.Count == 0)
        {

            state = BattleState.LOST;

            EndBattle();

        }
        else if (EnemyCharacters.Count == 0)
        {

            state = BattleState.WON;

            EndBattle();

        }

        else
        {
            
            state = BattleState.ORDER;

            Order();

        }

    }

    void EndBattle()
    {

        if(state == BattleState.WON)
        {

            print("ganaste");
            Destroy(enemyInWorld);
            ResetValues();
            inputManager.SwichActionMap(ActionMaps.MoveOut);
            gameManager.Change2NavigationMode();

        }

        if (state == BattleState.LOST)
        {

            print("perdiste");

        }

    }

    void ResetValues()
    {
        order.Clear();
        EnemyOrder.Clear();
        MainChOrder.Clear();
        MainCharactersAlive.Clear();
        EnemyCharacters.Clear();
        MainCharacters.Clear();

        enemyInWorld= null;

    }

}
