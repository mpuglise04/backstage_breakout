using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JetpackMessageUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private float displayTime = 3f;

    private void Awake()
    {
        // Make sure text starts invisible
        if (messageText != null)
        {
            Color c = messageText.color;
            c.a = 0f;
            messageText.color = c;
        }
    }

    public void ShowMessage()
    {
        StartCoroutine(ShowMessageRoutine());
    }

    private IEnumerator ShowMessageRoutine()
    {
        // Fade in instantly
        Color c = messageText.color;
        c.a = 1f;
        messageText.color = c;

        // Wait a few seconds
        yield return new WaitForSeconds(displayTime);

        // Fade out
        float fadeDuration = 1f;
        float t = fadeDuration;
        while (t > 0f)
        {
            t -= Time.deltaTime;
            c.a = t / fadeDuration;
            messageText.color = c;
            yield return null;
        }

        // Ensure fully invisible
        c.a = 0f;
        messageText.color = c;
    }
}
