using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform target;
    private Transform _transform;

    private void Start()
    {
        _transform= GetComponent<Transform>();
        target = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    private void FixedUpdate()
    {
        _transform.rotation = Quaternion.LookRotation(target.position - _transform.position);


    }
}
