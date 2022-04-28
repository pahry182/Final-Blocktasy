using UnityEngine;

namespace FinalBlocktasy
{
    public class Health
    {
        public float currentHp;
        public float maxHp;

        public Health(float _maxHp)
        {
            this.maxHp = _maxHp;
            this.currentHp = _maxHp;
        }

        public void SetHP(float value)
        {
            currentHp = Mathf.Clamp(value, 0, maxHp);
        }
    }
}

