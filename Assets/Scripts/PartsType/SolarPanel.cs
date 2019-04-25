using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarPanel : MonoBehaviour {

    public int maxEnergyOutput;
    public float energyFactor;

    private GameObject m_sun;
    private GameObject m_pod;

	// Use this for initialization
	void Start ()
    {
        m_sun = GameObject.FindWithTag("Sun");
        //Pod = GameObject.FindWithTag("PodPart");


    }
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 SunToPanelDir = m_sun.transform.position - transform.position;
        energyFactor = Vector3.Dot(transform.up  / transform.up.magnitude, SunToPanelDir / SunToPanelDir.magnitude);
        if (energyFactor <= 0.0f)
            energyFactor = 0.0f;
        if(transform.parent == m_pod.transform)
        {
            m_pod.GetComponent<Pod>().m_maxEnergy += maxEnergyOutput * energyFactor;
        }

	}
}
