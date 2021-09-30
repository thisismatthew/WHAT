using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerShake : MonoBehaviour
{
    private Camera cam; // set this via inspector
    public float shake = 0;
    public float shakeAmount = 0.1f;
    public float decreaseFactor = 1.0f;
    private Vector3 origin, offset;

    private void Start()
    {
        origin = transform.position;
        cam = Camera.main;
    }

    private void Update()
    {
        if (shake > 0)
        {
            offset = new Vector3(Random.Range(-shakeAmount, shakeAmount), Random.Range(-shakeAmount, shakeAmount), 0);
            transform.position = origin + offset;
            shake -= Time.deltaTime * decreaseFactor;

        }
        else
        {
            transform.position = origin;
            shake = 0f;
        }
    }
}
