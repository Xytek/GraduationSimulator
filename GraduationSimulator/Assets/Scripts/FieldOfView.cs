﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FieldOfView : MonoBehaviour
{
    // Public because used by editor
    public float viewRadius;
    [Range(0, 360)] public float viewAngle;
    public List<Transform> visibleTargets = new List<Transform>();

    [SerializeField] private float _viewDelay;              // How long the player must be in view to be seen

    [SerializeField] private LayerMask _targetMask;         // A layer of the things the object can react to
    [SerializeField] private LayerMask _obstacleMask;       // A layer of things blocking the vision

    [SerializeField] private MeshFilter _viewMeshFilter;    // Holds a mesh we'll create later on
    private float _meshResolution = 1f;                     // Higher = more triangles for the mesh
    private int _edgeResolveIterations = 4;                 // The accuracy when finding edges
    private float _edgeDistTreshold = 0.5f;  // The distance between two points when looking for an edge. Ensures they're both on the same object, as opposed to one in the background
    private Mesh _fowMesh;                                  // The mesh we're creating for the field of view
    private Patrol _patrol;
    

    private struct EdgeInfo         // Struct used when finding the edge of an obstacle
    {
        public Vector3 pointA;      // Closest point to the edge ON the obstacle
        public Vector3 pointB;      // Closest point to the edge OFF the obstacle
        public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
        {
            pointA = _pointA;
            pointB = _pointB;
        }
    }

    private struct ViewCastInfo     // Struct to hold raycast info 
    {
        public bool hit;            // Did it hit something?
        public Vector3 point;       // Point that it hit
        public float distance;      // Distance to what it hit
        public float angle;         // Angle to what it hit
        public ViewCastInfo(bool _hit, Vector3 _point, float _distance, float _angle)
        {
            hit = _hit;
            point = _point;
            distance = _distance;
            angle = _angle;
        }
    }

    private void Start()
    {
        _fowMesh = new Mesh(); // Continuously updated in LateUpdate
        _viewMeshFilter.mesh = _fowMesh;
        _patrol = GetComponent<Patrol>();

        StartCoroutine(FindTargetsWithDelay());
    }

    private void LateUpdate()
    {
        DrawFieldOfView();
    }

    private IEnumerator FindTargetsWithDelay()
    {
        while (true)
        {
            yield return new WaitForSeconds(_viewDelay);
            FindVisibleTargets();
        }
    }

    private void FindVisibleTargets() // Will do more with this one later to decide various behaviours
    {
        visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, _targetMask);
        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float distToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, _obstacleMask))
                {
                    visibleTargets.Add(target);
                    if(visibleTargets != null)
                        _patrol.ChaseTarget(visibleTargets);
                }
            }
        }
    }

    private EdgeInfo FindEdge(ViewCastInfo minViewcast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewcast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < _edgeResolveIterations; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            bool edgeDistTresholdExceeded = Mathf.Abs(minViewcast.distance - newViewCast.distance) > _edgeDistTreshold;

            if (newViewCast.hit == minViewcast.hit && !edgeDistTresholdExceeded)
            {
                minAngle = angle;
                minPoint = newViewCast.point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }

        }
        return new EdgeInfo(minPoint, maxPoint);
    }

    private void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(viewAngle * _meshResolution);
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        ViewCastInfo oldviewCast = new ViewCastInfo();
        for (int i = 0; i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);

            if (i > 0)
            {
                bool edgeDistTresholdExceeded = Mathf.Abs(oldviewCast.distance - newViewCast.distance) > _edgeDistTreshold;
                if (oldviewCast.hit != newViewCast.hit || (oldviewCast.hit && newViewCast.hit && edgeDistTresholdExceeded))
                {
                    EdgeInfo edge = FindEdge(oldviewCast, newViewCast);
                    if (edge.pointA != Vector3.zero)
                        viewPoints.Add(edge.pointA);
                    if (edge.pointB != Vector3.zero)
                        viewPoints.Add(edge.pointB);
                }
            }
            viewPoints.Add(newViewCast.point);
            oldviewCast = newViewCast;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            if (i < vertexCount - 2)
            {

                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }
        _fowMesh.Clear();
        _fowMesh.vertices = vertices;
        _fowMesh.triangles = triangles;
        _fowMesh.RecalculateNormals();
    }


    private ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirectionFromAngle(globalAngle);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, viewRadius, _obstacleMask))
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        else
            return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngle);
    }

    public Vector3 DirectionFromAngle(float angleInDegrees, bool angleIsGlobal = true)
    {
        if (!angleIsGlobal)
            angleInDegrees += transform.eulerAngles.y;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
