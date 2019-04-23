using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarPanel : MonoBehaviour {

    public int maxEnergyOutput;
    public float energyFactor;

    private GameObject Sun;
    private GameObject Pod;

	// Use this for initialization
	void Start ()
    {
        Sun = GameObject.FindWithTag("Sun");
        //Pod = GameObject.FindWithTag("PodPart");


    }
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 SunToPanelDir = Sun.transform.position - transform.position;
        energyFactor = Vector3.Dot(transform.up  / transform.up.magnitude, SunToPanelDir / SunToPanelDir.magnitude);
        if (energyFactor <= 0.0f)
            energyFactor = 0.0f;
        if(transform.parent == Pod.transform)
        {
            Debug.Log("LOOOl");
            Pod.GetComponent<Pod>().m_maxEnergy += maxEnergyOutput * energyFactor;
        }

	}
}
