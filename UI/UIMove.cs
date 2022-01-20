using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMove : MonoBehaviour
{
    [SerializeField] private Vector2 startPoint;
    [SerializeField] private Vector2 endPoint;
    [SerializeField] private float duration;

    [SerializeField] private bool disableObjectOnEnd=true;

    [SerializeField] private bool playOnStart = false;

    private RectTransform _transform;

    private Coroutine coroutine;

    private void Awake()
    {
        _transform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        if (playOnStart)
        {
            coroutine = StartCoroutine(Move());
        }
    }

    public void StartMove()
    {
        gameObject.SetActive(true);

        if (coroutine != null)
            StopCoroutine(coroutine);

        coroutine = StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        Vector2 difference = endPoint - startPoint;

        Vector2 addValue = difference / duration;

        float currentTime = 0;

       
        _transform.anchoredPosition= startPoint;

        do
        {
            currentTime += Time.deltaTime;

            _transform.anchoredPosition += addValue * Time.deltaTime;
            
            yield return null;

        }while(currentTime < duration);

        _transform.anchoredPosition = endPoint;

        if (disableObjectOnEnd)
            gameObject.SetActive(false);

        yield break;

    }
}
