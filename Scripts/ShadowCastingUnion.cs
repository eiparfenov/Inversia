using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowCastingUnion : MonoBehaviour
{
    [SerializeField] private Transform lightSource;
    private List<ShadowCastingObject> _shadowCastingObjects;
    private GameObject _shadowCollidersParent;

    public void Create()
    {
        if (!lightSource)
        {
            Debug.LogWarning("Add light source to " + gameObject.name + " shadow casting union!!!");
            return;
        }
        
        if (_shadowCollidersParent)
            DestroyImmediate(_shadowCollidersParent);

        _shadowCollidersParent = new GameObject("Shadow colliders for " + gameObject.name);
        
        _shadowCastingObjects = new List<ShadowCastingObject>();
        for (int childNumber = 0; childNumber < transform.childCount; childNumber++)
        {
            Transform child = transform.GetChild(childNumber);
            
            ShadowCastingObject childShadowCastingObject = child.GetComponent<ShadowCastingObject>();
            if (!childShadowCastingObject)
                child.gameObject.AddComponent<ShadowCastingObject>();
            _shadowCastingObjects.Add(childShadowCastingObject);
            
            GameObject shadowColliderGameObject = new GameObject("Shadow(" + childNumber + ")");
            shadowColliderGameObject.transform.parent = _shadowCollidersParent.transform;
            ShadowCollider shadowCollider = shadowColliderGameObject.AddComponent<ShadowCollider>();
            
            childShadowCastingObject.AppliedShadowCollider = shadowCollider;
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
