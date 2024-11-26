using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacter : MonoBehaviour
{
    public BaseCharacter targetCharacter;
    public GameObject targetObject;

    public bool isPlayerCharacter = false;
    public bool isFightWithTarget = false;

    //public Action 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleAttackDelay()
    {
        if (isFightWithTarget)
        {

        }

        OnMoveEvent();
    }

    public void OnMoveEvent()
    {

    }

    public void OnAttackEvent()
    {

    }

    public void SetTarget()
    {

    }

    public void FindTarget()
    {

    }
}
