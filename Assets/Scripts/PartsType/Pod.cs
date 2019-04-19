using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pod : MonoBehaviour
{
    public float MaxEnergy = 500;
    public float mass = 0;

    public bool isSetup = false;
    public bool Launched = false;
    private bool isBuilt = false;

    private GameStateManager GameStatus;

    private float lastTime;

    private Text Speed;

    void Start()
    {
        lastTime = 0;
        GameStatus = GameObject.FindObjectOfType<GameStateManager>();

    }

    void Update()
    {
        if (Speed == null)
            Speed = GameObject.Find("Speed").GetComponent<Text>();
        //CONTROL SYSTEM
        if (isBuilt)
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                GetComponent<OriginShifting>().ReloadWorldObjects();
            }

            if (Input.GetKey(KeyCode.Q))
                GetComponent<Rigidbody>().AddTorque(transform.up * 10000);
            if (Input.GetKey(KeyCode.E))
                GetComponent<Rigidbody>().AddTorque(-transform.up * 10000);

            if (Time.time > lastTime)
            {
                RecalculateRocket();
                lastTime = Time.time + 0.5f;
            }

            if (!isSetup && GameObject.Find("RocketSpawn"))
            {
                gameObject.transform.position = GameObject.Find("RocketSpawn").transform.position + Vector3.up * 30;
                gameObject.transform.rotation = Quaternion.identity;
                isSetup = true;
            }
            if (Input.GetKeyDown(KeyCode.Space) && !Launched)
            {
                Launched = true;
            }

            GetComponent<Rigidbody>().isKinematic = !Launched;
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
            if (Speed != null)
                Speed.text = GetComponent<Rigidbody>().velocity.magnitude * 3.6f + " Km/h";
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
        isBuilt = true;

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

        mass = 0;
        foreach (Transform part in parts)
        {
            if (part.gameObject.GetComponent<Entity>())
            {
                if (part.gameObject.GetComponent<Entity>().isConnectedToPod)
                {
                    RocketParts.Add(part.gameObject);
                    mass += part.gameObject.GetComponent<Entity>().finalmass;
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

        centerOfMass.x /= mass;
        centerOfMass.y /= mass;
        centerOfMass.z /= mass;
        GetComponent<Rigidbody>().centerOfMass = transform.InverseTransformPoint(centerOfMass);
        GetComponent<Rigidbody>().mass = mass;
    }
}

