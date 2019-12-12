using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
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

    // Public because used by editor
    public float viewRadius;
    [Range(0, 360)] public float viewAngle;
    public List<Transform> visibleTargets = new List<Transform>();

    [SerializeField] private float _viewDelay = 1;              // How long the player must be in view to be seen

    [SerializeField] private LayerMask _targetMask = default;         // A layer of the things the object can react to
    [SerializeField] private LayerMask _obstacleMask = default;       // A layer of things blocking the vision

    [SerializeField] private MeshFilter _viewMeshFilter = default;    // Holds a mesh we'll create later on
    private float _meshResolution = 1f;                     // Higher = more triangles for the mesh
    private int _edgeResolveIterations = 1;                 // The accuracy when finding edges
    private float _edgeDistTreshold = 0.5f;  // The distance between two points when looking for an edge. Ensures they're both on the same object, as opposed to one in the background
    private Mesh _fowMesh;                                  // The mesh we're creating for the field of view
    private Teacher _teacher;
    private Rigidbody _r;

    
   
    private void Awake()
    {
        _teacher = GetComponent<Teacher>();
        //if (_patrol == null) Debug.LogError("Couldn't find patrol");
        _fowMesh = new Mesh(); // Continuously updated in LateUpdate
        _viewMeshFilter.mesh = _fowMesh;
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
            FindTargets();
        }
    }

    private void FindTargets() // Will do more with this one later to decide various behaviours
    {
        visibleTargets.Clear();       // Reset the list of targets for every check 
        // All targets within a sphere around the object
        Collider[] targetsInRadius = Physics.OverlapSphere(transform.position, viewRadius, _targetMask);

        if (targetsInRadius.Length == 0 && _teacher != null && _teacher.target != null)
            _teacher.SetTarget();


        // For every target in the sphere (used as ears)
        for (int i = 0; i < targetsInRadius.Length; i++)
        {
            Transform target = targetsInRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            // If it's anywhere within the radius, rather than just the field of view, then this is run
            if (target.tag == "Vial" && _teacher && target.gameObject.GetComponent<TriggeredVial>().HasDetonated())
            {
                _teacher.SetTarget(target);
                return;
            }
            // Check if the target is within the field of view
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
                // Check that there are no obstacles between them
                if (!Physics.Raycast(transform.position, dirToTarget, Vector3.Distance(transform.position, target.position), _obstacleMask))
                {
                    visibleTargets.Add(target);
                    if( _teacher)       // Some weird bug was that even when I assigned patrol it wouldn't accept it until it had failed once. And somehow it works even if I never assign it. Some sorcery going on
                        _teacher.SetTarget(visibleTargets);
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
