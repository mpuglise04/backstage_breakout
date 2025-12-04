using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float remainingTime;

    private void Start()
    {
        if (timerText != null)
            timerText.gameObject.SetActive(false);

        enabled = false;
    }

    public void StartTimer()
    {
        if (timerText != null)
            timerText.gameObject.SetActive(true);

        enabled = true;
    }
    void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }
        else
        {
            remainingTime = 0;
            timerText.color = Color.red;
            GameManager.Instance.GameOverTimeout();
        }

        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
