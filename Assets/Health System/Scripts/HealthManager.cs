using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace SimpleHealth
{
    [System.Serializable]
    public class HealthManager : MonoBehaviour
    {
        [System.Serializable]
        private struct Events 
        {
            public UnityEvent<float, float> onHealthChange;
            public UnityEvent onDie;
            public UnityEvent onCooldownStart;
            public UnityEvent onCooldownEnd;
        }

        [SerializeField] private float _maxHealth;

        [Tooltip("Cooldown time, in seconds. Set '0' for no cooldown")]
        [SerializeField] private float _cooldown;

        [Tooltip("The amount of Health to start with.")]
        [SerializeField] private float _startingHealth;

        [SerializeField] private Events _events;

        private float _cooldownTimer;
        private bool _coolingDown;

        public event UnityAction<float, float> OnHealthChange;
        public event UnityAction OnDie;
        public event UnityAction OnCooldownStart;
        public event UnityAction OnCooldownEnd;

        public bool IsCoolingDown
        {
            get => _coolingDown;
            private set
            {
                if (_coolingDown == value)
                    return;

                _coolingDown = value;

                if (_coolingDown)
                    Invoke_OnCooldownStart();
                else
                    Invoke_OnCooldownEnd();
            }
        }

        public float Cooldown { 
            get => _cooldown; 
            set
            {
                if (value < 0)
                    _cooldown = 0;
                else
                    _cooldown = value;
            }
        }
        public float CooldownTimer { get => IsCoolingDown ? _cooldownTimer : 0; }

        public float Health { get; private set; }
        public float MaxHealth
        {
            get => _maxHealth; 
            set
            {
                _maxHealth = value;
                if (Health > _maxHealth)
                    Health = _maxHealth;
            }
        }

        private void Awake()
        {
            if (_startingHealth > 0)
            {
                Health = _startingHealth <= _maxHealth ? _startingHealth : _maxHealth;
            }
            else
            {
                Health = _maxHealth;
            }
        }

        public bool Damage(float amount)
        {
            if (IsCoolingDown)
                return false;

            Health -= amount;

            if (Health <= 0)
            {
                Health = 0;
                Invoke_OnDie();
                return true;
            }
            else
            {
                Invoke_OnHealthChange(Health, -amount);
            }

            StartCoroutine("CooldownCoroutine");
            return true;
        }

        public void Heal(float amount)
        {
            Health += amount;

            if (Health >= _maxHealth)
            {
                Health = _maxHealth;
            }

            Invoke_OnHealthChange(Health, amount);
        }

        private void Invoke_OnHealthChange(float health, float delta)
        {
            _events.onHealthChange?.Invoke(health, delta);
            OnHealthChange?.Invoke(health, delta);
        }

        private void Invoke_OnDie()
        {
            _events.onDie?.Invoke();
            OnDie?.Invoke();
        }

        private void Invoke_OnCooldownStart()
        {
            _events.onCooldownStart?.Invoke();
            OnCooldownStart?.Invoke();
        }

        private void Invoke_OnCooldownEnd()
        {
            _events.onCooldownEnd?.Invoke();
            OnCooldownEnd?.Invoke();
        }

        private IEnumerator CooldownCoroutine()
        {
            if (_cooldown <= 0)
                yield break;

            _cooldownTimer = _cooldown;

            IsCoolingDown = true;
            while (_cooldownTimer > 0)
            {
                yield return null;
                _cooldownTimer -= Time.deltaTime;
            }
            IsCoolingDown = false;
        }
    }
}