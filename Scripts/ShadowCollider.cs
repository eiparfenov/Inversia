using UnityEngine;


public class ShadowCollider : MonoBehaviour
{
    [SerializeField] private GameObject tempMarker;
    public void RecalculateShadow(Vector3[] meshVertexesPositions, Vector3 lightSourcePosition)
    {
        Vector3[] pointsProjections = ProjectPointsOnPlane(meshVertexesPositions, lightSourcePosition);
        TempDisplay(pointsProjections);
    }

    private Vector3[] ProjectPointsOnPlane(Vector3[] meshVertexesPositions, Vector3 lightSourcePosition)
    {
        Vector3[] meshVertexesLocalPositions = new Vector3[meshVertexesPositions.Length];
        for (int i = 0; i < meshVertexesPositions.Length; i++)
        {
            meshVertexesLocalPositions[i] = transform.InverseTransformPoint(meshVertexesPositions[i]);
        }

        Vector3 lightSourceLocalPosition = transform.InverseTransformPoint(lightSourcePosition);


        Vector3[] pointsOnPlane = new Vector3[meshVertexesPositions.Length];

        Vector3 lightSourceParallelProjection = lightSourceLocalPosition;
        lightSourceParallelProjection.z = 0;

        for (int i = 0; i < meshVertexesLocalPositions.Length; i++)
        {
            Vector3 currentPoint = meshVertexesLocalPositions[i];
            Vector3 currentPointParallelProjection = currentPoint;
            currentPointParallelProjection.z = 0;

            pointsOnPlane[i] = lightSourceParallelProjection + currentPointParallelProjection *
                (lightSourceLocalPosition.z / (lightSourceLocalPosition.z - meshVertexesLocalPositions[i].z));
        }

        return pointsOnPlane;
    }

    private void TempDisplay(Vector3[] pointsToDisplay)
    {
        for (int childNumber = 0; childNumber < transform.childCount; childNumber++)
        {
            Destroy(transform.GetChild(childNumber).gameObject);
        }

        foreach (var point in pointsToDisplay)
        {
            Instantiate(tempMarker, point, Quaternion.identity, transform);
        }
    }
    
}
