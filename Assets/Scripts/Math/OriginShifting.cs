using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OriginShifting : MonoBehaviour
{

    [SerializeField] private float OriginThreshold;
    public Transform[] WorldGameObjs;
    private Vector3 lastPos;
    public Vector3 offset;
    public GameObject mainTarget;


    // Start is called before the first frame update
    void Start()
    {
        mainTarget = gameObject;
        lastPos = Vector3.zero;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        foreach(AeroDynamicsCore aero in GameObject.FindObjectsOfType<AeroDynamicsCore>())
        {
            aero.enabled = true;
        }

        OriginThreshold = GameObject.FindWithTag("GameStateManager").GetComponent<PhysicsSettings>().OriginThreshold;
        if (GetComponent<Rigidbody>().position.magnitude > OriginThreshold && GameObject.FindObjectOfType<GameStateManager>().Game == GameStateManager.GameState.SIMULATING)
        {
            offset = mainTarget.transform.position;
            mainTarget.transform.position -= offset;

            foreach (Transform obj in WorldGameObjs)
            {
                if(obj.transform.parent == null && obj.gameObject != gameObject)
                {
                    Debug.Log("LOL");
                    Vector3 velocity = GetComponent<Rigidbody>().velocity;
                    Vector3 velocityA = GetComponent<Rigidbody>().angularVelocity;
                    Quaternion rotation = GetComponent<Rigidbody>().rotation;
                    GetComponent<Rigidbody>().isKinematic = true;

                    obj.transform.position -= offset;
                    foreach(AeroDynamicsCore aero in GameObject.FindObjectsOfType<AeroDynamicsCore>())
                    {
                        aero.enabled = false;
                    }

                    GetComponent<Rigidbody>().velocity = velocity;
                    GetComponent<Rigidbody>().angularVelocity = velocityA;
                    GetComponent<Rigidbody>().rotation = rotation;
                    GetComponent<Rigidbody>().isKinematic = false;
                }
                
                /*if (obj.parent == null)
                {
                    obj.transform.position -= pos;
                }*/
            }
            /*if (GetComponent<Rigidbody>())
            {
                lastPos = GetComponent<Rigidbody>().position;
            }

            foreach (Transform obj in WorldGameObjs)
            {
                if (obj.GetComponent<Rigidbody>())
                {
                    Vector3 velocity = obj.GetComponent<Rigidbody>().velocity;
                    Vector3 velocityA = obj.GetComponent<Rigidbody>().angularVelocity;
                    obj.GetComponent<Rigidbody>().detectCollisions = false;
                    obj.GetComponent<Rigidbody>().position -= lastPos;
                    obj.GetComponent<Rigidbody>().velocity = velocity;
                    obj.GetComponent<Rigidbody>().angularVelocity = velocityA;
                }
                else
                {
                    if (obj.parent == null)
                        obj.position -= lastPos;

                }

            }*/
        }

    }

    public void ReloadWorldObjects()
    {
        WorldGameObjs = Transform.FindObjectsOfType<Transform>();
    }
}
