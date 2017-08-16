using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliCliBoy.model
{
    /**
     * プロジェクトクラス
     */
    public class Project : Notifier
    {
        // fields...
        private PointingMode mMode;     // pointing mode + base point
        private int mRepeat;            // repeat count
        private string mName;           // project name
        private ObservableCollection<TargetItem> mTargets;
        private static int sRepeatCountDef = 1;
        private static readonly string[] repeatpropnames = {"Repeat", "IsInfinite"};
        private uint mRatio;

        /**
         * コンストラクタ
         */
        public Project()
        {
            mMode = new PointingMode();
            mTargets = new ObservableCollection<TargetItem>();
            mRepeat = 0; // infinite
            mRatio = 0;
            ID = 0;
            IsModified = false;
            mTargets.CollectionChanged += OnCollectionChanged;
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            IsModified = true;
        }

        /**
         * 初回起動時などに自動生成されるデフォルトプロジェクトのテンプレート
         */
        public static Project DefaultProject
        {
            get
            {
                var p = new Project();
                p.Name = "default";
                return p;
            }
        }

        #region Properties

        private bool mIsModified = false;

        [System.Xml.Serialization.XmlIgnore]
        public bool IsModified {
            get
            {
                if (mIsModified) return true;
                foreach (var item in mTargets)
                {
                    if (item.IsModified)
                    {
                        return true;
                    }
                }
                return false;
            }
            set
            {
                mIsModified = value;
                if (false == value)
                {
                    foreach (var item in mTargets)
                    {
                        item.IsModified = false;
                    }
                }
            }
        }

        public uint Ratio
        {
            get { return (mRatio == 0) ? 100 : mRatio; }
            set
            {
                uint v = (value == 100) ? 0 : value;
                if (mRatio != v)
                {
                    mRatio = v;
                    notify("Ratio");
                    notify("RatioEnabled");
                    foreach (var t in mTargets)
                    {
                        t.SetRatio(Ratio);
                    }
                    IsModified = true;
                }
            }
        }

        [System.Xml.Serialization.XmlIgnore]
        public bool RatioEnabled
        {
            get { return Ratio != 100; }
        }
    


        /**
         * ポインティングモード
         */
        public PointingMode Mode
        {
            get { return mMode; }
            set {
                if (mMode.IsSame(value)){
                    return;
                }

                Point baseNew = new Point();
                if (value.IsRelative)
                {
                    baseNew = value.BasePoint;
                }
                foreach (var item in mTargets)
                {
                    if (item.Type != ClickType.NOOP)
                    {
                        //Point abs = item.ScreenPoint.AbsolutePoint;
                        //item.ScreenPoint.BasePoint = baseNew;
                        //item.ScreenPoint.AbsolutePoint = abs;
                        item.SetBasePointKeepAbs(baseNew);
                    }
                }
                mMode.CopyFrom(value);
                IsModified = true;
                notify("Mode");
            }
        }

        /**
         * リストビューに表示するためのデータソースコレクション
         */
        public ObservableCollection<TargetItem> Targets
        {
            get { return mTargets; }
        }

        /**
         * 繰り返し回数(0ならInfinite）
         */
        public int Repeat
        {
            get { return mRepeat; }
            set 
            {
                if (mRepeat != value)
                {
                    mRepeat = value;
                    if (value > 0)
                    {
                        sRepeatCountDef = value;
                    }
                    IsModified = true;
                    notify(repeatpropnames);
                }
            }
        }

        private int mId;
        public int ID 
        { 
            get{return mId;}
            set { if (mId != value) { mId = value; IsModified = true; } }
        }

        /**
         * Infiniteか有限回数リピートか？
         */
        [System.Xml.Serialization.XmlIgnore]
        public bool IsInfinite
        {
            get { return mRepeat <= 0; }
            set{
                Repeat = (value) ? 0 : sRepeatCountDef;
            }
        }

        /**
         * プロジェクトの名前
         */
        public string Name
        {
            get { return mName; }
            set
            {
                mName = value;
                IsModified = true;
                notify("Name");
            }
        }

        #endregion

        #region Item Cordinate

        /**
         * 相対座標モードの場合に、基準位置を再設定する。
         * （相対位置は変化せず、絶対位置が平行移動する。）
         */
        public void TranslateBasePoint(Point newPoint)
        {
            if (!Mode.IsRelative)
            {
                return;
            }

            //Size vt = new Size(newPoint.X- Mode.BasePoint.X, newPoint.Y-Mode.BasePoint.Y);
            for (int i = 0, ci = mTargets.Count; i < ci; i++)
            {
                TargetItem item = mTargets[i];
                if (item.Type != ClickType.NOOP)
                {
                    //item.ScreenPoint.BasePoint = newPoint;
                    item.TranslateBasePoint(newPoint);
                    mTargets[i] = item;
                }
            }
            mMode.BasePoint = newPoint;
            IsModified = true;
        }

        /**
         * アイテムの座標（絶対座標）をプロジェクト座標（Modeによって規定される座標系）に変換する
         */
        public TargetItem AdjustPoint(TargetItem item)
        {

            Point basePoint = new Point();
            uint ratio = 100;
            if (mMode.IsRelative)
            {
                basePoint = mMode.BasePoint;
                ratio = Ratio;
            }
            item.SetBasePointAndRatioKeepAbs(basePoint, ratio);
            //Point abs = item.ScreenPoint.AbsolutePoint;
            //item.ScreenPoint.BasePoint = basePoint;
            //item.ScreenPoint.Ratio = ratio;
            //item.ScreenPoint.AbsolutePoint = abs;
            return item;
        }

        /**
         * アイテムの座標（必ず絶対座標）を適切に変換してターゲットリストに追加する。
         */ 
        public void AddTarget(TargetItem item)
        {
            mTargets.Add(AdjustPoint(item));
            IsModified = true;
        }

        /**
         * アイテムの座標（必ず絶対座標）を適切に変換してターゲットリストに挿入する。
         */
        public void InsertTarget(int index, TargetItem item)
        {
            mTargets.Insert(index, AdjustPoint(item));
            IsModified = true;
        }

        //public void ReplaceTarget(int index, TargetItem item)
        //{
        //    mTargets[index] = AdjustPoint(item);
        //}
        #endregion
    }
}
