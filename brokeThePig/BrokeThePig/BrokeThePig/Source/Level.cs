using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrokeThePig.Source
{
    public class Level
    {
        public int MoneyToWin { get; set; }
        public int NumberOfTapToDo { get; set; }
        public String GiftImage { get; set; }
        public Level(int MoneyToWin, int NumberOfTapToDo, String GiftImage)
        {
            this.MoneyToWin = MoneyToWin;
            this.NumberOfTapToDo = NumberOfTapToDo;
            this.GiftImage = GiftImage;
        }
    }
}
