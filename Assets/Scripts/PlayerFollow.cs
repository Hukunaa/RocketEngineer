using UnityEngine;
using System.Collections;

public class PlayerFollow : MonoBehaviour
{

    public float turnSpeed = 0.0f;
    public float moveSpeed = 0.0f;
    public Vector3 m_offset;
    private Vector3 m_origin;
    private Vector3 playerPos;
    private float xRot;
    private float yRot;
    private float m_distance;

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
            playerPos = FindObjectOfType<Pod>().transform.position;
            finalPos = playerPos;
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

        m_distance += Input.GetAxis("Mouse ScrollWheel") * 8;
        transform.position = Vector3.Slerp(transform.position, finalRot * new Vector3(0, 0, m_distance) + finalPos, 0.9f);
        transform.rotation = Quaternion.Slerp(transform.rotation, finalRot, 0.4f);

    }
}