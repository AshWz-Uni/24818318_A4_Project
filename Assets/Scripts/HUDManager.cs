using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class HUDManager : MonoBehaviour
{
    [Header("Text fields")]
    public TMP_Text gameTimer;
    public TMP_Text scaredTimerText;
    public TMP_Text scaredTimer;

    public float ElapsedTime { get; private set; }

    private bool timerRunning = true;

    private void Start()
    {
        scaredTimerText.gameObject.SetActive(false);
        scaredTimer.gameObject.SetActive(false);
        ElapsedTime = 0f;
        RefreshTimer();
    }

    private void Update()
    {
        if (timerRunning)
        {
            ElapsedTime += Time.deltaTime;
            RefreshTimer();
        }
    }   

    private void RefreshTimer()
    {
        if (gameTimer == null) return;
        int totalCs = Mathf.FloorToInt(ElapsedTime * 100f);
        int cs = totalCs % 100;
        int s = (totalCs / 100) % 60;
        int m = totalCs / 6000;
        gameTimer.text = $"{m:D2}:{s:D2}:{cs:D2}"; 
    }
}