using UnityEngine;
using UnityEngine.UI;

public class LockedDoor : MonoBehaviour, IInteractable
{
    public void Interact()
    {
         NotificationManager.Instance.ShowNotification($"The Door is Locked...");
    }

}
