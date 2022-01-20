using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UiTimer : MonoBehaviour
{
    [SerializeField] private Image image;

    public Action<UiTimer> OnPassTime;

    private float timer;

    private float eventCicleTime;

    private void Awake()
    {
        if (image == null)
            image = GetComponent<Image>();
    }

    public void StartTimerWhitEvent(float newTimer,float eventTime)
    {
        timer = newTimer;
        eventCicleTime = eventTime;
        StartCoroutine(TimerWhitEvent());
    }


    public void StartTimer(float newTimer)
    {
        timer = newTimer;
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        float currentTimer = 0;
        float percentage = 0;
        image.fillAmount = 0;

        do
        {
            currentTimer += Time.deltaTime;

            percentage = (100 * currentTimer) / timer;

            image.fillAmount = percentage / 100;

            yield return null;

        } while (currentTimer < timer);

        image.fillAmount = 0;

        yield break;

    }

    IEnumerator TimerWhitEvent()
    {
        float currentEventTimer = 0;

        float currentTimer = 0;

        float percentage = 0;
        image.fillAmount = 0;

        do
        {
            currentEventTimer += Time.deltaTime;

            if(currentEventTimer >= eventCicleTime)
            {
              
                OnPassTime?.Invoke(this);

                currentEventTimer = 0;
            }

            currentTimer += Time.deltaTime;

            percentage = (100 * currentTimer) / timer;

            image.fillAmount = percentage / 100;


            yield return null;

        } while (currentTimer < timer);

        image.fillAmount = 0;

        yield break;

    }
}
