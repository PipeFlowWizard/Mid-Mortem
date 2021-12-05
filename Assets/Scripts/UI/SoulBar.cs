using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoulBar : MonoBehaviour
{
    private FloatValue soulCounter;
    [SerializeField] private Image soulFill;

    private void Start()
    {
        soulCounter = GetComponentInParent<UI>().soulCounter;
    }

    public void UpdateSouls()
    {
        float ratio = soulCounter.runTimeValue / 100f;
        soulFill.fillAmount = ratio;
    }
}
