using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class TrajectoryPredictor : MonoBehaviour
{
    public int maxReflections = 5;
    public float maxDistance = 30f;
    public LayerMask reflectionMask;

    private LineRenderer line;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 0;
    }

    public void ShowTrajectory(Vector3 startPos, Vector3 direction)
    {
        List<Vector3> points = new List<Vector3>();
        points.Add(startPos);

        Vector3 currentDir = direction.normalized;
        Vector3 currentPos = startPos;
        float remainingDistance = maxDistance;

        for (int i = 0; i < maxReflections; i++)
        {
            if (Physics.SphereCast(currentPos, 1, currentDir, out RaycastHit hit, remainingDistance, reflectionMask))
            {
                points.Add(hit.point);

                remainingDistance -= hit.distance;
                if (remainingDistance <= 0f)
                    break;

                currentDir = Vector3.Reflect(currentDir, hit.normal);
                currentPos = hit.point + hit.normal * 0.01f; // small offset to avoid self-collision
            }
            else
            {
                // No more hit, extend remainder of line
                points.Add(currentPos + currentDir * remainingDistance);
                break;
            }
        }

        line.positionCount = points.Count;
        line.SetPositions(points.ToArray());
    }

    public void Hide()
    {
        line.positionCount = 0;
    }
}
