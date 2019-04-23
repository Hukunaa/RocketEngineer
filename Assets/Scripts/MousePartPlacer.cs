using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePartPlacer : MonoBehaviour
{

    public Transform m_partTransform;

    Vector3 m_mouseWorldPod;
    Vector3 m_surfacePos = Vector3.zero;

    private float m_mouseZ = 10;
    public bool m_canTakePart;
    public RaycastHit m_partHit;

    private GameStateManager GameStatus;

    public Camera m_mainCam;

    public bool m_surfaceHit;
    public bool m_isOnGhost;

    // Start is called before the first frame update
    void Start()
    {
        m_isOnGhost = false;
        m_mainCam = Camera.main;
        m_canTakePart = false;
        GameStatus = GameObject.FindWithTag("GameStateManager").GetComponent<GameStateManager>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0) && GameStatus.Game == GameStateManager.GameState.BUILDING)
        {
            if (Physics.Raycast(m_mainCam.ScreenPointToRay(Input.mousePosition), out m_partHit, 1000.0f) && !m_isOnGhost)
            {
                m_partTransform = m_partHit.collider.transform;
                m_partTransform.parent = null;
                m_partTransform.gameObject.layer = 2;
                m_canTakePart = true;
                m_isOnGhost = true;

            }
            else if (m_isOnGhost == true)
            {
                Stick(m_partTransform.gameObject);
                m_partTransform.gameObject.layer = 9;
                m_isOnGhost = false;
                m_partTransform = null;
            }

            if(GameObject.FindObjectOfType<Pod>())
                GameObject.FindObjectOfType<Pod>().RecalculateRocket();
        }

        if (m_isOnGhost && GameStatus.Game == GameStateManager.GameState.BUILDING)
        {
            Vector3 mousePos = Vector3.zero;

            if (Input.GetKey(KeyCode.W))
                m_mouseZ += 0.1f;
            else if (Input.GetKey(KeyCode.S))
                m_mouseZ -= 0.1f;


            mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_mouseZ);

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000.0f))
            {
                m_surfacePos = hit.point;
                m_surfaceHit = true;
                m_partTransform.forward = hit.normal;
                m_partTransform.position = hit.point + hit.normal;
                m_partTransform.parent = hit.collider.transform;
                Debug.DrawRay(hit.point, hit.normal * 4, Color.red);
            }
            else
            {

                m_surfaceHit = false;
                m_partTransform.parent = null;
                m_mouseWorldPod = m_mainCam.ScreenToWorldPoint(mousePos);

                if (m_canTakePart)
                    m_partTransform.position = m_mouseWorldPod;
            }
        }

    }

    void Stick(GameObject hitObject)
    {

        if (m_surfaceHit)
        {
            Vector3 finalPos = m_partTransform.GetComponent<Collider>().ClosestPoint(m_surfacePos);
            m_partTransform.position = m_surfacePos - (finalPos - m_partTransform.position);
            m_canTakePart = false;
        }
        else
        {
            m_partTransform.GetComponent<JointFixer>().StickPart();
            m_partTransform.gameObject.layer = 9;
        }
    }
}
