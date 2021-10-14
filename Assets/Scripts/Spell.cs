using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private float damage;
    [SerializeField] private float lifetime = 10.0f;

    private float _spawnTime;


    private void Start()
    {
        _spawnTime = Time.time;
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }
    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        if (Time.time > _spawnTime + lifetime)
        {
            if (gameObject) Destroy(gameObject);
        }
    }

    // Todo: replace with raycasting
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            // Apply damage
            Destroy(other.gameObject);
        }
    }
}
