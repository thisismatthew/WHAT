using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextScene : MonoBehaviour
{

    public void LoadNext()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Restart()
    {
        SceneManager.LoadScene("Title");
        FindObjectOfType<AudioManager>().StopAll();
        Destroy(FindObjectOfType<AudioManager>().gameObject);
    }
}
