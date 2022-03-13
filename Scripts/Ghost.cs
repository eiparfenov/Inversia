using System;
using UnityEngine;


public class Ghost : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody2D _rb;
    private float _horizontal;
    
    public void Update()
    {
        //temp
        _horizontal = Input.GetAxis("Horizontal");
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _rb.velocity = new Vector2(_horizontal * speed, _rb.velocity.y);
    }
}