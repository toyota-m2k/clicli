using CliCliBoy.interop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Point = System.Drawing.Point;

namespace CliCliBoy.model
{
    public enum ClickType
    {
        //UNDEFINED,
        NOOP,
        CLICK,
        DBLCLK,
        WHEEL,
        KEYPRESS,
    }

    public enum KeyType : int
    {
        ESC  = MouseEmulator.VK_ESCAPE,
        Home = MouseEmulator.VK_HOME,
        End  = MouseEmulator.VK_END,
        PageDown = MouseEmulator.VK_NEXT,
        PageUp = MouseEmulator.VK_PRIOR,
        Return = MouseEmulator.VK_RETURN,
    }

    public class TargetItem : Notifier
    {


        private bool mEnabled;
        private ClickType mType;
        private int mWait;
        private string mComment;
        private string mName;
        private int mRepeat;
        private bool mModified;
        private ScreenPoint mScreenPoint;
        private ClickCondition mCondition;
        private KeyType mPressKey;

        //private Point mPoint;
        //private Point mBasePoint;
        //private uint mRatio;

        //private static readonly string[] bpnamesAll = { "Type", "Enabled", "Wait", "Repeat", "Point", "AbsolutePoint","BasePoint", "Comment" };

        [System.Xml.Serialization.XmlIgnore]
        public bool IsModified
        {
            get { return mModified || mScreenPoint.IsModified || mCondition.IsModified; }
            set
            {
                mModified = value;
                if (!value)
                {
                    mScreenPoint.IsModified = false;
                    mCondition.IsModified = false;
                }
            }
        }


        // Enabled flag
        public bool Enabled
        {
            get { return mEnabled; }
            set
            {
                mEnabled = value;
                notify("Enabled");
                IsModified = true;
            }
        }

        public ClickType Type
        {
            get { return mType; }
            set
            {
                if (value != mType)
                {
                    mType = value;
                    notify("Type");
                    IsModified = true;
                }
            }
        }

        ///**
        // * set:
        // *      単純にズーム率をセットする。
        // *      AbsolutePointが変化する
        // */
        //public uint Ratio
        //{
        //    get { return (mRatio==0)?100:mRatio; }
        //    set
        //    {
        //        var v = value;
        //        if (v == 100) v = 0;
        //        if (v != mRatio)
        //        {
        //            mRatio = v;
        //            notify("Ratio");
        //            notify("AbsolutePoint");
        //            IsModified = true;
        //        }
        //    }
        //}

        ///**
        // * set:
        // *      単純にPointをセットする。
        // *      AbsolutePointが変化する
        // */
        //public Point Point
        //{
        //    get { return mPoint; }
        //    set
        //    {
        //        if (value != mPoint)
        //        {
        //            mPoint = value;
        //            notify("Point");
        //            notify("AbsolutePoint");
        //            IsModified = true;
        //        }
        //    }
        //}

        ///**
        // * set: 
        // *      単純にBasePointを変更する。
        // *      結果的に、AbsolutePointが平行移動する
        // * 
        // */
        //public Point BasePoint
        //{
        //    get { return mBasePoint; }
        //    set {
        //        if (value != mBasePoint)
        //        {
        //            mBasePoint = value;
        //            notify("BasePoint");
        //            notify("AbsolutePoint");
        //            IsModified = true;
        //        }
        //    }
        //}

        ///**
        // * ベースポイントをリセットして絶対座標に戻す。
        // * ターゲット設定系UIでは、絶対座標に統一する必要があるため用意した。利用は極めて限定的に。
        // */
        //public void ResetBasePoint() {
        //    Point abs = AbsolutePoint;
        //    mBasePoint = Point.Empty;
        //    mRatio = 0;
        //    if (abs != mPoint)
        //    {
        //        mPoint = abs;
        //        notify("Point");
        //        notify("BasePoint");
        //        notify("Ratio");
        //        IsModified = true;
        //    }
        //}

        ///**
        // * set:
        // *      現在設定されているBasePoint/Ratioの下で、指定されたAbsolutePointがクリックされるように、Pointを変える。
        // */
        //[System.Xml.Serialization.XmlIgnore]
        //public Point AbsolutePoint
        //{
        //    get
        //    {
        //        if (Ratio == 100)
        //        {
        //            return new Point(BasePoint.X + Point.X, BasePoint.Y + Point.Y);
        //        }
        //        else
        //        {
        //            double r = (double)Ratio / 100;
        //            return new Point(BasePoint.X + (int)Math.Round(Point.X * r), BasePoint.Y + (int)(Math.Round(Point.Y * r)));
        //        }
        //    }

        //    set
        //    {
        //        Point dp = new Point( value.X - mBasePoint.X, value.Y - mBasePoint.Y);
        //        if (Ratio != 100)
        //        {
        //            double r = (double)Ratio /100;
        //            dp.X = (int)Math.Round((double)dp.X / r);
        //            dp.Y = (int)Math.Round((double)dp.Y / r);
        //        }
        //        Point = dp;
        //    }
        //}

        private int mWheelAmount;
        public int WheelAmount
        {
            get { return mWheelAmount; }
            set
            {
                if (mWheelAmount != value)
                {
                    mWheelAmount = value;
                    notify("WheelAmount");
                    IsModified = true;
                }
            }
        }


        public string Comment
        {
            get { return mComment; }
            set
            {
                mComment = value;
                notify("Comment");
                IsModified = true;
            }
        }

        public string Name
        {
            get { return mName; }
            set
            {
                mName = value;
                notify("Name");
                IsModified = true;
            }
        }

        public int Repeat
        {
            get { return (mRepeat <= 0) ? 1 : mRepeat; }
            set
            {
                mRepeat = value;
                notify("Repeat");
                IsModified = true;
            }
        }


        public ClickCondition Condition
        {
            get { return mCondition; }
            set
            {
                if (!mCondition.Equals(value))
                {
                    mCondition.CopyFrom(value);
                    IsModified = true;
                    notify("Condition");
                }
            }
        }

        /**
         * クリック位置を保持するプロパティ
         * XAMLにBindするためにpublicにしている。ClickConditionとの整合を保つため、Projectなどから、このプロパティを直接操作してはならない。
         * 
         * C#コードから操作する場合は、
         * SetRatio(),SetBasePointKeepAbs(), TranslateBasePoint(), SetBasePointAndRatioKeepAbs()などの更新用メソッドを使うか、
         * Clickerプロパティ経由でAbsolutePointを更新する。
         */
        public ScreenPoint ScreenPoint
        {
            get { return mScreenPoint; }
            set
            {
                if (!mScreenPoint.Equals(value))
                {
                    mScreenPoint.CopyFrom(value);
                    IsModified = true;
                }
            }
        }

        public void SetRatio(uint ratio)
        {
            ScreenPoint.Ratio = ratio;
            Condition.ScreenPoint.Ratio = ratio;
        }

        public void SetBasePointKeepAbs(Point bp)
        {
            Point abs = ScreenPoint.AbsolutePoint;
            ScreenPoint.BasePoint = bp;
            ScreenPoint.AbsolutePoint = abs;

            abs = Condition.ScreenPoint.AbsolutePoint;
            Condition.ScreenPoint.BasePoint = bp;
            Condition.ScreenPoint.AbsolutePoint = abs;
        }

        public void TranslateBasePoint(Point bp)
        {
            ScreenPoint.BasePoint = bp;
            Condition.ScreenPoint.BasePoint = bp;
        }

        public void SetBasePointAndRatioKeepAbs(Point bp, uint ratio)
        {
            Point abs = ScreenPoint.AbsolutePoint;
            ScreenPoint.BasePoint = bp;
            ScreenPoint.Ratio = ratio;
            ScreenPoint.AbsolutePoint = abs;

            abs = Condition.ScreenPoint.AbsolutePoint;
            Condition.ScreenPoint.BasePoint = bp;
            Condition.ScreenPoint.Ratio = ratio;
            Condition.ScreenPoint.AbsolutePoint = abs;
        }

        public IClicker Clicker
        {
            get { return ScreenPoint; }
        }


        //public String TypeName
        //{
        //    get
        //    {
        //        switch(mType) {
        //            default:
        //            case ClickType.NOOP:
        //                return "Noop";
        //            case ClickType.CLICK:
        //                return "Click";
        //            case ClickType.DBLCLK:
        //                return "Double Click";
        //        }
        //    }
        //}

        //public Visibility NeedsPoint
        //{
        //    get { return (mType == ClickType.NOOP) ? Visibility.Hidden : Visibility.Visible; }
        //}

        public int Wait
        {
            get { return (mWait < Defs.MINIMUM_WAIT) ? Defs.MINIMUM_WAIT : mWait; }
            set
            {
                mWait = value;
                notify("Wait");
                IsModified = true;
            }
        }

        [System.Xml.Serialization.XmlIgnore]
        public int _RawWait
        {
            get { return mWait; }
        }

        [System.Xml.Serialization.XmlIgnore]
        public int Timing { get; set; }     // for marge operation

        [System.Xml.Serialization.XmlIgnore]
        public string EmulateKeyTag
        {
            get; set;
        }

        public KeyType PressKey
        {
            get { return mPressKey; }
            set
            {
                mPressKey = value;
                notify("PressKey");
                notify("KeyShortLabel");
                IsModified = true;
            }
        }

        [System.Xml.Serialization.XmlIgnore]
        public string KeyShortLabel
        {
            get
            {
                switch (mPressKey)
                {
                    case KeyType.ESC: return "ESC";
                    case KeyType.End: return "End";
                    case KeyType.Home: return "Home";
                    case KeyType.PageDown: return "PgDn";
                    case KeyType.PageUp: return "PgUp";
                    case KeyType.Return: return "Rtrn";
                    default: return "";
                }
            }
        }

        public TargetItem()
        {
            mType = ClickType.CLICK;
            mEnabled = true;
            mWait = Defs.INITIAL_WAIT;
            mRepeat = 1;
            mComment = "";
            mName = "";
            mWheelAmount = 0;
            mCondition = new ClickCondition();
            mScreenPoint = new ScreenPoint();
            mPressKey = KeyType.ESC;
            IsModified = false;
            //mPoint = new Point();
            //mBasePoint = new Point();
            //mRatio = 0;
        }


        //public TargetItem(ClickType type, Point pos, int wait, int repeat, string comment, int wheelAmount)
        //{
        //    mType = type;
        //    mWait = wait;
        //    mEnabled = true;
        //    mRepeat = 1;
        //    mComment = comment;
        //    mWheelAmount = wheelAmount;
        //    IsModified = false;
        //    //mPoint = new Point(pos.X, pos.Y);
        //    //mBasePoint = new Point(0, 0);
        //    //mRatio = 0;
        //}

        public TargetItem(TargetItem s)
        {
            mType = s.mType;
            mWait = s.mWait;
            mEnabled = s.mEnabled;
            mRepeat = s.mRepeat;
            mComment = s.mComment;
            mName = s.mName;
            mWheelAmount = s.mWheelAmount;

            mCondition = new ClickCondition(s.Condition);
            mScreenPoint = new ScreenPoint(s.ScreenPoint);
            mPressKey = s.mPressKey;
            IsModified = false;
            //mPoint = s.mPoint;
            //mBasePoint = s.mBasePoint;
            //mRatio = s.mRatio;
        }

        //public void Clear()
        //{
        //    Type = ClickType.CLICK;
        //    Enabled = true;
        //    Wait = 1000;
        //    Repeat = 1;
        //    Comment = "";
        //    WheelAmount = 0;
        //    IsModified = false;
        //    //Point = Point.Empty;
        //    //BasePoint = Point.Empty;
        //    //Ratio = 0;
        //}

        public void CopyFrom(TargetItem s)
        {
            Type = s.Type;
            Enabled = s.Enabled;
            Wait = s.Wait;
            Repeat = s.Repeat;
            Comment = s.Comment;
            Name = s.Name;
            WheelAmount = s.WheelAmount;

            Condition = s.Condition;
            ScreenPoint = s.ScreenPoint;
            PressKey = s.PressKey;

            IsModified = true;
            //Point = s.Point;
            //BasePoint = s.BasePoint;
            //Ratio = s.Ratio;
        }

        public TargetItem Clone()
        {
            return new TargetItem(this);
        }

        public override string ToString()
        {
            return String.Format("TargetItem: {0} ({1},{2}) : {3}", mType.ToString(), mScreenPoint.Point.X, mScreenPoint.Point.Y, mComment);
        }

    }

}
