using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    SpriteRenderer _spriteRenderer;

    [SerializeField] private float fadeOutTime = 2.0f;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        StartCoroutine(FadeOutCoroutine(fadeOutTime));
    }

    IEnumerator FadeOutCoroutine(float fadeOutTime)
    {
        float time = fadeOutTime;
        var color = _spriteRenderer.color;

        while (time > 0)
        {
            float value = Mathf.Lerp(0, fadeOutTime, time);

            color.a = time;
            time -= Time.deltaTime;
            _spriteRenderer.color = color;

            yield return new WaitForEndOfFrame();
        }
    }
}
