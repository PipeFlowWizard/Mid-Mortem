using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Rebinding : MonoBehaviour
{

    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private InputActionReference spellinput;

    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    private void Awake()
    {
        gameObject.SetActive(false);
    }


    public void StartRebinding()
    {
        //playerInput.SwitchCurrentActionMap("Menu");
        rebindingOperation = spellinput.action.PerformInteractiveRebinding().OnMatchWaitForAnother(0.1f).OnComplete(operation =>RebindComplete()).Start();
    }

    private void RebindComplete()
    {
        rebindingOperation.Dispose();
        //playerInput.SwitchCurrentActionMap("Gameplay");
    }
    
}
