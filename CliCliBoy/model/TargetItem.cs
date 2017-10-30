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

    public class TargetItem : Notifier, IVersioning
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
        private ConditionList mConditionList;
        private KeyType mPressKey;

        //private Point mPoint;
        //private Point mBasePoint;
        //private uint mRatio;

        //private static readonly string[] bpnamesAll = { "Type", "Enabled", "Wait", "Repeat", "Point", "AbsolutePoint","BasePoint", "Comment" };

        [System.Xml.Serialization.XmlIgnore]
        public bool IsModified
        {
            get { return mModified || mScreenPoint.IsModified || mConditionList.IsModified; }
            set
            {
                mModified = value;
                if (!value)
                {
                    mScreenPoint.IsModified = false;
                    mConditionList.IsModified = false;
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

        public ConditionList ConditionList
        {
            get { return mConditionList; }
            set
            {
                if (!mConditionList.Equals(value))
                {
                    mConditionList.CopyFrom(value);
                    notify("ConditionList");
                    IsModified = true;
                }
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
            ConditionList.SetRatio(ratio);
        }

        public void SetBasePointKeepAbs(Point bp)
        {
            Point abs = ScreenPoint.AbsolutePoint;
            ScreenPoint.BasePoint = bp;
            ScreenPoint.AbsolutePoint = abs;

            ConditionList.SetBasePointKeepAbs(bp);
        }

        public void TranslateBasePoint(Point bp)
        {
            ScreenPoint.BasePoint = bp;

            ConditionList.TranslateBasePoint(bp);
        }

        public void SetBasePointAndRatioKeepAbs(Point bp, uint ratio)
        {
            Point abs = ScreenPoint.AbsolutePoint;
            ScreenPoint.BasePoint = bp;
            ScreenPoint.Ratio = ratio;
            ScreenPoint.AbsolutePoint = abs;

            ConditionList.SetBasePointAndRatioKeepAbs(bp, ratio);
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
            mConditionList = new ConditionList();
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

            mConditionList = new ConditionList(s.ConditionList);
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

            ConditionList = s.ConditionList;
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

        private int mUtilizationCount = 0;
        [System.Xml.Serialization.XmlIgnore]
        public int UtilizationCount
        {
            get
            {
                return mUtilizationCount;
            }
            set
            {
                mUtilizationCount = value;
                notify("UtilizationCount");
            }
        }

        public void Used()
        {
            UtilizationCount++;
        }

        public void OnVersionUp(int fromVersion)
        {
            if(null== Condition)
            {
                return;
            }

            if(Condition.Type!=ClickCondition.ConditionType.NONE && !ConditionList.HasCondition)
            {
                var c = new ConditionList.Condition();
                if (Condition.Type == ClickCondition.ConditionType.WAIT)
                {
                    ConditionList.Type = ConditionList.ActionType.WAIT;
                }
                else if (Condition.Type == ClickCondition.ConditionType.SKIP)
                {
                    ConditionList.Type = ConditionList.ActionType.SKIP;
                }

                c.ColorRange = Condition.ColorRange;
                c.ScreenPoint = Condition.ScreenPoint;
                c.IsValid = true;
                c.IsModified = true;
                ConditionList.Add(c);
            }
        }
    }
}
