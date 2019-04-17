using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OriginShifting : MonoBehaviour
{
    
    [SerializeField] private float OriginThreshold;
    public Transform[] WorldGameObjs;
    private Vector3 lastPos;

    // Start is called before the first frame update
    void Start()
    {
        lastPos = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if( transform.position.x > OriginThreshold || transform.position.x < -OriginThreshold ||
            transform.position.y > OriginThreshold || transform.position.y < -OriginThreshold ||
            transform.position.z > OriginThreshold || transform.position.z < -OriginThreshold)
            {
                lastPos = transform.position;
                foreach(Transform obj in WorldGameObjs)
                    if(obj.parent == null)
                        obj.position -= lastPos;
            }

    }

    public void ReloadWorldObjects()
    {
        WorldGameObjs = Transform.FindObjectsOfType<Transform>();
    }
}
