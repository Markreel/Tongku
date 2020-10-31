using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] Gate gate;
    [Space]
    [SerializeField] float pressPlateDuration = 0.5f;
    [SerializeField] AnimationCurve pressPlateCurve;
    [SerializeField] Vector3 pressedOffset;
    private Coroutine pressPlateRoutine;

    [SerializeField] AudioClip pressedSFX;
    [SerializeField] AudioClip releasedSFX;
    private AudioSource audioSource;

    private Vector3 originalPos;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = false;
    }

    void Start()
    {
        originalPos = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
            PressPlate();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            ReleasePlate();
    }

    void PressPlate()
    {
        if(pressPlateRoutine != null) StopCoroutine(pressPlateRoutine);
        pressPlateRoutine = StartCoroutine(IPressPlate());
    }
    void ReleasePlate()
    {
        gate.CloseGate();
        audioSource.PlayOneShot(releasedSFX);
        if (pressPlateRoutine != null) StopCoroutine(pressPlateRoutine);
        pressPlateRoutine = StartCoroutine(IReleasePlate());
    }

    IEnumerator IPressPlate()
    {
        Vector3 _startPos = transform.position;

        float _lerpTime = 0;
        while (_lerpTime < 1)
        {
            _lerpTime += Time.deltaTime / pressPlateDuration;
            float _lerpKey = pressPlateCurve.Evaluate(_lerpTime);

            transform.position = Vector3.Lerp(_startPos, originalPos + pressedOffset, _lerpKey);

            yield return null;
        }

        audioSource.PlayOneShot(pressedSFX);
        gate.OpenGate();

        yield return null;
    }
    IEnumerator IReleasePlate()
    {
        Vector3 _startPos = transform.position;

        float _lerpTime = 0;
        while (_lerpTime < 1)
        {
            _lerpTime += Time.deltaTime / pressPlateDuration;
            float _lerpKey = pressPlateCurve.Evaluate(_lerpTime);

            transform.position = Vector3.Lerp(_startPos, originalPos, _lerpKey);

            yield return null;
        }

        yield return null;
    }



}
