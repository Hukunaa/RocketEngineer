using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pod : MonoBehaviour
{
    public float m_maxEnergy = 500;
    public float m_mass = 0;

    public bool m_isSet = false;
    public bool m_launched = false;
    private bool m_isBuilt = false;
    public bool m_activeVessel = false;
    private GameStateManager m_gameStatus;

    private float m_lastCheck;

    private Text m_speedText;

    public float m_speed;
    public float m_altitude;
    

    void Start()
    {
        m_activeVessel = true;
        m_lastCheck = 0;
        m_gameStatus = GameObject.FindObjectOfType<GameStateManager>();

    }

    void FixedUpdate()
    {
        //CONTROL SYSTEM
        if (m_isBuilt)
        {
            m_speed = Mathf.Round(GetComponent<Rigidbody>().velocity.magnitude * 3.6f);
            m_altitude = Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Planet").transform.position) - GameObject.FindGameObjectWithTag("Planet").GetComponent<GravityCore>().radius;


            if (Input.GetKey(KeyCode.Q))
                GetComponent<Rigidbody>().AddTorque(transform.up * 10000);
            if (Input.GetKey(KeyCode.E))
                GetComponent<Rigidbody>().AddTorque(-transform.up * 10000);

            if (!m_isSet)
            {
                FindLaunchPad();
            }
            if (Input.GetKeyDown(KeyCode.Space) && !m_launched)
            {
                Launch();
                ReloadFloatingOrigin();
            }

            GetComponent<Rigidbody>().isKinematic = !m_launched;
            CheckRocketPhysics();
            ThrustControl();
            CheckMiscInputs();

        }
    }

    void Launch()
    {
        m_launched = true;
    }

    public void CheckRocketPhysics()
    {
        if (Time.time > m_lastCheck)
        {
            RecalculateRocket();
            m_lastCheck = Time.time + 0.5f;
        }
    }

    public void ReloadFloatingOrigin()
    {
        GetComponent<OriginShifting>().ReloadWorldObjects();
    }

    void FindLaunchPad()
    {
        if(GameObject.Find("RocketSpawn"))
        {
            gameObject.transform.position = GameObject.Find("RocketSpawn").transform.position + Vector3.up * 30;
            gameObject.transform.rotation = Quaternion.identity;
            m_isSet = true;
        }
    }

    void CheckMiscInputs()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            m_gameStatus.LoadEditor();
        }

        if (Input.GetKey(KeyCode.K))
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * 10000);
        }
    }

    void ThrustControl()
    {
        Engine[] engines = GameObject.FindObjectsOfType<Engine>();
        foreach (Engine engine in engines)
        {
            if (engine.GetComponent<Entity>().isConnectedToPod)
            {
                if (Input.GetKey(KeyCode.W))
                    engine.EngineNozzle.transform.localEulerAngles = new Vector3(20, 0, 0);
                else if (Input.GetKey(KeyCode.S))
                    engine.EngineNozzle.transform.localEulerAngles = new Vector3(-20, 0, 0);
                else if (Input.GetKey(KeyCode.A))
                    engine.EngineNozzle.transform.localEulerAngles = new Vector3(0, 0, 20);
                else if (Input.GetKey(KeyCode.D))
                    engine.EngineNozzle.transform.localEulerAngles = new Vector3(0, 0, -20);
                else
                    engine.EngineNozzle.transform.localEulerAngles = new Vector3(0, 0, 0);
            }
        }
    }
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.TransformPoint(GetComponent<Rigidbody>().centerOfMass), 0.4f);
    }
    public void Build()
    {
        //PodCamera.gameObject.SetActive(true);

        //GameStatus.Game = GameStateManager.GameState.SIMULATING;
        m_isBuilt = true;

        GameObject[] AllParts = GameObject.FindGameObjectsWithTag("Part");
        foreach (GameObject Part in AllParts)
        {
            if (Part.GetComponent<Entity>() && Part.transform.IsChildOf(gameObject.transform))
                if (Part.GetComponent<Entity>().isConnectedToPod)
                {
                    Part.GetComponent<JointFixer>().enabled = false;
                    if (Part.GetComponent<Seperator>())
                    {
                        Part.GetComponent<Seperator>().Attach();
                    }
                }
        }
    }

    public void RecalculateRocket()
    {
        List<GameObject> RocketParts = new List<GameObject>();
        Transform[] parts = transform.GetComponentsInChildren<Transform>();

        m_mass = 0;
        foreach (Transform part in parts)
        {
            if (part.gameObject.GetComponent<Entity>())
            {
                if (part.gameObject.GetComponent<Entity>().isConnectedToPod)
                {
                    RocketParts.Add(part.gameObject);
                    m_mass += part.gameObject.GetComponent<Entity>().finalmass;
                }
            }
        }

        Vector3 centerOfMass;
        centerOfMass = Vector3.zero;
        //ObjectsforMass.Add(RocketPart);  

        for (int i = 0; i < RocketParts.ToArray().Length; i++)
        {
            centerOfMass += new Vector3(RocketParts[i].transform.position.x * RocketParts[i].GetComponent<Entity>().finalmass,
                                            RocketParts[i].transform.position.y * RocketParts[i].GetComponent<Entity>().finalmass,
                                            RocketParts[i].transform.position.z * RocketParts[i].GetComponent<Entity>().finalmass);
        }

        centerOfMass.x /= m_mass;
        centerOfMass.y /= m_mass;
        centerOfMass.z /= m_mass;
        GetComponent<Rigidbody>().centerOfMass = transform.InverseTransformPoint(centerOfMass);
        GetComponent<Rigidbody>().mass = m_mass;
    }
}

