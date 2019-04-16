using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour
{

    public float PowerFactor = 0.0f;
    public float FuelCost;
    
    public GameObject EngineNozzle;
    public int Power;

    public GameObject Pod;
    public GameObject FuelTank;

    private bool foundPod;
    private bool foundFuelTank;

    // Start is called before the first frame update
    void Start()
    {
        FuelTank = gameObject;
        Pod = gameObject;
        foundPod = false;
    }

    void Update()
    {   
        if(!foundPod)
        {   for(int i = 0; i < 10; ++i)
            {
                if(!Pod.CompareTag("PodPart"))
                {
                    if(Pod.transform.parent != null)
                        Pod = Pod.transform.parent.gameObject;
                }
                else
                {
                    foundPod = true;
                    break;
                }
            }
        }

        if(!foundFuelTank)
        {   for(int i = 0; i < 10; ++i)
            {
                if(!FuelTank.GetComponent<FuelTank>())
                {
                    if(FuelTank.transform.parent != null)
                        FuelTank = FuelTank.transform.parent.gameObject;
                }
                else
                {
                    foundFuelTank = true;
                    break;
                }
            }
        }


        if(Input.GetKey(KeyCode.LeftShift))
        {
            PowerFactor += 0.01f;
        }

        if(Input.GetKey(KeyCode.LeftControl))
        {
            PowerFactor -= 0.01f;
        }

        if(PowerFactor <= 0.0f)
            PowerFactor = 0;

        if(PowerFactor >= 1.0f)
            PowerFactor = 1;

    }
    // Update is called once per frame
    void FixedUpdate()
    {

        if(GetComponent<Entity>().isConnectedToPod && FuelTank != gameObject && FuelTank.GetComponent<FuelTank>().tank.tankFuel > 0)
        {
            Pod.GetComponent<Rigidbody>().AddForceAtPosition(EngineNozzle.transform.up * Power * PowerFactor, EngineNozzle.transform.position);
            FuelTank.GetComponent<FuelTank>().tank.tankFuel -= FuelCost / 100 * PowerFactor; 
        }
    }
}
