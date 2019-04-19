using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildEditorManager : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CreateEntity(GameObject entity)
    {
        Instantiate(entity, Camera.main.transform.position + Camera.main.transform.forward * 10, transform.rotation);
    }

    public void DisableButton(GameObject entity)
    {
        entity.SetActive(false);
    }

    public void EnableButton(GameObject entity)
    {
        entity.SetActive(true);
    }
}
