using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace AutoClicker.model
{
    public class ConditionList : Notifier
    {
        #region Type Definitions
        /**
         * Condition Class
         */
        public class Condition : Notifier
        {
            #region Private Fields

            private bool mValid = false;
            private bool mNegation;
            private ScreenPoint mScreenPoint;
            private HSVColorRange mColorRange;
            private bool mModified = false;
            
            #endregion

            #region Construction & Duplicating

            public Condition()
            {
                mValid = false;
                mNegation = false;
                mColorRange = new HSVColorRange();
                mScreenPoint = new ScreenPoint();
                mModified = false;
            }

            public Condition(Condition s)
            {
                mValid = s.mValid;
                mNegation = s.Negation;
                mColorRange = s.ColorRange.Clone();
                mScreenPoint = s.ScreenPoint.Clone();
                mModified = false;
            }

            public void Clear()
            {
                mValid = false;
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
                mValid = s.mValid;
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
                return c.mValid == mValid && c.ScreenPoint == ScreenPoint && c.ColorRange == ColorRange && c.Negation == Negation;
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

            public bool IsValid
            {
                get { return mValid; }
                set {
                    mValid = value;
                    notify("IsValid");
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
            public bool Decide(IDebugOutput dbgout, bool ignoreNegation)
            {
                return TestAt(ScreenPoint.AbsolutePoint, dbgout, ignoreNegation);
            }

            public bool TestAt(Point absPoint, IDebugOutput dbgout, bool ignoreNegation)
            {
                try
                {
                    using (Bitmap myBitmap = new Bitmap(1, 1))
                    {
                        using (Graphics g = Graphics.FromImage(myBitmap))
                        {
                            g.CopyFromScreen(absPoint, new Point(0, 0), new System.Drawing.Size(1, 1));
                            var color = myBitmap.GetPixel(0, 0);
                            var result = ColorRange.IsInRange(color, dbgout);
                            return ignoreNegation ? result : (mNegation ? !result : result);
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
                    Debug.WriteLine(e2.ToString());
                    Debug.WriteLine(absPoint.ToString());
                    return false;
                }
            }
            #endregion

        }
        public enum ConditionCombination
        {
            AND,
            OR,
        }

        public enum ActionType
        {
            NONE = 0,
            SKIP = 1,
            WAIT = 2,
        }


        #endregion

        #region Private Fields

        private ObservableCollection<Condition> mList;
        private ConditionCombination mCombination;
        private ActionType mType;
        private bool mModified;
        private static readonly Condition sDefaultCondition = new Condition();

        #endregion

        #region Construction & Duplication

        public ConditionList()
        {
            mList = new ObservableCollection<Condition>();
            mCombination = ConditionCombination.AND;
            mType = ActionType.NONE;
            mModified = false;
        }
        public ConditionList(ConditionList s)
        {
            mList = new ObservableCollection<Condition>();
            foreach (var c in s.List)
            {
                mList.Add(c.Clone());
            }
            mCombination = s.Combination;
            mType = s.mType;
            mModified = false;
        }

        public void CopyFrom(ConditionList s)
        {
            if(null==s)
            {
                Clear();
                return;
            }
            mList = s.mList;
            mType = s.mType;
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
            Combination = ConditionCombination.AND;
            mType = ActionType.SKIP;
        }

        public void PrepareForEditing()
        {
            int count = mList.Count;
            if (count == 0 || mList[count-1].IsValid)
            {
                mList.Add(new Condition());
            }
            if (Type == ActionType.NONE)
            {
                Type = ActionType.SKIP;
            }
        }

        public void TrimAfterEditing()
        {
            if (mType == ActionType.NONE)
            {
                mList.Clear();
            }
            else
            {
                for (int i = mList.Count - 1; i >= 0; i--)
                {
                    if (!mList[i].IsValid)
                    {
                        mList.RemoveAt(i);
                    }
                }
                if(0==mList.Count)
                {
                    Type = ActionType.NONE;
                }
            }
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
                    notify("CombinationName");
                }
            }
        }

        [System.Xml.Serialization.XmlIgnore]
        public string CombinationName
        {
            get
            {
                return mCombination.ToString();
            }
        }


        public ActionType Type
        {
            get
            {
                return mType;
            }
            set
            {
                if(mType!=value)
                {
                    mType = value;
                    notify("Type");
                    IsModified = true;
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

        #region Comparison

        public bool Equals(ConditionList c)
        {
            if (null == c)
            {
                return false;
            }
            if( mCombination != c.Combination)
            {
                return false;
            }
            if(mList.Equals(c.List))
            {
                return true;
            }
            if(mList.Count!=c.List.Count)
            {
                return false;
            }
            for(int i=0, ci=mList.Count;i<ci;i++)
            {
                if(mList[i]!=c.List[i])
                {
                    return false;
                }
            }
            return true;
        }

        #endregion

        #region Conditional Test
        public bool Decide(IDebugOutput dbgout, bool ignoreNegation)
        {
            if (mList.Count == 0)
            {
                return true;
            }
            foreach (var c in mList)
            {
                if (c.Decide(dbgout, ignoreNegation))
                {
                    if(mCombination==ConditionCombination.OR)
                    {
                        return true;
                    }
                }
                else
                {
                    if (mCombination == ConditionCombination.AND)
                    {
                        return false;
                    }
                }
            }
            return mCombination == ConditionCombination.AND;
        }
        #endregion

        #region Coordination
        public void SetRatio(uint ratio)
        {
            foreach(var c in mList)
            {
                c.ScreenPoint.Ratio = ratio;
            }
        }

        public void SetBasePointKeepAbs(Point bp)
        {
            foreach(var c in mList)
            {
                Point abs = c.ScreenPoint.AbsolutePoint;
                c.ScreenPoint.BasePoint = bp;
                c.ScreenPoint.AbsolutePoint = abs;
            }

        }

        public void TranslateBasePoint(Point bp)
        {
            foreach (var c in mList)
            {
                c.ScreenPoint.BasePoint = bp;
            }
        }

        public void SetBasePointAndRatioKeepAbs(Point bp, uint ratio)
        {
            foreach (var c in mList)
            {
                Point abs = c.ScreenPoint.AbsolutePoint;
                c.ScreenPoint.BasePoint = bp;
                c.ScreenPoint.Ratio = ratio;
                c.ScreenPoint.AbsolutePoint = abs;
            }
        }

        #endregion

        #region List Operation
        public void Add(Condition condition)
        {
            condition.IsValid = true;
            int count = mList.Count;
            if (count > 0 && !mList[count - 1].IsValid)
            {
                mList.Insert(count - 1, condition);
            }
            else
            {
                mList.Add(condition);
            }
            notify("IsMulti");
            notify("HasCondition");
            IsModified = true;
        }

        public void Update(Condition original, Condition updated)
        {
            if(original==updated)
            {
                return;
            }

            int index = mList.IndexOf(original);
            if(index>=0)
            {
                mList[index] = updated;
                IsModified = true;
            }
            else
            {
                Add(updated);
            }
        }

        public void Remove(Condition condition)
        {
            if(!condition.IsValid)
            {
                return;
            }
            mList.Remove(condition);
            IsModified = true;
            notify("IsMulti");
            notify("HasCondition");
        }

        public void Remove(List<Condition> conditions)
        {
            bool modified = false;
            foreach(var c in conditions)
            {
                if (!c.IsValid)
                {
                    continue;
                }
                mList.Remove(c);
                modified = true;
            }

            if (modified)
            {
                IsModified = true;
                notify("IsMulti");
                notify("HasCondition");
            }
        }

        #endregion
    }
}
