using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliCliBoy.model
{
    public class IntRange
    {
        public int Min { get; set; }
        public int Max { get; set; }

        public int Median
        {
            get
            {
                return (int)Math.Round((double)(Min + Max) / 2);
            }
        }

        public IntRange()
        {
            Clear();
        }

        public void Clear()
        {
            Min = int.MaxValue;
            Max = int.MinValue;
        }

        public bool IsValid
        {
            get
            {
                return Min <= Max;
            }
        }

        public void AddValue(int value)
        {
            if (value < Min)
            {
                Min = value;
            }
            if( value > Max)
            {
                Max = value;
            }
        }

        public bool IsInRange(int value)
        {
            return Min <= value && value <= Max;
        }
    }
}
