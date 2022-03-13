using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class ShadowCastingObject : MonoBehaviour
{
    [SerializeField] [HideInInspector] private Vector3[] meshOffsets;
    [SerializeField] [HideInInspector] private Transform lightSource;
    [SerializeField] [HideInInspector] private ShadowCollider appliedShadowCollider;

    public ShadowCollider AppliedShadowCollider
    {
        get => appliedShadowCollider;
        set => appliedShadowCollider = value;
    }
    public Transform LightSource
    {
        get => lightSource;
        set => lightSource = value;
    }

    public void RecalculateMesh()
    {
        meshOffsets = GetComponent<MeshFilter>().sharedMesh.vertices.Distinct().ToArray();
    }

    public void RenderShadow()
    {
        // transforms mesh vertexes in world space
        Vector3[] meshVertexesInWorldSpace = new Vector3[meshOffsets.Length];
        for (int i = 0; i < meshOffsets.Length; i++)
        {
            meshVertexesInWorldSpace[i] = transform.TransformPoint(meshOffsets[i]);
        }

        // recalculates shadow
        appliedShadowCollider.RecalculateShadow(meshVertexesInWorldSpace, lightSource.position);
    }
}
