using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScale : MonoBehaviour
{
    [SerializeField] private Vector2 startWidthHeigth;
    [SerializeField] private Vector2 endWidthHeigth;
    [SerializeField] private float duration;

    [SerializeField] private bool disableObjectOnEnd = true;

    private RectTransform rt;

    private Coroutine coroutine;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

    public void StartScale()
    {
        gameObject.SetActive(true);

        if (coroutine != null)
            StopCoroutine(coroutine);

        coroutine = StartCoroutine(Scale());
    }

    IEnumerator Scale()
    {
        Vector2 difference = endWidthHeigth - startWidthHeigth;

        Vector2 addValue = difference / duration;

        float currentTime = 0;

        float currentWidth = startWidthHeigth.x;

        float currentHeigth = startWidthHeigth.y;


        SetSize(rt, currentWidth, currentHeigth);

        do
        {
            currentTime += Time.deltaTime;

            currentWidth += addValue.x * Time.deltaTime;
            currentHeigth += addValue.x * Time.deltaTime;

            SetSize(rt, currentWidth,currentHeigth);

            yield return null;

        } while (currentTime < duration);

        SetSize(rt, endWidthHeigth.x,endWidthHeigth.y);

        if (disableObjectOnEnd)
            gameObject.SetActive(false);

        yield break;

    }

    public void SetSize(RectTransform rt, float width, float height)
    {
        rt.sizeDelta = new Vector2(width, height);
    }
}
