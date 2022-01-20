using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AiAtack: MonoBehaviour
{
    public Action OnAtack;

    public Action OnExecuteAtack;
}


public interface IAtackAI
{
    void DoAtack();

    bool GetCanAtack();

    void CanAtack();
   
}

public enum AtackType
{
    SINGLE,
    AREA
}