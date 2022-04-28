using UnityEngine;

namespace FinalBlocktasy
{
    public class Level
    {
        public float currentXp;
        public float maxXp;
        public int level;

        public Level()
        {
            this.currentXp = 0;
            this.maxXp = 100;            
            this.level = 1;
        }

        public void AddXp(float value)
        {
            if (value < 1) return;

            currentXp += value;
            while (currentXp >= maxXp)
            {
                level++;
                currentXp -= maxXp;
                maxXp = 100 + (20 * (level - 1));
            }
        }
    }
}

