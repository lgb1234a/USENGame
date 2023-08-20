using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadNextScene());
    }

    IEnumerator LoadNextScene() {
        yield return new WaitForSecondsRealtime(3.0f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Home");
    }
}
