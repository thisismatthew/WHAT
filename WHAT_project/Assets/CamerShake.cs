using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerShake : MonoBehaviour
{
    private Camera cam; // set this via inspector
    public float shake = 0;
    public float shakeAmount = 0.1f;
    public float decreaseFactor = 1.0f;
    private Vector3 origin, offset, UIorigin;
    public float UIShakeMagnitude = 10f;
    public GameObject UI;

    private void Start()
    {
        origin = transform.position;
        cam = Camera.main;
        UIorigin = UI.transform.position;
    }

    private void Update()
    {
        if (shake > 0)
        {
            offset = new Vector3(Random.Range(-shakeAmount, shakeAmount), Random.Range(-shakeAmount, shakeAmount), 0);
            transform.position = origin + offset;
            UI.transform.position = UIorigin + (offset*UIShakeMagnitude);
            shake -= Time.deltaTime * decreaseFactor;

        }
        else
        {
            transform.position = origin;
            shake = 0f;
        }
    }
}
