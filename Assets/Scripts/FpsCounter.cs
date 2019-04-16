using UnityEngine;
using UnityEngine.UI;
 
public class FpsCounter : MonoBehaviour
{
    public int avgFrameRate;
    public Text display_Text;

    public void Start()
    {
        display_Text = GetComponent<Text>();
    }
    public void Update ()
    {
        float current = 0;
        current = (int)(1f / Time.unscaledDeltaTime);
        avgFrameRate = (int)current;
        display_Text.text = avgFrameRate.ToString() + " FPS";
    }
}