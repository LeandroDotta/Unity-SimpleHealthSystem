using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleHealth.Samples
{
    public class HealthManagerTester : MonoBehaviour
    {
        [SerializeField] private HealthManager _manager;

        private string _textMaxHealth;
        private string _textHealthAmount;
        private string _textCooldown;
        private float _healthAmount = 1;

        private void Start() 
        {
            _textMaxHealth = _manager.MaxHealth.ToString();
            _textHealthAmount = _healthAmount.ToString();
            _textCooldown = _manager.Cooldown.ToString();
        }

        private void OnGUI() 
        {
            GUILayout.BeginArea(new Rect(10, 10, 300, 1000));

            GUILayout.BeginVertical(GUILayout.Width(200));
            GUILayout.Label("Health Manager");

                GUILayout.Label($"Current Health {_manager.Health}");
                GUILayout.Toggle(_manager.IsCoolingDown, "Cooling Down");
                GUILayout.Label($"Cooldown Timer: {_manager.CooldownTimer}");
                
                GUILayout.Label("--------------------");
                GUILayout.Label("Health Control");

                // Max Health Field
                GUILayout.BeginHorizontal();
                GUILayout.Label("Max Health");

                _textMaxHealth = GUILayout.TextField(_textMaxHealth);
                if (float.TryParse(_textMaxHealth, out float newMaxHealth)) 
                {
                    if (newMaxHealth != _manager.MaxHealth)
                    {
                        _manager.MaxHealth = newMaxHealth;
                    }
                }
                GUILayout.EndHorizontal();

                // Cooldown Field
                GUILayout.BeginHorizontal();
                GUILayout.Label("Cooldown");

                _textCooldown = GUILayout.TextField(_textCooldown);
                if (float.TryParse(_textCooldown, out float newCooldown))
                {
                    if (newCooldown != _manager.Cooldown) 
                    {
                        _manager.Cooldown = newCooldown;
                    }
                }

                GUILayout.EndHorizontal();

                // Controls to Change Damage
                GUILayout.BeginHorizontal();

                // Apply Damage Button
                if (GUILayout.Button("-")) 
                {
                     _manager.Damage(_healthAmount);
                }

                // Text field with the amount of health to apply (damage or heal)
                _textHealthAmount = GUILayout.TextField(_textHealthAmount);
                if (!float.TryParse(_textHealthAmount, out _healthAmount))
                {
                    _textHealthAmount = _healthAmount.ToString();
                }

                // Heal Button
                if (GUILayout.Button("+"))
                {
                    _manager.Heal(_healthAmount);
                }
                GUILayout.EndHorizontal();
            
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }

        private void OnValidate() 
        {
            if (_manager != null)
                return;

            _manager = FindObjectOfType<HealthManager>();
        }
    }
}