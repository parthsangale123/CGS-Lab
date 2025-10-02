using UnityEngine;

public class Speaker : MonoBehaviour, IInteractable
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] playlist;//Songs
    private int currentTrack = -1;//Start before first

    private void Awake()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
        audioSource.loop = false;//We want natural song endings
        audioSource.playOnAwake = false;
    }

    public void Interact()
    {
        if (playlist.Length == 0) return;

        //If speaker is stopped -> start from first song
        if (currentTrack == -1)
        {
            currentTrack = 0;
            PlayTrack(currentTrack);
            return;
        }

        //If not last track -> switch to next immediately
        if (currentTrack < playlist.Length - 1)
        {
            currentTrack++;
            PlayTrack(currentTrack);
        }
        else
        {
            //Last track -> stop the speaker
            StopSpeaker();
        }
    }

    private void Update()
    {
        //Handle auto progression
        if (currentTrack >= 0 && !audioSource.isPlaying)
        {
            //If more tracks remain -> go to next
            if (currentTrack < playlist.Length - 1)
            {
                currentTrack++;
                PlayTrack(currentTrack);
            }
            else
            {
                //Last song finished naturally -> stop
                StopSpeaker();
            }
        }
    }

    private void PlayTrack(int index)
    {
        audioSource.clip = playlist[index];
        audioSource.Play();
        Debug.Log($"Now playing: {playlist[index].name}");
    }

    private void StopSpeaker()
    {
        audioSource.Stop();
        currentTrack = -1;// Reset
        Debug.Log("Speaker stopped.");
    }
}
