using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{

    public Camera MapCamera;
    public Camera PlayerCamera;
    public bool m_isOnmap;
    public LineRenderer TrajectoryLine;

    // Start is called before the first frame update
    void Start()
    {
        m_isOnmap = false;
    }

    public void FindCameras()
    {
        PlayerCamera = GameObject.Find("PlayerCam").GetComponent<Camera>();
        MapCamera = GameObject.Find("MapCam").GetComponent<Camera>();
        TrajectoryLine = GameObject.Find("TrajectoryLine").GetComponent<LineRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        if(SceneManager.GetActiveScene().name == "Sandbox" && PlayerCamera == null)
            FindCameras();

        if(Input.GetKeyDown(KeyCode.M))
            m_isOnmap = !m_isOnmap;

        if(PlayerCamera && MapCamera)
        {
            if(m_isOnmap)
            {
                TrajectoryLine.enabled = true;
                PlayerCamera.enabled = false;
                MapCamera.enabled = true;
                MapCamera.GetComponent<PlayerFollow>().isOnMap = true;
            }
            if(!m_isOnmap)
            {
                TrajectoryLine.enabled = false;
                PlayerCamera.enabled = true;
                PlayerCamera.GetComponent<PlayerFollow>().isOnMap = false;
                MapCamera.enabled = false;
            }
        }
    }
}
