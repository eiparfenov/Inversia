using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowCastingUnion : MonoBehaviour
{
    [SerializeField] private Transform lightSource;
    private List<ShadowCastingObject> _shadowCastingObjects;

    public void Create()
    {
        if (!lightSource)
        {
            Debug.LogWarning("Add light source to " + gameObject.name + " shadow casting union!!!");
            return;
        }
        
        _shadowCastingObjects = new List<ShadowCastingObject>();
        for (int childNumber = 0; childNumber < transform.childCount; childNumber++)
        {
            Transform child = transform.GetChild(childNumber);
            
            ShadowCastingObject childShadowCastingObject = child.GetComponent<ShadowCastingObject>();
            if (!childShadowCastingObject)
                child.gameObject.AddComponent<ShadowCastingObject>();
            _shadowCastingObjects.Add(childShadowCastingObject);
            
            childShadowCastingObject.LightSource = lightSource;
            childShadowCastingObject.RecalculateMesh();
        }
    }

    public void RenderShadows()
    {
        foreach (var shadowCastingObject in _shadowCastingObjects)
        {
            shadowCastingObject.RenderShadow();
        }
    }

    private void Start()
    {
        Create();
    }

    private void Update()
    {
        RenderShadows();
    }
}
