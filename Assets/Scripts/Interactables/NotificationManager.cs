using UnityEngine;
using TMPro;
using System.Collections;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance;

    [SerializeField] private TMP_Text notificationText;
    [SerializeField] private float displayTime = 2f; // seconds

    private Coroutine currentRoutine;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        notificationText.text = "";
    }

    public void ShowNotification(string message)
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(DisplayMessage(message));
    }

    private IEnumerator DisplayMessage(string message)
    {
        notificationText.text = message;
        yield return new WaitForSeconds(displayTime);
        notificationText.text = "";
    }
}