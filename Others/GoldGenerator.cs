using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class OnAction : UnityEvent { }



public class GoldGenerator : MonoBehaviour
{
    [SerializeField] private GoldManager goldManager;

    [SerializeField] private int addGoldValue;

    [SerializeField] private float goldInterval=1f;

    [SerializeField] private int generationCicles=3;

    [SerializeField] private bool infiniteDuration=false;

    [SerializeField] private bool infiniteGerationOnStart = false;

    public OnAction OnGold;

    private int id;

    public void Start()
    {
        if (infiniteGerationOnStart)
            StartCoroutine(InfiniteGenerator());
    }

    public void StartGenerator(out int executionId)
    {
        executionId = id++;
        if (infiniteDuration)
            StartCoroutine(InfiniteGenerator());
        else
            StartCoroutine(Generator(executionId));

    }

    IEnumerator InfiniteGenerator()
    {
        do
        {
            yield return new WaitForSeconds(goldInterval);

            OnGold?.Invoke();

            goldManager.AddGold(addGoldValue);

        } while (true);
    }

    IEnumerator Generator(int id)
    {       
        float currentValue=0;

        do
        {
            yield return new WaitForSeconds(goldInterval);

            OnGold?.Invoke();

            goldManager.AddGold(addGoldValue);

            currentValue++;


        } while (currentValue < generationCicles);

        goldManager.DecreaseGeneratorIndex(this,id);

        id--;

        yield break;

    }

    public float GetGoldInterval()
    {
        return goldInterval;
    }

    public float GetTotalTimer()
    {
        return goldInterval * generationCicles;
    }
}
