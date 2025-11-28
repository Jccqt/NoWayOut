using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    [Header("Components")]
    public Light myLight;
    public AudioSource buzzSource; // Assign the Looping "Sparks" source here
    public Renderer bulbRenderer;

    [Header("Secondary Sound")]
    public AudioClip fizzleClip;   // Drag "bulbFizzleOut.wav" here
    [Range(0, 100)]
    public int fizzleChance = 20;  // % Chance to play fizzle when flickering

    [Header("Settings")]
    public float minIntensity = 0f;
    public float maxIntensity = 10f;
    [Range(1, 50)]
    public int flickerSmoothness = 5;

    private Material bulbMat;
    private Color baseEmission;
    private float nextFizzleTime = 0f;

    void Start()
    {
        if (myLight == null) myLight = GetComponent<Light>();

        if (bulbRenderer != null)
        {
            bulbMat = bulbRenderer.material;
            bulbMat.EnableKeyword("_EMISSION");

            if (bulbMat.HasProperty("_EmissionColor"))
                baseEmission = bulbMat.GetColor("_EmissionColor");
            else
                baseEmission = Color.white;
        }
    }

    void Update()
    {
        if (Random.Range(0, flickerSmoothness) == 0)
        {
            // 1. Randomize Light
            float val = Random.Range(minIntensity, maxIntensity);
            myLight.intensity = val;

            float ratio = val / maxIntensity;

            // 2. Sync Bulb Material
            if (bulbRenderer != null)
            {
                bulbMat.SetColor("_EmissionColor", baseEmission * ratio);
            }

            // 3. Control the "Sparks" Loop Volume
            if (buzzSource != null)
            {
                buzzSource.volume = ratio * 0.6f; // Quieter when dim
                buzzSource.pitch = Random.Range(0.8f, 1.2f);

                // 4. Randomly play the "Fizzle" sound
                // Only if light is dim (val is low) AND random chance hits AND we didn't just play it
                if (val < (maxIntensity * 0.5f) && Random.Range(0, 100) < fizzleChance && Time.time > nextFizzleTime)
                {
                    if (fizzleClip != null)
                    {
                        buzzSource.PlayOneShot(fizzleClip); // Play sound ON TOP of the sparks
                        nextFizzleTime = Time.time + 0.5f;  // Don't spam it too fast
                    }
                }
            }
        }
    }
}
