using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelFailedUI : MonoBehaviour
{
    [SerializeField]
    private Text failReason;

    public void SetFailedReason(string _reason)
    {
        failReason.text = _reason;
    }
}
