using UnityEngine;

public class SoulQueue : MonoBehaviour
{
    [SerializeField] private GameObject dashAbility;
    [SerializeField] private GameObject firstAbility;
    [SerializeField] private GameObject secondAbility;

    public void OnDashAbilityGot()
    {
        dashAbility.SetActive(true);
    }

    public void OnFirstAbilityGot()
    {
        firstAbility.SetActive(true);
    }

    public void OnSecondAbilityGot()
    {
        secondAbility.SetActive(true);
    }
}
