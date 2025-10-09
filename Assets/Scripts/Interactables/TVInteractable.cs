using UnityEngine;
using UnityEngine.Video;

public class TVInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private VideoPlayer videoPlayer;// VideoPlayer component
    [SerializeField] private Renderer tvScreenRenderer;// TV screen mesh renderer
    [SerializeField] private Material blackMaterial;// Black material to show when TV is off

    private bool isPlaying = false;
    private Material originalMaterial;// stores the original video material

    private void Start()
    {
        // Store the original material (video will display on this)
        originalMaterial = tvScreenRenderer.material;

        // Make TV black at the start
        tvScreenRenderer.material = blackMaterial;

        videoPlayer.loopPointReached += OnVideoEnd;
    }

    public void Interact()
    {
        Debug.Log("TV Interacted!");
        if (isPlaying)
        {
            videoPlayer.Pause();// pause the video
            NotificationManager.Instance.ShowNotification($"Video is Paused...");
        }
        else
        {
            videoPlayer.Play();
            tvScreenRenderer.material = originalMaterial;// show video
            NotificationManager.Instance.ShowNotification($"Video is Playing...");
        }

        isPlaying = !isPlaying;
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        isPlaying = false;
        tvScreenRenderer.material = blackMaterial;// show black after video ends
    }
}
