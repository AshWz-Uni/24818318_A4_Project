using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private string levelSceneName = "Level1";
    [SerializeField] private AudioSource introAudioSource;
    [SerializeField] private AudioSource mainMenuAudioSource;

    public void GoToLevel1() => StartCoroutine(WaitForAudioToFinish());

    public void GoStartScene() => SceneManager.LoadScene("StartScene");

    private IEnumerator WaitForAudioToFinish()
    {
        mainMenuAudioSource.Stop();
        introAudioSource.Play();
        yield return new WaitForSeconds(introAudioSource.clip.length);
        SceneManager.LoadScene(levelSceneName);
    }
}