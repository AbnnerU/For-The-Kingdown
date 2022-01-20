using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SimpleMovementY : MonoBehaviour
{
    [SerializeField] private Vector3 startPoint;

    [SerializeField] private Vector3 endPoint;

    [SerializeField] private float duration;

    private Transform _transform;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    public void StartMove()
    {
        StartCoroutine(MoveY());
    }

    IEnumerator MoveY()
    {
       
        Vector3 difference = endPoint - startPoint;

        Vector3 addValue = difference / duration;

        float currentTime = 0;


        _transform.localPosition = startPoint;

        do
        {
            currentTime += Time.deltaTime;

            _transform.localPosition += addValue * Time.deltaTime;

            yield return null;

        } while (currentTime < duration);

        _transform.localPosition = endPoint;

        yield break;
    }


}
