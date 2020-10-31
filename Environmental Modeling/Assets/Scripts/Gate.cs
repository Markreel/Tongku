using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] float openGateDuration = 3f;
    [SerializeField] float closeGateDuration = 0.5f;
    [SerializeField] AnimationCurve openGateCurve;
    [SerializeField] AnimationCurve closeGateCurve;
    [SerializeField] Vector3 openGateOffset;
    private Coroutine openGateRoutine;

    [SerializeField] AudioClip openGateSFX;
    [SerializeField] AudioClip closeGateSFX;
    [SerializeField] AudioClip shutGateSFX;
    private AudioSource audioSource;

    private Vector3 originalPos;

    void Start()
    {
        originalPos = transform.position;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = false;
    }

    public void OpenGate()
    {
        if (openGateRoutine != null) StopCoroutine(openGateRoutine);
        openGateRoutine = StartCoroutine(IOpenGate());
    }

    public void CloseGate()
    {
        if (openGateRoutine != null) StopCoroutine(openGateRoutine);
        openGateRoutine = StartCoroutine(ICloseGate());
    }

    IEnumerator IOpenGate()
    {
        Vector3 _startPos = transform.position;
        audioSource.PlayOneShot(openGateSFX);
        float _loopTimer = openGateSFX.length;

        float _lerpTime = 0;
        while (_lerpTime < 1)
        {
            _lerpTime += Time.deltaTime / openGateDuration;
            float _lerpKey = openGateCurve.Evaluate(_lerpTime);

            _loopTimer -= Time.deltaTime;
            if(_loopTimer <= 0)
            {
                _loopTimer = openGateSFX.length;
                audioSource.PlayOneShot(openGateSFX);
            }


            transform.position = Vector3.Lerp(_startPos, originalPos + openGateOffset, _lerpKey);

            yield return null;
        }

        yield return null;
    }
    IEnumerator ICloseGate()
    {
        Vector3 _startPos = transform.position;
        audioSource.PlayOneShot(closeGateSFX);
        float _loopTimer = openGateSFX.length;

        float _lerpTime = 0;
        while (_lerpTime < 1)
        {
            _lerpTime += Time.deltaTime / closeGateDuration;
            float _lerpKey = closeGateCurve.Evaluate(_lerpTime);

            _loopTimer -= Time.deltaTime;
            if (_loopTimer <= 0)
            {
                _loopTimer = openGateSFX.length;
                audioSource.PlayOneShot(openGateSFX);
            }

            transform.position = Vector3.Lerp(_startPos, originalPos, _lerpKey);

            yield return null;
        }

        audioSource.PlayOneShot(shutGateSFX);

        yield return null;
    }


}
