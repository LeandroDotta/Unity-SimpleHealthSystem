using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleHealth.Samples
{
    public class CharacterSample : MonoBehaviour
    {
        [SerializeField] private HealthManager _manager;
        [SerializeField] private HealthText _healthTextPrefab;
        [SerializeField] private Color _colorHeal = Color.green;
        [SerializeField] private Color _colorDamage = Color.red;

        private void Start() 
        {
            _manager.OnHealthChange += HealthChange;
        }

        private void OnDestroy() 
        {
            _manager.OnHealthChange -= HealthChange;
        }

        private void HealthChange(float health, float amount)
        {
            if (amount > 0)
            {
                // Is Healing
                ShowHealthText(_colorHeal, amount.ToString());
            }
            else if (amount < 0)
            {
                // Is Damage
                ShowHealthText(_colorDamage, amount.ToString());
            }
        }

        private void ShowHealthText(Color color, string text)
        {
            HealthText healthText = Instantiate(_healthTextPrefab, transform.position + (Vector3.up * 2), Quaternion.identity, transform);
            healthText.Color = color;
            healthText.Text = text;
            healthText.gameObject.SetActive(true);
        }
    }
}