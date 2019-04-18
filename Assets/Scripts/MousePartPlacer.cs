using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePartPlacer : MonoBehaviour
{

    public Transform PartTransform;
    
    Vector3 MouseWorldPos;
    Vector3 surfacePos = Vector3.zero;

    private float mouseZ = 10;
    public bool canTakePart;
    public RaycastHit PartHit;

    private GameStateManager GameStatus;
    
    public Camera mainCam;

    public bool hasSurfacetoHit;
    public bool isOnGhost;

    // Start is called before the first frame update
    void Start()
    {
        isOnGhost = false;

        canTakePart = false;
        GameStatus = GameObject.FindWithTag("GameStateManager").GetComponent<GameStateManager>();
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetMouseButtonDown(0) && GameStatus.Game == GameStateManager.GameState.BUILDING)
        {
            if(Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out PartHit, 1000.0f) && !isOnGhost)
            {
                PartTransform = PartHit.collider.transform;
                PartTransform.parent = null;
                PartTransform.gameObject.layer = 2;
                canTakePart = true;
                isOnGhost = true;

            }
            else if(isOnGhost == true)
            {
                Stick(PartTransform.gameObject);
                GameObject.FindObjectOfType<Pod>().RecalculateRocket();
                PartTransform.gameObject.layer = 9;
                isOnGhost = false;
                PartTransform = null;
            }
        }

        if(isOnGhost && GameStatus.Game == GameStateManager.GameState.BUILDING)
        {
            Vector3 mousePos = Vector3.zero;

            if(Input.GetKey(KeyCode.W))
                mouseZ += 0.1f;
            else if(Input.GetKey(KeyCode.S))
                mouseZ -= 0.1f;


            mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, mouseZ);

            RaycastHit hit;
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000.0f))
            {
                surfacePos = hit.point;
                hasSurfacetoHit = true;
                PartTransform.forward = hit.normal;
                PartTransform.position = hit.point + hit.normal;
                PartTransform.parent = hit.collider.transform;
                Debug.DrawRay(hit.point , hit.normal * 4, Color.red);
            }
            else
            {
                
                hasSurfacetoHit = false;
                PartTransform.parent = null;
                MouseWorldPos = mainCam.ScreenToWorldPoint(mousePos);
                
                if(canTakePart) 
                    PartTransform.position = MouseWorldPos;
            }
        }

    }

    void Stick(GameObject hitObject)
    {

        if(hasSurfacetoHit)
        {
            Vector3 finalPos = PartTransform.GetComponent<Collider>().ClosestPoint(surfacePos);
            Debug.Log(PartTransform.GetComponent<Collider>().ClosestPoint(surfacePos));

            /*if(hitObject.GetComponent<Entity>().isConnectedToPod)
            {
                Transform[] massParts = PartTransform.transform.GetComponentsInChildren<Transform>();
                    foreach(Transform part in massParts)
                        if(part.gameObject.GetComponent<Entity>())
                            if(part.gameObject.GetComponent<Entity>().isConnectedToPod)
                                GameObject.FindObjectOfType<Pod>().AddMass(part.gameObject);
            }*/

            PartTransform.position = surfacePos - (finalPos - PartTransform.position);
            GameObject.FindObjectOfType<Pod>().RecalculateRocket();
            canTakePart = false;
        }
        else
        {
            PartTransform.GetComponent<JointFixer>().StickPart();
            PartTransform.gameObject.layer = 9;
        }
    }
}
