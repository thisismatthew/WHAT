using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DrawTask", menuName = "ScriptableObjects/DrawTaskObject", order = 1)]
public class DrawTask : ScriptableObject
{
    public Sprite traceImage;
    public string GestureName;
    public string TaskName;
    public string TaskDesc;
}
