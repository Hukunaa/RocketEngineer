using System.Collections;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class AeroDynamicsCore : MonoBehaviour
{
    public GameObject m_pod;
    public GameObject m_currentPlanet;
    public AnimationCurve m_atmosphereCruve;
    public Rigidbody m_podRigidbody;
    public float TorqueThreshold;

    public Vector3d m_normal;
    public Vector3d m_finalNormal;
    private Vector3d m_finalPos;

    private Vector3d[] m_vertices;
    private Vector3d[] m_normals;
    private int[] m_trianglesIds;

    public Vector3d m_torque;
    public Vector3d m_force;

    private Vector3d velocity;
    [SerializeField] private Vector3 velocityf;
    public float m_airResistance;

    void Start()
    {
        m_normal = Vector3d.zero;
        m_finalNormal = Vector3d.zero;
        m_trianglesIds = GetComponent<MeshFilter>().mesh.triangles;
        m_force = Vector3d.zero;

        m_pod = gameObject;
        m_normals = new Vector3d[GetComponent<MeshFilter>().mesh.normals.Length];
        for (int i = 0; i < GetComponent<MeshFilter>().mesh.normals.Length; ++i)
        {
            m_normals[i] = Vector3d.FromVector3(GetComponent<MeshFilter>().mesh.normals[i]);
        }

        m_vertices = new Vector3d[GetComponent<MeshFilter>().mesh.vertices.Length];
        for (int i = 0; i < GetComponent<MeshFilter>().mesh.vertices.Length; ++i)
        {
            m_vertices[i] = Vector3d.FromVector3(GetComponent<MeshFilter>().mesh.vertices[i]);
        }
        CalculateAerodynamics();

    }

    void Update()
    {
        float m_distance;
        //TEMPORARY
        if (m_currentPlanet != null)
        {
            m_distance = Mathf.Clamp01(m_pod.GetComponent<Pod>().m_altitude / m_currentPlanet.GetComponent<GravityCore>().AtmosphereThickness);
            m_airResistance = m_atmosphereCruve.Evaluate(m_distance);
        }
        else if (m_currentPlanet == null)
        {
            m_currentPlanet = GameObject.FindWithTag("Planet");
            m_airResistance = 0;
        }
        //--------------------

        if (!m_pod.GetComponent<Rigidbody>())
        {
            if (m_pod.transform.parent != null)
                m_pod = m_pod.transform.parent.gameObject;
        }
        else
        {
            m_podRigidbody = m_pod.GetComponent<Rigidbody>();
        }

        if (m_podRigidbody != null)
        {
            velocity = Vector3d.FromVector3(m_podRigidbody.velocity);
        }



        CalculateAerodynamics();
        m_finalNormal = Vector3d.FromVector3(transform.rotation * Vector3d.FromVector3d(m_normal));
        //m_force = m_finalNormal.normalized * ((velocity.magnitude * velocity.magnitude) * m_airResistance);
        m_force = ((m_finalNormal.normalized * ((velocity.magnitude * velocity.magnitude)) / 2) * m_airResistance) / 1000;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(transform.position, Vector3d.FromVector3d(velocity));
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, Vector3d.FromVector3d(m_force * 10));
    }
    void FixedUpdate()
    {

        if (Input.GetKey(KeyCode.G))
        {
            m_podRigidbody.AddForce(Vector3d.FromVector3d(Vector3d.forward) * 10000);
        }
        if (m_podRigidbody != null)
            m_podRigidbody.AddForceAtPosition(-Vector3d.FromVector3d(m_force), transform.TransformPoint(Vector3d.FromVector3d(m_finalPos)));

    }

    public void FindPod()
    {
        m_pod = gameObject;
    }
    void CalculateAerodynamics()
    {
        int j = 0;
        m_normal = Vector3d.zero;
        if (velocity.magnitude > 0.1f)
        {
            for (int i = 0; i < m_trianglesIds.Length / 3; ++i)
            {
                if (Vector3d.Dot(Vector3d.FromVector3(transform.rotation * Vector3d.FromVector3d(m_normals[m_trianglesIds[i * 3]])), velocity) > 0 && velocity.magnitude > 0.1f)
                {
                    double a = Vector3d.Distance(m_vertices[m_trianglesIds[i * 3]], m_vertices[m_trianglesIds[(i * 3) + 1]]);
                    double b = Vector3d.Distance(m_vertices[m_trianglesIds[(i * 3) + 1]], m_vertices[m_trianglesIds[(i * 3) + 2]]);
                    double c = Vector3d.Distance(m_vertices[m_trianglesIds[(i * 3) + 2]], m_vertices[m_trianglesIds[i * 3]]);

                    double s = (a + b + c) / 2;

                    double area = Mathf.Sqrt((float)(s * (s - a) * (s - b) * (s - c)));

                    Vector3d centralPoint = new Vector3d(
                    ((m_vertices[m_trianglesIds[i * 3]].x + m_vertices[m_trianglesIds[(i * 3) + 1]].x + m_vertices[m_trianglesIds[(i * 3) + 2]].x)) / 3,
                    ((m_vertices[m_trianglesIds[i * 3]].y + m_vertices[m_trianglesIds[(i * 3) + 1]].y + m_vertices[m_trianglesIds[(i * 3) + 2]].y)) / 3,
                    ((m_vertices[m_trianglesIds[i * 3]].z + m_vertices[m_trianglesIds[(i * 3) + 1]].z + m_vertices[m_trianglesIds[(i * 3) + 2]].z)) / 3);

                    m_finalPos += centralPoint;
                    m_normal += m_normals[m_trianglesIds[i * 3]] * (0.5f + area) * 2;
                    Debug.DrawRay(transform.TransformPoint(Vector3d.FromVector3d(centralPoint)), transform.rotation * Vector3d.FromVector3d(m_normals[m_trianglesIds[i * 3]]) * (float)(0.5 + area), Color.red);

                    ++j;
                }
            }
            m_normal /= j;
            m_finalPos /= j;
        }
    }

    /*void OnTriggerEnter(Collider col)
    {
        if (col.transform.parent.CompareTag("Planet"))
        {
            m_atmospherePoint = transform.position;
            m_atmosphere = col.gameObject;
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (m_atmosphere == col.gameObject)
            m_atmosphere = null;
    }*/
}