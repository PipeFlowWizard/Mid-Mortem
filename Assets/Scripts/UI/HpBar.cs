using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    private static Entity playerStats;
    [SerializeField] private Image hpFill;

    private void Start()
    {
        if (playerStats == null)
            playerStats = GetComponentInParent<UI>().player.GetComponent<Entity>();
    }

    public void UpdateHP()
    {
        float ratio = (float) playerStats.CurrentHealth / playerStats.MaxHealth;
        hpFill.fillAmount = ratio;
    }
}
