using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneManager : MonoBehaviour
{
    [SerializeField] private string LevelSceneName = "Level1";
    [SerializeField] private AudioSource introAudioSource;
    [SerializeField] private AudioSource mainMenuAudioSource;

    public void GoToLevel1()
    {
        StartCoroutine(WaitForAudioToFinish());

    }

    private IEnumerator WaitForAudioToFinish()
    {
        mainMenuAudioSource.Stop();
        introAudioSource.Play();
        yield return new WaitForSeconds(introAudioSource.clip.length);

        UnityEngine.SceneManagement.SceneManager.LoadScene(LevelSceneName);
    }
}
