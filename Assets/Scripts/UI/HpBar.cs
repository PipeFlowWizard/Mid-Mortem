using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    
    private Entity playerStats;

    private Image hpFill;

    private float medThreshold = 0.6f;
    private float sadThreshold = 0.3f;

    private void Start()
    {
        playerStats = GetComponentInParent<UI>().player.GetComponent<Entity>();
        hpFill = transform.Find("Skull-Fill").GetComponent<Image>();
    }

    public void UpdateHP()
    {
        float ratio = (float) playerStats.CurrentHealth / playerStats.MaxHealth;
        hpFill.fillAmount = ratio;
    }
}
