using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SimpleHealth.Samples
{
    public class HealthText : MonoBehaviour
    {
        private TMP_Text text;

        private void Start() 
        {
            text = GetComponentInChildren<TMP_Text>();    
            text.color = Color;
            text.text = Text;
            Invoke("SelfDestroy", 2);
        }

        public Color Color { get; set; } = Color.white;
        public string Text { get; set; } = "000";

        private void SelfDestroy()
        {
            Destroy(gameObject);
        }
    }
}