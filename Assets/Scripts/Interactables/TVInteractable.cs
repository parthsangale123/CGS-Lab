using UnityEngine;
using UnityEngine.Video;
using System.IO; // Required for Path.Combine

public class TVInteractable : MonoBehaviour, IInteractable
{
    [Header("Components")]
    [SerializeField] private VideoPlayer videoPlayer; // VideoPlayer component
    [SerializeField] private Renderer tvScreenRenderer; // TV screen mesh renderer

    [Header("Materials & Videos")]
    [SerializeField] private Material blackMaterial; // Black material when TV is off

    // --- WebGL Change: Use string filenames instead of VideoClip assets ---
    [SerializeField] private string firstVideoFileName = "MyFirstVideo.mp4";
    [SerializeField] private string secondVideoFileName = "MySecondVideo.mp4";

    private Material originalMaterial;
    private enum TVState { Off, PlayingFirst, PlayingSecond }
    private TVState currentState = TVState.Off;

    private void Start()
    {
        originalMaterial = tvScreenRenderer.material;
        tvScreenRenderer.material = blackMaterial;

        // --- WebGL Change: Configure VideoPlayer for URL playback ---
        videoPlayer.source = VideoSource.Url;
        videoPlayer.prepareCompleted += OnVideoPrepared; // Optional: for smoother start

        videoPlayer.loopPointReached += OnVideoEnd;
    }

    public void Interact()
    {
        switch (currentState)
        {
            case TVState.Off:
                // First interaction → play first video
                PlayVideo(firstVideoFileName);
                currentState = TVState.PlayingFirst;
                break;

            case TVState.PlayingFirst:
                // Interaction during first video → switch to second video
                PlayVideo(secondVideoFileName);
                currentState = TVState.PlayingSecond;
                break;

            case TVState.PlayingSecond:
                // Interaction during second video → turn TV off
                StopTV();
                break;
        }
    }

    // --- WebGL Change: This method now accepts a filename string ---
    private void PlayVideo(string fileName)
    {
        // Construct the full URL to the video in the StreamingAssets folder
        videoPlayer.url = Path.Combine(Application.streamingAssetsPath, fileName);

        // Prepare the video to pre-buffer it. Playback starts in OnVideoPrepared.
        videoPlayer.Prepare();

        tvScreenRenderer.material = originalMaterial;
        NotificationManager.Instance.ShowNotification($"Loading {fileName}...");
    }

    // This event is called when videoPlayer.Prepare() is complete
    private void OnVideoPrepared(VideoPlayer source)
    {
        source.Play();
        NotificationManager.Instance.ShowNotification($"Now Playing...");
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
            PlayVideo(secondVideoFileName);
            currentState = TVState.PlayingSecond;
        }
        else if (currentState == TVState.PlayingSecond)
        {
            // Video 2 ended → TV turns off
            StopTV();
        }
    }
}