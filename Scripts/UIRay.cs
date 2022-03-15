using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(LineRenderer))]
public class UIRay : MonoBehaviour
{
    [SerializeField] private float maxDepth;
    [SerializeField] private GameObject dot;
    [SerializeField] private VRInputSystem inputSystem;
    private LineRenderer _lineRenderer;

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        float lenght;
        if (inputSystem.Data.pointerCurrentRaycast.distance == 0)
        {
            _lineRenderer.enabled = false;
            dot.SetActive(false);
        }
        else
        {
            lenght = inputSystem.Data.pointerCurrentRaycast.distance;

            RaycastHit hit;
            Physics.Raycast(new Ray(transform.position, transform.forward), out hit, lenght);

            Vector3 endPosition = transform.position + transform.forward * lenght;
            if (hit.collider == null)
            {

                dot.SetActive(true);
                dot.transform.position = endPosition;

                _lineRenderer.enabled = true;
                _lineRenderer.SetPosition(0, transform.position);
                _lineRenderer.SetPosition(1, endPosition);
            }
            else
            {
                _lineRenderer.enabled = false;
                dot.SetActive(false);
            }
        }
    }
}
