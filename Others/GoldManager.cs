using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GoldManager : MonoBehaviour
{
    [SerializeField] private int currentGold=300;
    [SerializeField] private UnitysManager unitysManager;
    [SerializeField] private Dictionary<int,bool> purchaseList=new Dictionary<int, bool>();
    [SerializeField] private int goldGeneratorPrice=200;
    [SerializeField] private GoldGenerator infiniteGenerator;
    [SerializeField] private GoldGenerator[] goldGeneratos;

    public Action<GoldGenerator,int> OnActiveNewGenerator;

    public Action<GoldGenerator,int> OnDisableGenerator;

    [SerializeField]private int generatorIndex = -1;

    public Action OnUpdateGold;

    private void Awake()
    {
        //List<Unitys> temp = unitysManager.GetUnitys();

        //foreach(Unitys u in temp)
        //{
        //    bool canBuy = currentGold > u.cost;

        //    purchaseList.Add(u.cost, canBuy);
        //}
    }

    private void Start()
    {
        OnUpdateGold?.Invoke();
    }

    public void TryBuyUnity(GameObject prefabRef)
    {
        if (prefabRef == null)
            return;

        int cost;

        if(unitysManager.SpawnUnity(prefabRef,currentGold,out cost)==true)
        {
            currentGold -= cost;

            OnUpdateGold?.Invoke();
        }
        else
        {
            print("Can´t buy");
        }


    }

    public void TryBuyUnity(GameObject prefabRef, out bool sucess)
    {
        if (prefabRef == null)
        {
            sucess = false;
            return;
        }

        int cost;

        if (unitysManager.SpawnUnity(prefabRef, currentGold, out cost) == true)
        {
            currentGold -= cost;

            OnUpdateGold?.Invoke();

            sucess = true;
        }
        else
        {
            print("Can´t buy");

            sucess = false;
        }


    }

    public void TryBuyUnity(GameObject prefabRef, out bool sucess, out GameObject spawnedObj)
    {
        if (prefabRef == null)
        {
            spawnedObj = null;
            sucess = false;
            return;
        }

        int cost;

        GameObject objRef;

        if (unitysManager.SpawnUnity(prefabRef, currentGold, out cost, out objRef) == true)
        {
            currentGold -= cost;

            OnUpdateGold?.Invoke();

            spawnedObj = objRef;

            sucess = true;
        }
        else
        {
            //print("Can´t buy");

            spawnedObj = null;
            sucess = false;
        }


    }

    public void TryBuyGoldGenerator()
    {
        if (generatorIndex+1 < goldGeneratos.Length)
        {
            if (currentGold >= goldGeneratorPrice)
            {
                generatorIndex++;

                int id;
                goldGeneratos[generatorIndex].StartGenerator(out id);

                OnActiveNewGenerator?.Invoke(goldGeneratos[generatorIndex],id);

                currentGold -= goldGeneratorPrice;

                OnUpdateGold?.Invoke();
            }
        }
    }

    public void TryBuyGoldGenerator(out bool sucess)
    {
        if (generatorIndex + 1 < goldGeneratos.Length)
        {
            if (currentGold >= goldGeneratorPrice)
            {
                generatorIndex++;

                int id;

                goldGeneratos[generatorIndex].StartGenerator(out id);

                OnActiveNewGenerator?.Invoke(goldGeneratos[generatorIndex],id);

                currentGold -= goldGeneratorPrice;

                OnUpdateGold?.Invoke();

                sucess = true;
            }
            else
            {
                sucess = false;
            }
        }
        else
        {
            sucess = false;
        }

       
    }

    public void DecreaseGeneratorIndex(GoldGenerator goldGenerator, int id)
    {
        OnDisableGenerator?.Invoke(goldGenerator,id);
        generatorIndex--;
    }

    public void AddGold(int value)
    {        
        currentGold += value;

        OnUpdateGold?.Invoke();
    }

    public int GetCurrentGold()
    {
        return currentGold;
    }

    public int GetGoldGeneratorAmount()
    {
        return generatorIndex;
    }


}
