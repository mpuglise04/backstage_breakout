using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SunglassesTimerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    private Coroutine timerRoutine;

    private void Awake()
    {
        // Start invisible
        SetAlpha(0f);
    }

    public void StartTimer(float duration)
    {
        // Restart if it's already running
        if (timerRoutine != null)
            StopCoroutine(timerRoutine);

        timerRoutine = StartCoroutine(TimerRoutine(duration));
    }

    private IEnumerator TimerRoutine(float duration)
    {
        float remaining = duration;

        // Fade text visible
        SetAlpha(1f);

        while (remaining > 0f)
        {
            remaining -= Time.deltaTime;

            int seconds = Mathf.CeilToInt(remaining);

            timerText.text = $"Sunglasses: {seconds}s";

            yield return null;
        }

        // Fade out when done
        float fadeTime = 0.5f;
        float t = fadeTime;
        while (t > 0f)
        {
            t -= Time.deltaTime;
            SetAlpha(t / fadeTime);
            yield return null;
        }

        SetAlpha(0f);
        timerRoutine = null;
    }

    private void SetAlpha(float a)
    {
        if (timerText == null) return;

        Color c = timerText.color;
        c.a = a;
        timerText.color = c;
    }
}
