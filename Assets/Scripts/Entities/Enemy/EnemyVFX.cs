using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyVFX : MonoBehaviour
{
    [SerializeField] private Material _material;

    // private Material _material; // Reference to Enemy Material
    private Color enemyColor; // Original Enemy Color
    private Enemy _enemy;
    [SerializeField] Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _enemy = GetComponent<Enemy>();
        if(!_material)
            _material = GetComponentInChildren<MeshRenderer>().material;
        enemyColor = _material.GetColor("_LightColor");
    }

    private void FixedUpdate()
    {
        /*// Get Health of Enemy to determine color
        SetEnemyHealthState();*/
    }

    public void SetEnemyHealthState()
    {
        // If maxHealth is less than 50 but greater than 25, the Enemy turns yellow
        // Call CurrentHealthState
        if (_enemy.CurrentHealthState() == 1)
        {
            Color yellow = Color.yellow;
            yellow.a = 0.5f;
            _material.SetColor("_LightColor",yellow);
        }
        // If maxHealth is less than 25, Enemy turns red
        else if (_enemy.CurrentHealthState() == 2)
        {
      
            Color red = Color.red;
            red.a = 0.5f;
            _material.SetColor("_LightColor",red);
        
        }

    }



    public void ChangeColor()
    {
        _material.color = new Color(UnityEngine.Random.Range(0.0f,1.0f),UnityEngine.Random.Range(0.0f,1.0f),UnityEngine.Random.Range(0.0f,1.0f),1);
    }
}
