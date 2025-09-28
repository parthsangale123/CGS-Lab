using UnityEngine;
using UnityEngine.Video;

public class TVInteractable : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;  // VideoPlayer component on the TV

    public bool isPlaying = false;

    public void Interact()
    {
        Debug.Log("TV Interacted!");
        if (isPlaying)
        {
            videoPlayer.Pause();
        }
        else
        {
            videoPlayer.Play();
        }

        isPlaying = !isPlaying;
    }
}
