using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NoteInteract : MonoBehaviour
{
    private float _duration;
    private Vector3 _upScale;

    private NoteSfx _noteSfx;

    private void Start()
    {
        _noteSfx = FindObjectOfType<NoteSfx>();

        _duration = .5f;
        _upScale = new Vector3(0, 0, 0);
    }

    private void OnMouseUpAsButton()
    {
        _noteSfx.PlaySound();
        StartCoroutine(CoroShrinkAndGrow(_upScale, _duration));
    }

    private IEnumerator CoroShrinkAndGrow(Vector3 upScale, float duration)
    {
        Vector3 initialScale = transform.localScale;

        for (float time = 0; time < duration * 2; time += Time.deltaTime)
        {
            float progress = Mathf.PingPong(time, duration) / duration;
            transform.localScale = Vector3.Lerp(initialScale, upScale, progress);
            yield return null;
        }
        transform.localScale = initialScale;
    }
}
