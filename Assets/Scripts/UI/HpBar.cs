using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    public Sprite happySprite;
    public Sprite medSprite;
    public Sprite sadSprite;
    public Sprite deadSprite;

    private Entity playerStats;

    private Image hpFill;
    private Image faceEmotion;

    private float medThreshold = 0.6f;
    private float sadThreshold = 0.3f;

    private void Start()
    {
        playerStats = GetComponentInParent<UI>().player.GetComponent<Entity>();
        faceEmotion = transform.Find("Face").GetComponent<Image>();
        hpFill = transform.Find("Fill").GetComponent<Image>();
    }

    public void UpdateHP()
    {
        float ratio = (float) playerStats.CurrentHealth / playerStats.MaxHealth;
        hpFill.fillAmount = ratio;

        if (ratio > medThreshold)
            faceEmotion.sprite = happySprite;
        else if (ratio > sadThreshold)
            faceEmotion.sprite = medSprite;
        else if (ratio > 0)
            faceEmotion.sprite = sadSprite;
        else
            faceEmotion.sprite = deadSprite;
    }
}
