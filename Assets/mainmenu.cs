using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
public class mainmenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Animator anim;
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    public void Load()
    {
        anim.SetBool("fade", true);
        StartCoroutine(load());
    }
    public void OpenURL(string url)
    {
               Application.OpenURL(url);
    }
    IEnumerator load()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("SampleScene");
    }
}
