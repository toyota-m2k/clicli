using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace CliCliBoy.model
{
    public class ConditionList : Notifier
    {
        #region Type Definitions
        /**
         * Condition Class
         */
        public class Condition : Notifier
        {
            #region Type Definition

            public enum ActionType
            {
                NONE,   // 条件設定は無効
                WAIT,   // 条件を満たすまで待機
                SKIP,   // 条件を満たさなければ、次へ進む
            }

            #endregion

            #region Private Fields

            private ActionType mType;
            private bool mNegation;
            private ScreenPoint mScreenPoint;
            private HSVColorRange mColorRange;
            private bool mModified = false;
            
            #endregion

            #region Construction & Duplicating

            public Condition()
            {
                mType = ActionType.NONE;
                mNegation = false;
                mColorRange = new HSVColorRange();
                mScreenPoint = new ScreenPoint();
                mModified = false;
            }

            public Condition(Condition s)
            {
                mType = s.Type;
                mNegation = s.Negation;
                mColorRange = s.ColorRange.Clone();
                mScreenPoint = s.ScreenPoint.Clone();
                mModified = false;
            }

            public void Clear()
            {
                Type = ActionType.NONE;
                mNegation = false;
                ColorRange.Clear();
                mScreenPoint = new ScreenPoint();
            }

            public void CopyFrom(Condition s)
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

            public Condition Clone()
            {
                return new Condition(this);
            }
            #endregion

            #region Comparison

            public bool Equals(Condition c)
            {
                if(null==c)
                {
                    return false;
                }
                return c.Type == Type && c.ScreenPoint == ScreenPoint && c.ColorRange == ColorRange && c.Negation == Negation;
            }

            public override bool Equals(Object c)
            {
                return Equals(c as Condition);
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            public static bool operator == (Condition a, Condition b)
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

            public static bool operator !=(Condition a, Condition b)
            {
                return !(a == b);
            }

            #endregion

            #region Properties

            public ActionType Type
            {
                get { return mType; }
                set
                {
                    if (mType != value)
                    {
                        mType = value;
                        notify("Type");
                        notify("ConditionSummary");
                        IsModified = true;
                    }
                }
            }

            public bool Negation
            {
                get { return mNegation; }
                set
                {
                    if (mNegation != value)
                    {
                        mNegation = value;
                        IsModified = true;
                        notify("Negation");
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
                        notify("ScreenPoint");
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
                        IsModified = true;
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
                        case ActionType.NONE:
                            return "None";
                        case ActionType.SKIP:
                            return "Skip";
                        case ActionType.WAIT:
                            return "Wait";
                    }
                }
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
            #endregion

            #region Condition Test
            public bool Decide(StringBuilder sb = null)
            {
                return TestAt(ScreenPoint.AbsolutePoint, sb);
            }

            public bool TestAt(Point absPoint, StringBuilder sb = null)
            {
                try
                {
                    using (Bitmap myBitmap = new Bitmap(1, 1))
                    {
                        using (Graphics g = Graphics.FromImage(myBitmap))
                        {
                            g.CopyFromScreen(absPoint, new Point(0, 0), new System.Drawing.Size(1, 1));
                            var color = myBitmap.GetPixel(0, 0);
                            var result = ColorRange.IsInRange(color, sb);
                            return mNegation ? !result : result;
                        }
                    }
                }
                catch (Win32Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                    return false;
                }
                catch (Exception e2)
                {
                    //Debug.WriteLine(e2.ToString());
                    Globals.Logger.Output(e2.ToString());
                    Globals.Logger.Output(absPoint.ToString());
                    return false;
                }
            }
            #endregion

        }
        public enum ConditionCombination
        {
            CMB_AND,
            CMB_OR,
        }
        #endregion

        #region Private Fields

        private ObservableCollection<Condition> mList;
        private ConditionCombination mCombination;
        private bool mModified;
        private static readonly Condition sDefaultCondition = new Condition();

        #endregion

        #region Construction & Duplication

        public ConditionList()
        {
            mList = new ObservableCollection<Condition>();
            mCombination = ConditionCombination.CMB_AND;
            mModified = false;
        }
        public ConditionList(ConditionList s)
        {
            mList = new ObservableCollection<Condition>();
            mCombination = ConditionCombination.CMB_AND;
            CopyFrom(s);
            mModified = false;
        }

        public void CopyFrom(ConditionList s)
        {
            if(null==s)
            {
                Clear();
                return;
            }
            mList.Clear();
            foreach(var c in s.List)
            {
                mList.Add(c.Clone());
            }
            Combination = s.Combination;
            mModified = true;
        }

        public ConditionList Clone()
        {
            return new ConditionList(this);
        }

        public void Clear()
        {
            if (mList.Count > 0)
            {
                mList.Clear();
                mModified = true;
            }
            Combination = ConditionCombination.CMB_AND;
        }
        #endregion

        #region Properties
        public ConditionCombination Combination
        {
            get
            {
                return mCombination;
            }
            set
            {
                if (mCombination != value)
                {
                    IsModified = true;
                    mCombination = value;
                    notify("Combination");
                }
            }
        }

        public ObservableCollection<Condition> List
        {
            get
            {
                return mList;
            }
        }

        [System.Xml.Serialization.XmlIgnore]
        public Condition Head
        {
            get
            {
                if(mList.Count==0)
                {
                    return sDefaultCondition;
                }
                else
                {
                    return mList[0];
                }
            }
        }

        [System.Xml.Serialization.XmlIgnore]
        public bool HasCondition
        {
            get
            {
                return mList.Count > 0;
            }
        }

        [System.Xml.Serialization.XmlIgnore]
        public bool IsMulti
        {
            get
            {
                return mList.Count > 1;
            }
        }

        [System.Xml.Serialization.XmlIgnore]
        public bool IsModified
        {
            get
            {
                if (mModified)
                {
                    return true;
                }
                foreach (var c in mList)
                {
                    if (c.IsModified)
                    {
                        return true;
                    }
                }
                return false;
            }
            set
            {
                mModified = false;
                foreach (var c in mList)
                {
                    c.IsModified = false;
                }
            }
        }
        #endregion

        #region Conditional Test
        public bool Decide(StringBuilder sb = null)
        {
            if (mList.Count == 0)
            {
                return true;
            }
            foreach (var c in mList)
            {
                if (c.Decide(sb))
                {
                    if(mCombination==ConditionCombination.CMB_OR)
                    {
                        return true;
                    }
                }
                else
                {
                    if (mCombination == ConditionCombination.CMB_AND)
                    {
                        return false;
                    }
                }
            }
            return mCombination == ConditionCombination.CMB_AND;
        }
        #endregion

        #region Modification
        public void SetRatio(uint ratio)
        {
            foreach(var c in mList)
            {
                c.ScreenPoint.Ratio = ratio;
            }
        }
        #endregion
    }
}
