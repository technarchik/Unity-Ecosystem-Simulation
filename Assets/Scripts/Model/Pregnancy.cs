using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Model
{
    public class Pregnancy
    {
        private Animal mother;
        private Animal father;

        public float TimeUntilBirth { get; protected set; }

        public Pregnancy(Animal mother, Animal father)
        {
            TimeUntilBirth =
                mother is Predator
                ? ((TimeController.Instance.SECONDS_IN_A_DAY * 1) * WorldController.PredatorBreedingRate)
                : ((TimeController.Instance.SECONDS_IN_A_DAY * 1) * WorldController.PreyBreedingRate);

            this.mother = mother;
            this.father = father;
        }

        public void UpdatePregnancy(float deltatime) 
        {
            TimeUntilBirth -= deltatime;

            if (TimeUntilBirth <= 0) 
            {
                GiveBirth();
            }
        }

        private void GiveBirth() 
        {
            mother.GiveBirth(father);
        }

        private void MisCarry() 
        {
            mother.MisCarry();
        }
    }
}
