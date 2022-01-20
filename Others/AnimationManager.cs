using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    [SerializeField] private Animator anim;

    [SerializeField] private bool useMoveAnimation;

    [SerializeField] private float minVelocity = 0.01f;

    [SerializeField] private HealthManager healthManager;

    [SerializeField] private AiAtack aiAtack;

    [SerializeField] private AIGeneral aIGeneral;

    private Transform _tranform;

    private  Vector3 lastPosition;

    private bool active = true;

    private void Awake()
    {
        if(aiAtack)
        aiAtack.OnAtack += AiAtack_OnAtack;
        if(aIGeneral)
        aIGeneral.OnAction += Action;

        _tranform = GetComponent<Transform>();
    }

    private void OnDisable()
    {
        active = false;
    }

    private void OnEnable()
    {
        active = true;

        lastPosition = _tranform.position;
    }

    private void Update()
    {
        if (useMoveAnimation == false)
            return;

        if (active)
        {
            
            if((_tranform.position - lastPosition).magnitude > minVelocity)
            {
                anim.SetFloat("Walk Blend", 1);
            }
            else
            {
               
                anim.SetFloat("Walk Blend", 0);
            }
            lastPosition = _tranform.position;
        }
    }

    private void AiAtack_OnAtack()
    {
        anim.SetBool("Atack", true);
    }

    private void Action()
    {
        anim.SetBool("Action", true);
    }

    public void ReleaseObject()
    {
        healthManager.ReleaseObject();
    }


    public void SetDeath(bool value)
    {
        anim.SetBool("Death", value);
    }

    public void SetAtackFalse()
    {
        anim.SetBool("Atack", false);
    }

    public void SetActionFalse()
    {
        anim.SetBool("Action", false);
    }
}
