using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryCalc : MonoBehaviour
{

    private Pod m_pod;

    public Vector3[] pointsPositions;

    [Range (0, 256)]
    public int nbPoints;

    private LineRenderer m_line;
    private Planet m_planet;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        m_line = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if(m_planet == null && FindObjectOfType<Planet>())
            m_planet = FindObjectOfType<Planet>();

        if(m_pod == null)
        {
            foreach(Pod pod in FindObjectsOfType<Pod>())
                if(pod.m_activeVessel)
                    m_pod = pod;
        }

        pointsPositions = new Vector3[nbPoints];

        for(int i = 0; i < nbPoints; ++i)
        {
            if(m_planet.GetComponent<GravityCore>().currentRocket == m_pod.gameObject)
                pointsPositions[i] = m_pod.transform.position + new Vector3(m_pod.GetComponent<Rigidbody>().velocity.x * i, 
                                                                            m_pod.GetComponent<Rigidbody>().velocity.y * i - (0.5f * m_planet.GetComponent<GravityCore>().gravityAccel * i * i), 
                                                                            m_pod.GetComponent<Rigidbody>().velocity.z * i);

                //((m_pod.GetComponent<Rigidbody>().velocity + m_planet.GetComponent<GravityCore>().finalForce) * i);
                
        }
        SetPoints();
    }

    void SetPoints()
    {
        m_line.positionCount = nbPoints;
        m_line.SetPositions(pointsPositions);
    }
}
