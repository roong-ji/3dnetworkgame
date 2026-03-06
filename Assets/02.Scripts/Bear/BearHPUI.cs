using UnityEngine;
using UnityEngine.UI;

public class BearHPUI : MonoBehaviour
{
    [SerializeField] private BearController _bearController;
    [SerializeField] private Image _healthGauge;
    
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        transform.forward = _camera.transform.forward;
        _healthGauge.fillAmount = _bearController.Stat.Health / _bearController.Stat.MaxHealth;
    }
}
