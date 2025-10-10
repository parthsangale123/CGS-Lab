using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenMainMenu : MonoBehaviour, IInteractable
{

    public void Interact()
    {
        SceneManager.LoadScene("MainMenu");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}