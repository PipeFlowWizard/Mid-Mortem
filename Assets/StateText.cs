using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StateText : MonoBehaviour
{
    private Enemy _enemy;
    private GameObject cam;
    public TextMeshPro text;
    // Start is called before the first frame update

    void Start()
    {
        cam = GameObject.FindWithTag("MainCamera");
        _enemy = GetComponentInParent<Enemy>();
        text = GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
            transform.LookAt(-cam.transform.position);
            
            text.text = _enemy.StateMachine.CurrentState.ToString();
    }
}
