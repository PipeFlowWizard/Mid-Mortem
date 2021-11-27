using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyVFX : MonoBehaviour
{

    private Material _material; // Reference to Enemy Material
    private Color enemyColor; // Original Enemy Color
    private bool flash;
    private Enemy _enemy;
    [SerializeField] Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _enemy = GetComponent<Enemy>();
        _material = GetComponentInChildren<MeshRenderer>().material;
        enemyColor = _material.color;
    }

    private void FixedUpdate()
    {
        /*// Get Health of Enemy to determine color
        SetEnemyHealthState();*/
    }

    public void SetEnemyHealthState()
    {
        // If health is less than 50 but greater than 25, the Enemy turns yellow
        // Call CurrentHealthState
        if (_enemy.CurrentHealthState() == 1)
        {
            if (flash)
            {
                _material.color = enemyColor;
            }
            else
            {
                Color yellow = Color.yellow;
                yellow.a = 0.5f;
                _material.color = yellow;
            }
        }
        // If health is less than 25, Enemy turns red
        else if (_enemy.CurrentHealthState() == 2)
        {
            if (flash)
            {
                _material.color = enemyColor;
            }
            else
            {
                Color red = Color.red;
                red.a = 0.5f;
                _material.color = red;
            }
        }

    }

    public void MeleeAttack()
    {
        _animator.Play("EnemyMelee");
    }

    public void ChangeColor()
    {
        _material.color = new Color(UnityEngine.Random.Range(0.0f,1.0f),UnityEngine.Random.Range(0.0f,1.0f),UnityEngine.Random.Range(0.0f,1.0f),1);
    }
}
