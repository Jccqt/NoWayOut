using UnityEngine;

public class LightController : MonoBehaviour
{
    private Light myLight;

    void Awake()
    {
        myLight = GetComponent<Light>();
    }

    public void ChangeToRed()
    {
        if (myLight != null) myLight.color = Color.red;
    }

    public void ChangeToWhite()
    {
        if (myLight != null) myLight.color = Color.white;
    }

    public void TurnOff()
    {
        if (myLight != null) myLight.enabled = false;
    }

    public void TurnOn()
    {
        if (myLight != null) myLight.enabled = true;
    }
}
