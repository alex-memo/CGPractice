using TMPro;
using UnityEngine;

public class HUDManager : InstanceFactory<HUDManager>
{
    [SerializeField] private TMP_Text hpText;

    private void Start()
    {
        GameManager.Instance.OnDealDamage += updateHpText;
    }

    private void updateHpText()
    {
        hpText.text = $"Health = {GameManager.Instance.Hp}";
    }
}
