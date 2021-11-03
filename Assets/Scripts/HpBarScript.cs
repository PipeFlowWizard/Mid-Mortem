using UnityEngine;
using UnityEngine.UI;

public class HpBarScript : MonoBehaviour
{
    [SerializeField] private float hp;
    [SerializeField] private float maxHp;
    [SerializeField] private Image hpFill;
    public Sprite happySprite;
    public Sprite medSprite;
    public Sprite sadSprite;
    public Sprite deadSprite;
    [SerializeField] private Transform Player; // Get Player HP from Player Transform

    private Image image;

    private float medThreshold = 0.6f;
    private float sadThreshold = 0.3f;

    private void Start()
    {
        image = transform.Find("Face").GetComponent<Image>();
    }

    private void Update()
    {
        float ratio = hp / maxHp;
        hpFill.fillAmount = ratio;

        if (ratio > medThreshold)
            image.sprite = happySprite;
        else if (ratio > sadThreshold)
            image.sprite = medSprite;
        else if (ratio > 0)
            image.sprite = sadSprite;
        else
            image.sprite = deadSprite;
    }

    public void UpdateHP(float value)
    {
        hp += value;
        if (hp > maxHp)
            hp = maxHp;
        else if (hp < 0)
            hp = 0;
    }
}
