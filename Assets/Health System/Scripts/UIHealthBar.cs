using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SimpleHealth.UI
{
    public class UIHealthBar : MonoBehaviour
    {
        [System.Serializable]
        private class AnimationAttibutes 
        {
            public bool enabled = true;
            [Range(0.1f, 1f)] public float interval = 0.3f;
            public AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);
        }

        [SerializeField] private Image _bar;
        [field: SerializeField] public HealthManager HealthManager { get; set; }
        [SerializeField] private AnimationAttibutes _animation = new AnimationAttibutes();


        private void Start()
        {
            if (HealthManager == null)
            {
                Debug.LogError("The Health Bar component could not be initialized without a HealthManager component reference.");
                return;
            }

            SetHealth(HealthManager.Health);
            HealthManager.OnHealthChange += HealthChange;
        }

        private void OnDestroy() 
        {
            if (HealthManager == null)
                return;

            HealthManager.OnHealthChange -= HealthChange;
        }

        /// <summary>
        /// Sets the amount of health and updates the Health's bar.
        /// </summary>
        /// <param name="health">The current amount of health.</param>
        public void SetHealth(float health)
        {
            if (HealthManager == null)
            {
                Debug.LogWarning("Can't set Health Bar's value, it was not properly initialized.");
                return;
            }

            // Updates the bar with or without animation
            if (_animation.enabled)
            {
                StopCoroutine("AnimateBarCoroutine");
                StartCoroutine("AnimateBarCoroutine", health);
            }
            else 
            {
                _bar.fillAmount = health / HealthManager.MaxHealth;
            }
        }

        private void HealthChange(float health, float amount)
        {
            SetHealth(health);
        }

        private IEnumerator AnimateBarCoroutine(float health)
        {
            float currentFill = _bar.fillAmount;
            float targetFill = health / HealthManager.MaxHealth;

            float timer = 0;
            while (_bar.fillAmount != targetFill)
            {
                yield return null;
                timer += (Time.deltaTime / _animation.interval);
                _bar.fillAmount = Mathf.Lerp(currentFill, targetFill, _animation.curve.Evaluate(timer));
            }
        }
    }
}