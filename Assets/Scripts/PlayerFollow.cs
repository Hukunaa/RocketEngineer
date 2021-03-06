﻿using UnityEngine;
using System.Collections;

public class PlayerFollow : MonoBehaviour
{

    public float turnSpeed = 0.0f;
    public float moveSpeed = 0.0f;
    public Vector3 m_offset;
    private Vector3 m_origin;
    private Pod m_player;
    private float m_distance;
    private float xRot;
    private float yRot;
    public bool isOnMap;

    void Start()
    {
        xRot = 0;
        yRot = 0;

        m_origin = Vector3.zero;
        m_distance = -20;
        m_offset = transform.position;
    }

    void FixedUpdate()
    {
        Vector3 finalPos = Vector3.zero;
        
        if(FindObjectOfType<GameStateManager>().Game == GameStateManager.GameState.BUILDING)
        {
            finalPos = m_origin;
        }
        else if(FindObjectOfType<GameStateManager>().Game == GameStateManager.GameState.SIMULATING)
        {

            foreach(Pod m_pod in FindObjectsOfType<Pod>())
                if(m_pod.m_activeVessel == true)
                    m_player = m_pod;

            finalPos = m_player.transform.position;
        }

        if (Input.GetMouseButton(1))
        {
            xRot += Input.GetAxis("Mouse X") * turnSpeed;
            yRot += Input.GetAxis("Mouse Y") * turnSpeed * -1;
        }
        if(Input.GetMouseButton(2))
        {
            m_origin -= transform.right * Input.GetAxis("Mouse X") * moveSpeed;
            m_origin -= transform.up * Input.GetAxis("Mouse Y") * moveSpeed;
        }

        Quaternion finalRot = Quaternion.Euler(yRot, xRot, 0);
        
        m_distance += Input.GetAxis("Mouse ScrollWheel") * 12;

        if(!isOnMap)
            transform.position = finalRot * new Vector3(0, 0, m_distance) + finalPos;
        else
            transform.position = finalRot * new Vector3(0, 0, m_distance - 60000) + finalPos;

        transform.rotation = Quaternion.Slerp(transform.rotation, finalRot, 0.4f);

    }
}