using UnityEngine;

public class LightInteractable : MonoBehaviour, IInteractable
{
    private AudioSource audioSource;
    [SerializeField] private Light[] lights; // Lights to control by the switch
    private bool isOn = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

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

        string message = isOn ? "Lights turned ON" : "Lights turned OFF";
        if (NotificationManager.Instance != null)
        {
            NotificationManager.Instance.ShowNotification(message);
            audioSource.Play();
        }
    }
}
