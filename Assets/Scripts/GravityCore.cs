using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityCore : MonoBehaviour
{

    Rigidbody[] Rockets;
    public float planetMass;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Rockets = GameObject.FindObjectsOfType<Rigidbody>();
        foreach (Rigidbody Rocket in Rockets)
        {
            float force = ((6.67f * Mathf.Pow(10.0f, -11.0f) * Rocket.mass * planetMass) / Mathf.Pow(Vector3.Distance(Rocket.transform.position, transform.position) * 10, 2));
            Rocket.AddForce((transform.position - Rocket.position).normalized * force);
        }
    }
}
