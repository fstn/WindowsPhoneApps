using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrokeThePig.UC.Weapons
{
    public class SimpleWeapon:BrokeThePigWeapon
    {
        private SoundController sc;
        public SimpleWeapon()
        {
            sc = new SoundController();
        }
    }
}
