using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Interactable : MonoBehaviour
{
    [Header("Axis")]
    [SerializeField] private bool xMovementAllowed;
    [SerializeField] private Vector2 xAxisRange;
    [Space]
    [SerializeField] private bool yMovementAllowed;
    [SerializeField] private Vector2 yAxisRange;
    [Space]
    [SerializeField] private bool zMovementAllowed;
    [SerializeField] private Vector2 zAxisRange;

    [Header("Rotation")]
    [SerializeField] private bool rotationAllowed;


    private Rigidbody _rb;
    public void onVRDragStart()
    {
        _rb.isKinematic = false;
    }
    public void onVRDrag()
    {
        Vector3 pos = transform.localPosition;
        if (xMovementAllowed)
            pos.x = Mathf.Clamp(pos.x, xAxisRange.x, xAxisRange.y);
        if (yMovementAllowed)
            pos.y = Mathf.Clamp(pos.y, yAxisRange.x, yAxisRange.y);
        if (zMovementAllowed)
            pos.z = Mathf.Clamp(pos.z, zAxisRange.x, zAxisRange.y);
        transform.localPosition = pos;
    }
    public void onVRDragFinish()
    {
        _rb.isKinematic = true;
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;
        _rb.isKinematic = true;
        
        if (!xMovementAllowed)
            _rb.constraints |= RigidbodyConstraints.FreezePositionX;
        if (!yMovementAllowed)
            _rb.constraints |= RigidbodyConstraints.FreezePositionY;
        if (!zMovementAllowed)
            _rb.constraints |= RigidbodyConstraints.FreezePositionZ;
        if (!rotationAllowed)
            _rb.constraints |= RigidbodyConstraints.FreezeRotation;
    }
}
