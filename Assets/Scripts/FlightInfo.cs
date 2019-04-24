using UnityEngine;
using UnityEngine.UI;

public class FlightInfo : MonoBehaviour
{
    public int avgFrameRate;
    public Text m_fpsText;
    public Text m_speedText;
    public Text m_altitudeText;
    public Text m_atmosphereInfo;

    private float m_speed;
    private float m_altitude;
    private float m_atmosphereRatio;

    public void Start()
    {
    }
    public void Update()
    {
        //TEMPORARY BEFORE MULTISHIP INTEGRATION
        if(FindObjectOfType<Pod>())
        {
            m_atmosphereRatio = FindObjectOfType<Pod>().GetComponent<AeroDynamicsCore>().m_airResistance;
            m_speed = FindObjectOfType<Pod>().m_speed;
            m_altitude = FindObjectOfType<Pod>().m_altitude;
        }

        //===================
        float current = 0;
        current = (int)(1f / Time.unscaledDeltaTime);
        avgFrameRate = (int)current;
        
        if(m_fpsText != null)
            m_fpsText.text = avgFrameRate.ToString() + " FPS";
        
        if(m_speedText != null)
            m_speedText.text = m_speed + " Km/h";
        
        if(m_altitudeText != null)
            m_altitudeText.text = Mathf.RoundToInt(m_altitude) + " m";

        if(m_atmosphereInfo != null)
            m_atmosphereInfo.text = m_atmosphereRatio + " atm";
        
    }
}