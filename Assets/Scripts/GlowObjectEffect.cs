using UnityEngine;

public class GlowObjectEffect : MonoBehaviour
{
    private Material mat;
    private Color originalEmission;

    void Start()
    {
        // Get the material of this object
        Renderer rend = GetComponent<Renderer>();
        mat = rend.material;

        // Enable emission so we can make it glow
        mat.EnableKeyword("_EMISSION");

        // Save the default "off" color
        originalEmission = mat.GetColor("_EmissionColor");
    }

    public void ToggleHighlight(bool state)
    {
        if (state)
        {
            // Turn glow ON (Yellowish-white)
            mat.SetColor("_EmissionColor", new Color(0.5f, 0.5f, 0.3f));
        }
        else
        {
            // Return to normal
            mat.SetColor("_EmissionColor", originalEmission);
        }
    }
}
