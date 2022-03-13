using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class ShadowCollider : MonoBehaviour
{
    private PolygonCollider2D _appliedPolygonCollider;
    private PolygonCollider2D AppliedPolygonCollider
    {
        get
        {
            if (!_appliedPolygonCollider)
                _appliedPolygonCollider = GetComponent<PolygonCollider2D>();
            return _appliedPolygonCollider;
        }
    }

    [SerializeField] private GameObject tempMarker;
    public void RecalculateShadow(Vector3[] meshVertexesPositions, Vector3 lightSourcePosition)
    {
        Vector3[] pointsProjections = ProjectPointsOnPlane(meshVertexesPositions, lightSourcePosition);
        Vector3[] shadowPoints = ConvexHull(pointsProjections);
        if (shadowPoints.Length >= 3)
        {
            UpdateCollider(shadowPoints);
            TempDisplay(shadowPoints);
        }
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
            currentPointParallelProjection -= lightSourceParallelProjection;

            pointsOnPlane[i] = lightSourceParallelProjection + currentPointParallelProjection *
                (lightSourceLocalPosition.z / (lightSourceLocalPosition.z - meshVertexesLocalPositions[i].z));
        }

        return pointsOnPlane;
    }
    private Vector3[] ConvexHull(Vector3[] allPoints)
        {
            // Move left point to the top of the array
            for (int i = 1; i < allPoints.Length; i++)
            {
                if (allPoints[i].x > allPoints[0].x)
                {
                    (allPoints[0], allPoints[i]) = (allPoints[i], allPoints[0]);
                }
            }
            
            // Create convex-hull data and sort points by angle
            PairForConvexHull[] pointsInfo = new PairForConvexHull[allPoints.Length - 1];
            for (int i = 1; i < allPoints.Length; i++)
            {
                float angle = Vector3.SignedAngle(Vector3.up, allPoints[i] - allPoints[0], Vector3.forward);
                pointsInfo[i - 1] = new PairForConvexHull(angle, allPoints[i]);
            }
            Array.Sort(pointsInfo);
            
            // Create and fill queue and list with points
            Queue<Vector3> points = new Queue<Vector3>();
            List<Vector3> result = new List<Vector3>();

            result.Add(allPoints[0]);
            result.Add(pointsInfo[0].Point);

            for (int i = 1; i < pointsInfo.Length; i++)
            {
                points.Enqueue(pointsInfo[i].Point);
            }
            
            // Convex-hull algorithm
            Vector3 nextPoint = points.Peek();
            while(result.Count > 1)
            {
                float angle = Vector3.SignedAngle(result[result.Count - 1] - result[result.Count - 2], nextPoint - result[result.Count - 1], Vector3.forward);
                if(angle < 0f)
                {
                    result.Add(nextPoint);
                    if (points.Count != 0)
                        nextPoint = points.Dequeue();
                    else
                        break;
                }
                else
                {
                    result.RemoveAt(result.Count - 1);
                }
            }
            
            return result.ToArray();

        }
    private void UpdateCollider(Vector3[] points)
    {
        Vector2[] path = new Vector2[points.Length];
        for (int i = 0; i < path.Length; i++)
        {
            path[i] = points[i];
        }

        AppliedPolygonCollider.SetPath(0, path);
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
class PairForConvexHull: IComparable
{
    private float angle;
    private Vector3 point;

    public PairForConvexHull(float angle, Vector3 point)
    {
        this.angle = angle;
        this.point = point;
    }

    public Vector3 Point { get => point; set => point = value; }
    public float Angle { get => angle; set => angle = value; }

    public int CompareTo(object obj)
    {
        if (((PairForConvexHull)obj).Angle < angle)
            return -1;
        else
            return 1;
    }
}
