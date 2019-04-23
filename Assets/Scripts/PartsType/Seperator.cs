using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seperator : MonoBehaviour
{

    [SerializeField] private List<GameObject> ChildList;
    private GameObject Pod;
    private bool foundPod;
    private bool isTriggered;
    void Start()
    {
        isTriggered = false;
        Pod = gameObject;
        ChildList = new List<GameObject>();
    }

    void Update()
    {
        if(isTriggered == false)
        {
            if(Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("Detached Stage !");
                Detach();
                isTriggered = true;
            }
        }

    }

    public void Detach()
    {
         if(!foundPod)
        {
             for(int i = 0; i < 10; ++i)
            {
                if(!Pod.GetComponent<Pod>())
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

        GameObject MainPart = ChildList[0];
        MainPart.AddComponent<Pod>();
        MainPart.AddComponent<Rigidbody>();
        
        MainPart.transform.parent = null;
        MainPart.GetComponent<Pod>().Build();
        MainPart.GetComponent<Pod>().m_isSet = true;
        MainPart.GetComponent<Pod>().RecalculateRocket();
        MainPart.GetComponent<AeroDynamicsCore>().FindPod();
        for(int i = 0; i < MainPart.transform.childCount; ++i)
        {
            if(MainPart.transform.GetChild(i).GetComponent<Entity>())
            {
                MainPart.transform.GetChild(i).GetComponent<AeroDynamicsCore>().FindPod();
            }
        }

        MainPart.GetComponent<Pod>().m_launched = true;

        Pod.GetComponent<Pod>().RecalculateRocket();
        MainPart.GetComponent<Rigidbody>().velocity = Pod.GetComponent<Rigidbody>().velocity;
        MainPart.GetComponent<Rigidbody>().useGravity = false;
        MainPart.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        
        
    }

    public void Attach()
    {
        Transform[] childsTransform = transform.GetComponentsInChildren<Transform>();

        foreach(Transform child in childsTransform)
        {
            if(child.GetComponent<Entity>() && child.gameObject != transform.gameObject)
            {
                ChildList.Add(child.gameObject);
            }
        }


    }
}
