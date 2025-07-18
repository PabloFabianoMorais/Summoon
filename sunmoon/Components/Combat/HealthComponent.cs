using System;
using sunmoon.Core.ECS;

namespace sunmoon.Components.Combat
{
    public class HealthComponent : Component
    {
        public float MaxHealth { get; set; }
        public float CurrentHealth { get; private set; }
        public bool IsDead { get; private set; }
        public event Action OnDeath;
        public event Action<float, float> OnHealthChanged;

        public override void Initialize()
        {
            base.Initialize();
            CurrentHealth = MaxHealth;
            IsDead = false;
        }

        public void TakeDemage(float amount)
        {
            if (IsDead) return;

            CurrentHealth -= amount;
            CurrentHealth = Math.Max(0, CurrentHealth);
            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
            if (CurrentHealth <= 0)
                Die();
        }

        private void Die()
        {
            if (IsDead) return;

            IsDead = true;
            OnDeath?.Invoke();
        }
    }
}