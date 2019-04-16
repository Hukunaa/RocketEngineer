using System.Collections;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class AeroDynamicsCore : MonoBehaviour
{
    public GameObject Pod;
    public Rigidbody PodRigidbody;
    public float TorqueThreshold;

    public Vector3d NORMAL;
    public Vector3d FINALNORMAL;
    private Vector3d FINALPOS;

    private Vector3d[] vertices;
    private Vector3d[] Normals;
    private int[] triangles;
    public Vector3d Torque;
    private Vector3d centerOfMass;
    private Vector3d velocity;

    Vector3d lastPos;
    Vector3d rotationForce;

    private bool foundPod;
    private double anglePower;

    public PIDController RotationPID;

    void Start()
    {
        NORMAL = Vector3d.zero;
        FINALNORMAL = Vector3d.zero;
        triangles = GetComponent<MeshFilter>().mesh.triangles;
        Torque = Vector3d.zero;
        foundPod = false;
        anglePower = 0;

        Pod = gameObject;
        Normals = new Vector3d[GetComponent<MeshFilter>().mesh.normals.Length];
        for(int i = 0; i < GetComponent<MeshFilter>().mesh.normals.Length; ++i)
        {
            Normals[i] = Vector3d.FromVector3(GetComponent<MeshFilter>().mesh.normals[i]);
        }
        
        vertices = new Vector3d[GetComponent<MeshFilter>().mesh.vertices.Length];
        for(int i = 0; i < GetComponent<MeshFilter>().mesh.vertices.Length; ++i)
        {
            vertices[i] = Vector3d.FromVector3(GetComponent<MeshFilter>().mesh.vertices[i]);
        }

        /*Debug.Log("Normals length = " + Normals.Length);
        Debug.Log("FinalPositions length = " + vertices.Length);*/

        /*foreach(Vector3d vert in vertices)
            transform.TransformPoint(vert);*/
        CalculateAerodynamics();

    }

    void Update()
    {
        if(!foundPod)
        {   for(int i = 0; i < 10; ++i)
            {
                if(!Pod.GetComponent<Rigidbody>())
                {
                    if(Pod.transform.parent != null)
                        Pod = Pod.transform.parent.gameObject;
                }
                else
                {
                    PodRigidbody = Pod.GetComponent<Rigidbody>();
                    foundPod = true;
                    break;
                }
            }
        }
        if(PodRigidbody != null)
            velocity = Vector3d.FromVector3(PodRigidbody.velocity);



        CalculateAerodynamics();
        FINALNORMAL = Vector3d.FromVector3(transform.rotation * Vector3d.FromVector3d(NORMAL));
        anglePower = Vector3d.Angle(FINALNORMAL, velocity);

        if(anglePower > 90)
            anglePower = 180 - anglePower;

        Torque = Vector3d.Cross(FINALNORMAL, velocity).normalized * RotationPID.Update((float)anglePower) * velocity.magnitude * velocity.magnitude;
        /* Debug.DrawRay(transform.position, velocity, Color.magenta);
        Debug.DrawRay(transform.position, FINALNORMAL, Color.red);
        Debug.DrawRay(transform.position, Torque.normalized, Color.blue);*/
    }

     public void OnDrawGizmos()
     {
        Gizmos.DrawRay(transform.position, Vector3d.FromVector3d(velocity));
        Gizmos.DrawRay(transform.position, Vector3d.FromVector3d(FINALNORMAL));
        Gizmos.DrawRay(transform.position, Vector3d.FromVector3d(Torque));
     }
    void FixedUpdate()
    {

        if(Input.GetKey(KeyCode.G))
        {
            PodRigidbody.AddForce(Vector3d.FromVector3d(Vector3d.forward) * 10000);
        }
        if(PodRigidbody != null)
            PodRigidbody.AddTorque(Vector3d.FromVector3d(Torque));
        // Mathf.Pow(velocity.magnitude, 2);
        /* * (1 + Vector3d.Distance(gameObject.transform.position + FINALPOS, Pod.transform.position + PodRigidbody.centerOfMass))*/

    }

    void CalculateAerodynamics()
    {
        int j = 0;
        NORMAL = Vector3d.zero;
        if(velocity.magnitude > 0.1f)
        {
            for(int i = 0; i < triangles.Length / 3; ++i)
            {
                if(Vector3d.Dot(Vector3d.FromVector3(transform.rotation * Vector3d.FromVector3d(Normals[triangles[i * 3]])), velocity) > TorqueThreshold && velocity.magnitude > 0.1f)
                {
                    double a = Vector3d.Distance(vertices[triangles[i * 3]], vertices[triangles[(i * 3) + 1]]);
                    double b = Vector3d.Distance(vertices[triangles[(i * 3) + 1]], vertices[triangles[(i * 3) + 2]]);
                    double c = Vector3d.Distance(vertices[triangles[(i * 3) + 2]], vertices[triangles[i * 3]]);

                    double s = (a + b + c) / 2;

                    double area = Mathf.Sqrt((float)(s * (s - a) * (s - b) * (s - c)));
                    
                    Vector3d centralPoint = new Vector3d(
                    ((vertices[triangles[i * 3]].x + vertices[triangles[(i * 3) + 1]].x + vertices[triangles[(i * 3) + 2]].x)) / 3, 
                    ((vertices[triangles[i * 3]].y + vertices[triangles[(i * 3) + 1]].y + vertices[triangles[(i * 3) + 2]].y)) / 3, 
                    ((vertices[triangles[i * 3]].z + vertices[triangles[(i * 3) + 1]].z + vertices[triangles[(i * 3) + 2]].z)) / 3);

                    //FINALPOS += centralPoint;
                    Debug.DrawRay(transform.TransformPoint(Vector3d.FromVector3d(centralPoint)), transform.rotation * Vector3d.FromVector3d(Normals[triangles[i * 3]]) * (float)(0.5 + area), Color.red);
                    NORMAL += Normals[triangles[i * 3]] * (0.5f + area) * 2;
                    
                    ++j;
                }
            }
            NORMAL /= j;
        }

        //FINALPOS /= (triangles.Length / 3);
    }
}