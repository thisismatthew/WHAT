using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmitEvent : MonoBehaviour
{
    private GameManager gm;
    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    public void SubmitionEvent()
    {
        FindObjectOfType<AudioManager>().Play("Upload");
        gm.SubmitTask();
    }
}
