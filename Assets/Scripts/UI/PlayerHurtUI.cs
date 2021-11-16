using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurtUI : MonoBehaviour
{
    [SerializeField] private float duration;
    private float activeTime;

    private void OnEnable()
    {
        activeTime = 0;
    }

    private void Update()
    {
        if (activeTime < duration)
            activeTime += Time.deltaTime;
        else
            gameObject.SetActive(false);
    }
}
