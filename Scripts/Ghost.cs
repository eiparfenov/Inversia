using System;
using UnityEngine;
using UnityEngine.Events;


public class Ghost : MonoBehaviour
{
    public UnityEvent onGhostFall = new UnityEvent();
    [SerializeField] private float speed;
    private Rigidbody2D _rb;
    private float _horizontal;
    
    public void SetHorizontalAxis(float axis)
    {
        _horizontal = axis;
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _rb.velocity = new Vector2(_horizontal * speed, _rb.velocity.y);
    }
    private void Update()
    {
        if (transform.position.y < -5f)
            onGhostFall.Invoke();
    }
    private void OnDestroy()
    {
        onGhostFall.RemoveAllListeners();
    }
}
