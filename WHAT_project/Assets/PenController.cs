using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PenController : MonoBehaviour
{
    public DrawAreaDetector detector;
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }
    // Update is called once per frame
    void Update()
    {
        if (detector.PosInArea(cam.ScreenToWorldPoint(Input.mousePosition)))
        {
            transform.position = cam.ScreenToWorldPoint(Input.mousePosition);
            Cursor.visible = false;
        }
        else
        {
            Cursor.visible = true;
        }
        
    }
}
