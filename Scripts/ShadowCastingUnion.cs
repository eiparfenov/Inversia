using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ShadowCastingUnion : MonoBehaviour
{
    [SerializeField] private Transform lightSource;
    public Transform LightSource
    {
        get => lightSource;
        set => lightSource = value;
    }
    [SerializeField] [HideInInspector] private List<ShadowCastingObject> shadowCastingObjects;
    [SerializeField] [HideInInspector] private GameObject shadowCollidersParent;

    public void Create()
    {
        if (!lightSource)
        {
            Debug.LogWarning("Add light source to " + gameObject.name + " shadow casting union!!!");
            return;
        }
        
        if (shadowCollidersParent)
            DestroyImmediate(shadowCollidersParent);

        shadowCollidersParent = new GameObject("Shadow colliders for " + gameObject.name);
        shadowCollidersParent.transform.parent = transform.parent;
        
        shadowCastingObjects = new List<ShadowCastingObject>();
        for (int childNumber = 0; childNumber < transform.childCount; childNumber++)
        {
            Transform child = transform.GetChild(childNumber);
            
            ShadowCastingObject childShadowCastingObject = child.GetComponent<ShadowCastingObject>();
            if (!childShadowCastingObject)
                childShadowCastingObject = child.gameObject.AddComponent<ShadowCastingObject>();
            shadowCastingObjects.Add(childShadowCastingObject);
            
            GameObject shadowColliderGameObject = new GameObject("Shadow(" + childNumber + ")");
            shadowColliderGameObject.transform.parent = shadowCollidersParent.transform;
            ShadowCollider shadowCollider = shadowColliderGameObject.AddComponent<ShadowCollider>();
            
            childShadowCastingObject.AppliedShadowCollider = shadowCollider;
            childShadowCastingObject.LightSource = lightSource;
            childShadowCastingObject.RecalculateMesh();
        }
    }

    public void RenderShadows()
    {
        foreach (var shadowCastingObject in shadowCastingObjects)
        {
            shadowCastingObject.RenderShadow();
        }
    }
    
    private void Update()
    {
        RenderShadows();
    }
}
