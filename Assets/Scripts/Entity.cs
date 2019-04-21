using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public bool isConnectedToPod;
    public float mass;
    public float finalmass;
    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.GetComponent<Pod>())
            isConnectedToPod = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<FuelTank>())
        {
            finalmass = GetComponent<FuelTank>().tankFuel + mass;
            
        }
        else
            finalmass = mass;

        if (transform.parent)
        {
            if (transform.parent.GetComponent<Entity>().isConnectedToPod)
                isConnectedToPod = true;
        }
        else if (gameObject.GetComponent<Pod>())
            isConnectedToPod = true;
        else
            isConnectedToPod = false;
    }
}
