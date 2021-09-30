using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DrawTask", menuName = "ScriptableObjects/DrawTaskObject", order = 1)]
public class DrawTask : ScriptableObject
{
    public Sprite traceImage;
    public string GestureName;
    public string Sender;
    public string TaskName;
    public string TaskDesc;
    public bool TaskFail = false;
    public string taskFailName;
    public string taskFailDesc;
}
