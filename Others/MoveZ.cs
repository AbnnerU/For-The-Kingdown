using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class OutOfMinPosition : UnityEvent<bool> { }

[System.Serializable]
public class OutOfMaxPosition : UnityEvent<bool> { }

public class MoveZ : MonoBehaviour
{
    [SerializeField] private float startPoint;

    [SerializeField] private float minValue;

    [SerializeField] private float maxValue;

    [SerializeField] private float speed;

    [SerializeField] private float direction;

    public OutOfMinPosition OnOutOfMinPosition;

    public OutOfMaxPosition OnOutOfMaxPosition;

    public bool markTrueEvent = true;

    private Transform _transform;


    private void Awake()
    {
        _transform = GetComponent<Transform>();

        _transform.position = new Vector3(_transform.position.x, _transform.position.y, startPoint);

        direction = 0;
    }



    public void Update()
    {
        if(_transform.position.z <= minValue && direction == -1f)
        {           
            _transform.position = new Vector3(_transform.position.x, _transform.position.y, minValue);

            direction = 0;

            if (markTrueEvent == true)
            {
                markTrueEvent = false;
                OnOutOfMinPosition?.Invoke(false);
            }
        }
        else if(_transform.position.z >= maxValue && direction == 1f)
        {
            _transform.position = new Vector3(_transform.position.x, _transform.position.y, maxValue);

            direction = 0;

            if (markTrueEvent == true)
            {
                markTrueEvent = false;
                OnOutOfMaxPosition?.Invoke(false);
            }
        }
        else if(direction!=0)
        {
            if (markTrueEvent == false)
            {
                markTrueEvent = true;

                OnOutOfMinPosition?.Invoke(true);
                OnOutOfMaxPosition?.Invoke(true);
            }

            _transform.position += Vector3.forward * direction * speed * Time.deltaTime;
        }

        
    }


    public void ChangeDirection(float newDirection)
    {
        direction = newDirection;
    }

    public void DirectionZero()
    {
        direction = 0;
    }
}
