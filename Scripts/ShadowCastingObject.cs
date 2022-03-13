using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShadowCastingObject : MonoBehaviour
{
    private Vector3[] _meshOffsets;
    private Transform _lightSource;

    public Transform LightSource
    {
        get => _lightSource;
        set => _lightSource = value;
    }

    public void RecalculateMesh()
    {
        _meshOffsets = (Vector3[])GetComponent<MeshFilter>().mesh.vertices.Distinct();
    }

    public void RenderShadow()
    {
        // transforms mesh vertexes in world space
        Vector3[] meshVertexesInWorldSpace = new Vector3[_meshOffsets.Length];
        for (int i = 0; i < _meshOffsets.Length; i++)
        {
            meshVertexesInWorldSpace[i] = transform.TransformPoint(_meshOffsets[i]);
        }
        
        // recalculates shadow
        
    }
}
