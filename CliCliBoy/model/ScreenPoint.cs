using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Point = System.Drawing.Point;

namespace CliCliBoy.model
{
    public interface IClicker
    {
        Point ClickPoint { get; set; }
    }


    public class ScreenPoint    : Notifier, IClicker
    {
        private Point mPoint;
        private Point mBasePoint;
        private uint mRatio;

        [System.Xml.Serialization.XmlIgnore]
        public bool IsModified { get; set; }


        /**
         * set:
         *      単純にズーム率をセットする。
         *      AbsolutePointが変化する
         */
        public uint Ratio
        {
            get { return (mRatio == 0) ? 100 : mRatio; }
            set
            {
                var v = value;
                if (v == 100) v = 0;
                if (v != mRatio)
                {
                    mRatio = v;
                    notify("Ratio");
                    notify("AbsolutePoint");
                    IsModified = true;
                }
            }
        }

        /**
         * set:
         *      単純にPointをセットする。
         *      AbsolutePointが変化する
         */
        public Point Point
        {
            get { return mPoint; }
            set
            {
                if (value != mPoint)
                {
                    mPoint = value;
                    notify("Point");
                    notify("AbsolutePoint");
                    IsModified = true;
                }
            }
        }

        /**
         * set: 
         *      単純にBasePointを変更する。
         *      結果的に、AbsolutePointが平行移動する
         * 
         */
        public Point BasePoint
        {
            get { return mBasePoint; }
            set
            {
                if (value != mBasePoint)
                {
                    mBasePoint = value;
                    notify("BasePoint");
                    notify("AbsolutePoint");
                    IsModified = true;
                }
            }
        }

        /**
         * ベースポイントをリセットして絶対座標に戻す。
         * ターゲット設定系UIでは、絶対座標に統一する必要があるため用意した。利用は極めて限定的に。
         */
        public void ResetBasePoint()
        {
            Point abs = AbsolutePoint;
            mBasePoint = Point.Empty;
            mRatio = 0;
            if (abs != mPoint)
            {
                mPoint = abs;
                notify("Point");
                notify("BasePoint");
                notify("Ratio");
                IsModified = true;
            }
        }

        /**
         * set:
         *      現在設定されているBasePoint/Ratioの下で、指定されたAbsolutePointがクリックされるように、Pointを変える。
         */
        [System.Xml.Serialization.XmlIgnore]
        public Point AbsolutePoint
        {
            get
            {
                if (Ratio == 100)
                {
                    return new Point(BasePoint.X + Point.X, BasePoint.Y + Point.Y);
                }
                else
                {
                    double r = (double)Ratio / 100;
                    return new Point(BasePoint.X + (int)Math.Round(Point.X * r), BasePoint.Y + (int)(Math.Round(Point.Y * r)));
                }
            }

            set
            {
                Point dp = new Point(value.X - mBasePoint.X, value.Y - mBasePoint.Y);
                if (Ratio != 100)
                {
                    double r = (double)Ratio / 100;
                    dp.X = (int)Math.Round((double)dp.X / r);
                    dp.Y = (int)Math.Round((double)dp.Y / r);
                }
                Point = dp;
            }
        }

        public ScreenPoint()
        {
            mPoint = new Point();
            mBasePoint = new Point();
            mRatio = 0;
            IsModified = false;
        }

        public ScreenPoint(ScreenPoint s)
        {
            Point = s.Point;
            BasePoint = s.BasePoint;
            Ratio = s.Ratio;
            IsModified = false;
        }

        public void Clear()
        {
            Point = Point.Empty;
            BasePoint = Point.Empty;
            mRatio = 0;
            IsModified = true;
        }

        public void CopyFrom(ScreenPoint s)
        {
            if (null == s)
            {
                Clear();
                return;
            }
            Point = s.Point;
            BasePoint = s.BasePoint;
            Ratio = s.Ratio;
            IsModified = true;
        }

        public bool Equals(ScreenPoint s)
        {
            return null!=null && s.Point == Point && BasePoint == s.BasePoint && Ratio == s.Ratio;
        }

        public static bool operator ==(ScreenPoint a, ScreenPoint b)
        {
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }
            else
            {
                return a.Equals(b);
            }
        }
        public static bool operator !=(ScreenPoint a, ScreenPoint b)
        {
            return !(a == b);
        }


        public ScreenPoint Clone()
        {
            return new ScreenPoint(this);
        }

        Point IClicker.ClickPoint
        {
            get
            {
                return AbsolutePoint;
            }

            set
            {
                AbsolutePoint = value;
            }
        }
    }
}
