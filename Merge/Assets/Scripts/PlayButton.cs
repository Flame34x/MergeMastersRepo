using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;

    public bool isPlayButton;
    public bool isThemeButton;
    public bool isBackButton;
    public bool isCreditsButton;
    public int themeIndex;

    private AudioSource audioSource;

    public AudioClip buttonClickSound;
    public AudioClip woosh;

    public Transform mainTargetTransform;
    public Transform themeTargetTransform;
    public Transform creditsTargetTransform;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }
    public void OnMouseOver()
    {
        spriteRenderer.color = Color.green;
    }

    public void OnMouseExit()
    {
        spriteRenderer.color = Color.white;
    }

    // Coroutine for smooth camera movement
    private IEnumerator MoveCamera(Transform targetTransform)
    {
        float duration = 0.5f;
        float t = 0;

        Vector3 startingPosition = Camera.main.transform.position;
        Vector3 targetPositionVector = new Vector3(targetTransform.position.x, targetTransform.position.y, -10);

        while (t < 1)
        {
            t += Time.deltaTime / duration;
            Camera.main.transform.position = Vector3.Lerp(startingPosition, targetPositionVector, t);
            yield return null;
        }

        // Ensure the camera reaches the target position exactly
        Camera.main.transform.position = targetPositionVector;
    }

    public void OnMouseDown()
    {
        if (isPlayButton)
        {
            StartCoroutine(MoveCamera(themeTargetTransform));
            audioSource.PlayOneShot(woosh);
        }
        if (isThemeButton)
        {
            SceneManager.LoadScene(themeIndex);
        }
        if (isBackButton)
        {
            StartCoroutine(MoveCamera(mainTargetTransform));
            audioSource.PlayOneShot(woosh);
        }
        if (isCreditsButton)
        {
            StartCoroutine(MoveCamera(creditsTargetTransform));
            audioSource.PlayOneShot(woosh);
        }

        audioSource.PlayOneShot(buttonClickSound);

    }
}
