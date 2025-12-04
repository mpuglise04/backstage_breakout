using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using TMPro;

public class LevelIntro : MonoBehaviour
{
    [Header("Cinemachine Cameras")]
    public CinemachineVirtualCamera introCam;
    public CinemachineVirtualCamera gameplayCam;

    [Header("Camera Pan Points")]
    public Transform panStart;
    public Transform panEnd;

    [Header("Timing")]
    public float fadeDuration = 2f;
    public float panDuration = 3f;

    [Header("UI")]
    public Image fadeImage;
    public TextMeshProUGUI skipIntroText;

    [Header("Player Control")]
    public MonoBehaviour playerMovementScript;

    [Header("Skip Intro")]
    public KeyCode skipKey = KeyCode.Space;

    private Coroutine introCoroutine;
    private bool introFinished = false;

    private void Start()
    {
        if (introCam != null)
            introCam.Priority = 20;
        if (gameplayCam != null)
            gameplayCam.Priority = 10;

        if (introCam != null && panStart != null)
        {
            Vector3 startPos = panStart.position;
            introCam.transform.position = new Vector3(
                startPos.x,
                startPos.y,
                introCam.transform.position.z
            );
        }

        if (fadeImage != null)
            fadeImage.color = new Color(0f, 0f, 0f, 1f);

        if (skipIntroText != null)
            skipIntroText.gameObject.SetActive(true);

        if (playerMovementScript != null)
            playerMovementScript.enabled = false;

        introCoroutine = StartCoroutine(IntroSequence());
    }

    private void Update()
    {
        if (!introFinished && Input.GetKeyDown(skipKey))
        {
            SkipIntro();
        }
    }

    private IEnumerator IntroSequence()
    {
        if (fadeImage != null && fadeDuration > 0f)
        {
            float t = 0f;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                float alpha = 1f - (t / fadeDuration); // 1 → 0
                alpha = Mathf.Clamp01(alpha);
                fadeImage.color = new Color(0f, 0f, 0f, alpha);
                yield return null;
            }
            fadeImage.color = new Color(0f, 0f, 0f, 0f);
        }

        if (introCam != null && panStart != null && panEnd != null && panDuration > 0f)
        {
            Vector3 startPos = new Vector3(
                panStart.position.x,
                panStart.position.y,
                introCam.transform.position.z
            );
            Vector3 endPos = new Vector3(
                panEnd.position.x,
                panEnd.position.y,
                introCam.transform.position.z
            );

            float t = 0f;
            while (t < panDuration)
            {
                t += Time.deltaTime;
                float progress = Mathf.Clamp01(t / panDuration);
                introCam.transform.position = Vector3.Lerp(startPos, endPos, progress);
                yield return null;
            }

            introCam.transform.position = endPos;
        }

        FinishIntro();
    }

    private void SkipIntro()
    {
        if (introCoroutine != null)
            StopCoroutine(introCoroutine);

        if (fadeImage != null)
            fadeImage.color = new Color(0f, 0f, 0f, 0f);

        if (introCam != null && panEnd != null)
        {
            introCam.transform.position = new Vector3(
                panEnd.position.x,
                panEnd.position.y,
                introCam.transform.position.z
            );
        }

        FinishIntro();
    }

    private void FinishIntro()
    {
        if (introFinished) return;
        introFinished = true;

        if (skipIntroText != null)
            skipIntroText.gameObject.SetActive(false);

        if (introCam != null)
            introCam.Priority = 0;
        if (gameplayCam != null)
            gameplayCam.Priority = 20;

        if (playerMovementScript != null)
            playerMovementScript.enabled = true;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.ShowIntroAndStartTimer();
        }
    }
}
