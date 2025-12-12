using UnityEngine;
using UnityEngine.UI;

namespace LifeSystem
{
    public class LifeSystem : MonoBehaviour
    {
        [Header("Life Parameters ")]
        [SerializeField] private int maxLife;
        [SerializeField] private Image healthBar;
        
        private int _currentLife;
        
        private void Awake()
        {
            ResetLife();
        }
        
        //Life System Voids
        public void ResetLife()
        {
            _currentLife = maxLife;
        }
        public void TakeDamage(int damage)
        {
            _currentLife -= damage;
            currenthealth();
        }

        public void TakeHeal(int heal)
        {
            
            _currentLife += heal;
            if (_currentLife > maxLife) _currentLife = maxLife;
            currenthealth();
        }

        public int currenthealth()
        {
            return _currentLife;
        }
        
        //healtbarLogic
        private void Update()
        {
            if (healthBar != null)
            {
                healthBar.fillAmount = (float)_currentLife / maxLife;
            }
        }
    }
}