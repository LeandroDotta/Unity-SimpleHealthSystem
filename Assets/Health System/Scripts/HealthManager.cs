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

        /// <summary>
        ///     <para>Event called when the health gets changed, either because it took damage or got healed.</para>
        ///     <para>
        ///         The first parameter is the current health value (after changing), and the second parameter is
        ///         the amount of health changed (negative value for damage, positive value for healing)
        ///     </para>
        /// </summary>
        public event UnityAction<float, float> OnHealthChange;
        /// <summary>Event called when the health reaches zero, indicating the target object died.</summary>
        public event UnityAction OnDie;
        /// <summary>Event called when the cooldown started.</summary>
        public event UnityAction OnCooldownStart;
        /// <summary>Event called when the cooldown ended.</summary>
        public event UnityAction OnCooldownEnd;

        /// <summary>Gets whether the Health System is currently cooling down.</summary>
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

        /// <summary>
        ///     <para>Gets or sets the cooldown time (in seconds)</para>
        ///     <para>Set it as 0 (zero) for no cooldown.</para>
        ///     <para>It only allows values above 0 (zero). If a value bellow 0 is passed to this property, it is set to 0 automatically.</para>
        /// </summary>
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

        /// <summary>
        ///     <para>Gets the Cooldown Timer. It represents the time left for the cooldown to end (if the it is currently cooling down).</para>
        ///     <para>When the Health system is not cooling down, it always returns <c>0</c> (zero).</para>
        /// </summary>
        public float CooldownTimer { get => IsCoolingDown ? _cooldownTimer : 0; }

        /// <summary>Gets the current Health value</summary>
        public float Health { get; private set; }

        /// <summary>
        ///     <para>Gets or sets this Health System maximum health.</para>
        ///     <para>
        ///         When setting the Max Health bellow the current health, the current <see cref="Health"/> property is
        ///         set with the Max Health value.
        ///     </para>
        /// </summary>
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

        /// <summary>
        /// Apply damage to this Health System.
        /// </summary>
        /// <param name="amount">The amount of damage to apply</param>
        /// <returns><c>true</c> if the damage is applied, or <c>false</c> if the Health System is currently cooling down.</returns>
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

        /// <summary>
        ///     <para>Adds health to this Health System</para>
        /// </summary>
        /// <param name="amount">The amount of health to add.</param>
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