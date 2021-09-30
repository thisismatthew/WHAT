using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject workUI;

    public void DisableUI()
    {
        workUI.active = false;
    }
    public void EnableUI()
    {
        workUI.active = true;
    }
}
