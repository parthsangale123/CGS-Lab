using UnityEngine;
using UnityEngine.Video;

public class TVInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private VideoPlayer videoPlayer;// VideoPlayer component
    [SerializeField] private Renderer tvScreenRenderer;// TV screen mesh renderer
    [SerializeField] private Material blackMaterial;// Black material when TV is off
    [SerializeField] private VideoClip firstVideo;// First video clip
    [SerializeField] private VideoClip secondVideo;// Second video clip

    private Material originalMaterial;
    private enum TVState { Off, PlayingFirst, PlayingSecond }
    private TVState currentState = TVState.Off;

    private void Start()
    {
        originalMaterial = tvScreenRenderer.material;
        tvScreenRenderer.material = blackMaterial;

        videoPlayer.loopPointReached += OnVideoEnd;
    }

    public void Interact()
    {
        switch (currentState)
        {
            case TVState.Off:
                // First interaction → play first video
                PlayVideo(firstVideo);
                currentState = TVState.PlayingFirst;
                NotificationManager.Instance.ShowNotification("Video 1 is Playing...");
                break;

            case TVState.PlayingFirst:
                // Interaction during first video → switch to second video
                PlayVideo(secondVideo);
                currentState = TVState.PlayingSecond;
                NotificationManager.Instance.ShowNotification("Video 2 is Playing...");
                break;

            case TVState.PlayingSecond:
                // Interaction during second video → turn TV off
                StopTV();
                break;
        }
    }

    private void PlayVideo(VideoClip clip)
    {
        videoPlayer.clip = clip;
        videoPlayer.Play();
        tvScreenRenderer.material = originalMaterial;
    }

    private void StopTV()
    {
        videoPlayer.Stop();
        tvScreenRenderer.material = blackMaterial;
        currentState = TVState.Off;
        NotificationManager.Instance.ShowNotification("TV is Off.");
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        if (currentState == TVState.PlayingFirst)
        {
            // Automatic progression → play second video
            PlayVideo(secondVideo);
            currentState = TVState.PlayingSecond;
            NotificationManager.Instance.ShowNotification("Video 2 is Playing...");
        }
        else if (currentState == TVState.PlayingSecond)
        {
            // Video 2 ended → TV turns off
            StopTV();
        }
    }
}
