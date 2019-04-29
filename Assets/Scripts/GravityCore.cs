using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityCore : MonoBehaviour
{

    Rigidbody[] Rockets;
    public float planetMass;
    public float radius;
    public float AtmosphereThickness;
    public Vector3 finalForce;
    public GameObject currentRocket;
    public float gravityAccel;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Planet>().GeneratePlanet();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Rockets = GameObject.FindObjectsOfType<Rigidbody>();
        foreach (Rigidbody Rocket in Rockets)
        {
            currentRocket = Rocket.gameObject;
            float force =   ((6.673f * Mathf.Pow(10.0f, -11.0f) * Rocket.mass * planetMass) / Mathf.Pow(Vector3.Distance(Rocket.transform.position, transform.position) * 100, 2));
            gravityAccel =  ((6.673f * Mathf.Pow(10.0f, -11.0f) * planetMass) / Mathf.Pow(Vector3.Distance(Rocket.transform.position, transform.position) * 100, 2));
            finalForce = (transform.position - Rocket.position).normalized * force;
            Rocket.AddForce(finalForce);
        }
    }
}

