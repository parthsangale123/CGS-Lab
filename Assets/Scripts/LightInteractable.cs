using UnityEngine;

public class LightInteractable : MonoBehaviour, IInteractable
{

    [SerializeField] private Light[] lights; // Lights to control
    private bool isOn = false;

    public void Interact()
    {
        if (lights == null || lights.Length == 0) return;

        isOn = !isOn;

        foreach (Light light in lights)
        {
            if (light != null)
                light.enabled = isOn;
        }

        Debug.Log(isOn ? "Lights turned ON" : "Lights turned OFF");
    }
    
}
