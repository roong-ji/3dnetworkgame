using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusUIAbility : PlayerAbility
{
    [SerializeField] private Image _healthGauge;
    [SerializeField] private Image _staminaGauge;
    
    private void Update()
    {
        _healthGauge.fillAmount = _owner.Stat.Health / _owner.Stat.MaxHealth;
        _staminaGauge.fillAmount = _owner.Stat.Stamina / _owner.Stat.MaxStamina;
    }
}
