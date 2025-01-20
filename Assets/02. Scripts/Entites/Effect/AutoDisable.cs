using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDisable : MonoBehaviour
{
    [SerializeField] private float _disableTime = 1.0f;
    
    private void OnEnable()
    {
        Invoke("SetActiveFalse", _disableTime);
    }

    private void SetActiveFalse()
    {
        gameObject.SetActive(false);
    }
}
