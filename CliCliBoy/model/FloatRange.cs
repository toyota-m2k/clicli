using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliCliBoy.model
{
    public class FloatRange
    {
        public float Min { get; set; }
        public float Max { get; set; }

        public virtual float Median
        {
            get
            {
                return (Min + Max) / 2f;
            }
        }

        public FloatRange()
        {
            Clear();
        }

        public void Clear()
        {
            Min = float.MaxValue;
            Max = float.MinValue;
        }

        public bool IsValid
        {
            get
            {
                return Min <= Max;
            }
        }

        public virtual void AddValue(float value)
        {
            if (value < Min)
            {
                Min = value;
            }
            if (value > Max)
            {
                Max = value;
            }
        }

        public virtual bool IsInRange(float value)
        {
            return Min <= value && value <= Max;
        }

        public bool Equals(FloatRange f)
        {
            return Max == f.Max && Min == f.Min;
        }

        public void CopyFrom(FloatRange s)
        {
            Max = s.Max;
            Min = s.Min;
        }
    }

    public class AngleRange : FloatRange
    {
        public override void AddValue(float value)
        {
            if(Min==float.MaxValue || Max==Min )
            {
                // 最初の１つ目 or ２つ目
                base.AddValue(value);
            }
            else
            {
                // ３つ目以降
                if (Max - Min <= 180f)
                {
                    if(value < Min)
                    {
                        Min = value;
                    }
                    else if(Max<value)
                    {
                        Max = value;
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    var d = Min + 360f - Max;
                    //var min = 0; // Max - Max;
                    var v = value - Max;
                    if(v<0)
                    {
                        v += 360f;
                    }

                    if( v>d )
                    {
                        if(v+d<180)
                        {
                            Min = value;
                        }
                        else
                        {
                            Max = value;
                        }
                        Debug.Assert(Max >= Min);
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }


        public override float Median
        {
            get
            {
                float v = Max - Min;
                if (Max - Min < 180)
                {
                    return (Min + Max ) /2f;
                }
                else
                {
                    return (Min + Max) / 2f + 180f;
                }
            }
        }

        public override bool IsInRange(float value)
        {
            if (Max - Min <= 180f)
            {
                return Min <= value && value <= Max;
            }
            else
            {
                var d = Min + 360f - Max;
                var v = value - Max;
                if( v<0 )
                {
                    v += 360f;
                }
                return v <= d;
            }
        }
    }


}
