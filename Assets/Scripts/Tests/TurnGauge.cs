using UnityEngine;

namespace FinalBlocktasy
{
    public class TurnGauge
    {
        public float currentGauge;
        public float maxGauge = 100;
        public bool isTakingTurn;

        public TurnGauge()
        {
            this.currentGauge = 0;
            this.isTakingTurn = false;
        }

        public void SetTurnGauge(float value)
        {
            currentGauge = Mathf.Clamp(value, 0, maxGauge);
        }

        public void ThisUnitTurn()
        {
            if (currentGauge == maxGauge)
            {
                currentGauge = 0;
                isTakingTurn = true;
            }
        }
    }
}

