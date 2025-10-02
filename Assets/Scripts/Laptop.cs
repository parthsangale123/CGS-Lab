using UnityEngine;

public class Laptop : MonoBehaviour, IInteractable
{
    [SerializeField] private Renderer screenRenderer;// The laptop screen mesh renderer
    [SerializeField] private Texture offTexture;// Black/off texture
    [SerializeField] private Texture onTexture;// Image to show when on

    private bool isOn = false;

    public void Interact()
    {
        isOn = !isOn;

        if (isOn)
            screenRenderer.material.mainTexture = onTexture;
        else
            screenRenderer.material.mainTexture = offTexture;
    }
}
