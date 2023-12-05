using System.Collections;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public RectTransform rectTransform;
    public float moveSpeed = 100f; // Adjust this value to control the speed

    public void Move()
    {
        StartCoroutine(MoveToMiddle());
    }

    public IEnumerator MoveToMiddle()
    {
        Debug.Log("MoveToMiddle coroutine started.");

        RectTransform myRectTransform = gameObject.GetComponent<RectTransform>();
        while (myRectTransform.anchoredPosition.y < rectTransform.anchoredPosition.y)
        {
            float step = moveSpeed * Time.deltaTime;
            myRectTransform.anchoredPosition = Vector2.MoveTowards(myRectTransform.anchoredPosition, rectTransform.anchoredPosition, step);
            yield return null;
        }

        Debug.Log("MoveToMiddle coroutine finished.");
    }
}