using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoClicker.model
{
    public class ClickCondition : Notifier
    {
        public enum ConditionType{
            NONE,   // 条件設定は無効
            WAIT,   // 条件を満たすまで待機
            SKIP,   // 条件を満たさなければ、次へ進む
        }

        private ConditionType mConditionType;
        private HSVColorRange mColorRange;
        private ScreenPoint mScreenPoint;
        private bool mModified;

        public ClickCondition()
        {
            mConditionType = ConditionType.NONE;
            mColorRange = new HSVColorRange();
            mScreenPoint = new ScreenPoint();
            mModified = false;
        }

        public ClickCondition(ClickCondition s)
        {
            Type = s.Type;
            mColorRange = s.ColorRange.Clone();
            mScreenPoint = s.ScreenPoint.Clone();
            IsModified = false;

        }

        public void Clear()
        {
            Type = ConditionType.NONE;
            ColorRange.Clear();
            mScreenPoint = new ScreenPoint();
        }

        public void CopyFrom(ClickCondition s)
        {
            if (null == s)
            {
                Clear();
                return;
            }
            Type = s.Type;
            ColorRange = s.ColorRange;
            ScreenPoint = s.ScreenPoint;
            IsModified = true;
        }
        public ClickCondition Clone()
        {
            return new ClickCondition(this);
        }

        public bool Equals(ClickCondition s)
        {
            return s.Type == Type && s.ColorRange.Equals(s.ColorRange) && s.ScreenPoint.Equals(ScreenPoint);
        }

        [System.Xml.Serialization.XmlIgnore]
        public bool IsModified
        {
            get { return mModified || mScreenPoint.IsModified; }
            set
            {
                mModified = value;
                if (!value)
                {
                    mScreenPoint.IsModified = false;
                }
            }
        }


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


        public ConditionType Type
        {
            get { return mConditionType; }
            set
            {
                if (mConditionType != value)
                {
                    mConditionType = value;
                    notify("Type");
                    notify("ConditionSummary");
                    IsModified = true;
                }
            }
        }


        public HSVColorRange ColorRange
        {
            get { return mColorRange; }
            set
            {
                if (!mColorRange.Equals(value))
                {
                    mColorRange.CopyFrom(value);
                    notify("ColorRange");
                }
            }
        }

        [System.Xml.Serialization.XmlIgnore]
        public string ConditionSummary
        {
            get
            {
                switch (Type)
                {
                    default:
                    case ConditionType.NONE:
                        return "None";
                    case ConditionType.SKIP:
                        return "Skip";
                    case ConditionType.WAIT:
                        return "Wait";
                }
            }
        }


        public bool Decide(IDebugOutput dbgout= null)
        {
            return TestAt(ScreenPoint.AbsolutePoint, dbgout);
            //try
            //{
            //    Bitmap myBitmap = new Bitmap(1, 1);
            //    Graphics g = Graphics.FromImage(myBitmap);
            //    g.CopyFromScreen(ScreenPoint.AbsolutePoint, new Point(0, 0), new System.Drawing.Size(1, 1));
            //    var color = myBitmap.GetPixel(0, 0);
            //    return ColorRange.IsInRange(color);
            //}
            //catch (Win32Exception ex)
            //{
            //    return false;
            //}
        }

        public bool TestAt(Point absPoint, IDebugOutput dbgout=null)
        {
            try
            {
                using (Bitmap myBitmap = new Bitmap(1, 1))
                {
                    using (Graphics g = Graphics.FromImage(myBitmap))
                    {
                        g.CopyFromScreen(absPoint, new Point(0, 0), new System.Drawing.Size(1, 1));
                        var color = myBitmap.GetPixel(0, 0);
                        return ColorRange.IsInRange(color, dbgout);
                    }
                }
            }
            catch (Win32Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return false;
            }
            catch(Exception e2)
            {
                //Debug.WriteLine(e2.ToString());
                dbgout.Put(e2.ToString());
                dbgout.Put(absPoint.ToString());
                return false;
            }
        }
    }
}
