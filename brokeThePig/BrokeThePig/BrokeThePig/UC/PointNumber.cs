using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrokeThePig.UC
{
    public class PointNumber
    {
        public int Number { get; set; }
        public PointNumber(int Number)
        {
            this.Number = Number;
        }

        public int FightNumber(BrokeThePigWeapon Weapon)
        {
            if (Weapon != null)
                Number = Weapon.Fight(Number);
            return Number;
        }

    }
}
