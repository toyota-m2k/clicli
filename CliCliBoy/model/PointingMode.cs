using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Point = System.Drawing.Point;

namespace CliCliBoy.model
{
    public class PointingMode : Notifier
    {
        bool mRelative;
        Point mBasePoint;

        public bool IsRelative
        {
            get { return mRelative; }
            set { 
                mRelative = value;
                notify("IsRelative");
                notify("IsAbsolute");
            }
        }


        [System.Xml.Serialization.XmlIgnore]
        public bool IsAbsolute
        { 
            get { return !IsRelative; } 
            set { IsRelative = !value; } 
        }

        public Point BasePoint {
            get { return mBasePoint; }
            set { 
                mBasePoint = value;
                notify("BasePoint");
            }
        }

        public PointingMode()
        {
            mRelative = true;
            mBasePoint = new Point();
        }

        public void Clear()
        {
            IsRelative = true;
            BasePoint = new Point();
        }

        public void CopyFrom(PointingMode mode)
        {
            IsRelative = mode.IsRelative;
            BasePoint = mode.BasePoint;
        }

        public bool IsSame(PointingMode mode)
        {
            return mode.IsRelative == IsRelative && BasePoint == mode.BasePoint;
        }

    }

}
