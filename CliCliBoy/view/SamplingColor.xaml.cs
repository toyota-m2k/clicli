using CliCliBoy.interop;
using CliCliBoy.model;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Point = System.Drawing.Point;

namespace CliCliBoy.view
{
    /// <summary>
    /// GetColor.xaml の相互作用ロジック
    /// </summary>
    public partial class SamplingColor : UserControl
    {
        const int GPTimerSec = 3;       // sec
        const int GPTImeInterval = 100; // msec

        public class SampleColors : Notifier
        {
            public SampleColors()
            {
                //mColorRange = new HSVColorRange();
                mCondition = new ConditionList.Condition();
                mRemainSec = GPTimerSec;
            }

            private ConditionList.Condition mCondition;

            public ConditionList.Condition Condition
            {
                get { return mCondition; }
                set {
                    mCondition = value;
                    SamplingPoint = value.ScreenPoint.AbsolutePoint;
                    notify("Condition");
                }
            }

            //private HSVColorRange mColorRange;
            //public HSVColorRange ColorRange
            //{
            //    get { return mColorRange; }
             
            //    set
            //    {
            //        if (value != null)
            //        {
            //            mColorRange = value;
            //        }
            //        else
            //        {
            //            mColorRange.Clear();
            //        }
            //        notify("ColorRange");

            //    }
            //}

            public System.Windows.Media.Brush this[int i]
            {
                get
                {
                    return new SolidColorBrush(mCondition.ColorRange[i]);
                }
            }

            private int mRemainSec;

            public int RemainSec
            {
                get { return mRemainSec; }
                set
                {
                    if (mRemainSec != value)
                    {
                        mRemainSec = value;
                        notify("RemainSec");
                    }
                }
            }

            public void Reset()
            {
                RemainSec = GPTimerSec;
                //Condition.Clear();
                //NotifyColorChanged();
                //SamplingPoint = Point.Empty;
            }

            public void NotifyColorChanged()
            {
                notify(Binding.IndexerName);
            }

            private Point mSamplingPoint;

            public Point SamplingPoint
            {
                get { return mSamplingPoint; }
                set
                {
                    if (mSamplingPoint != value)
                    {
                        mSamplingPoint = value;
                        notify("SamplingPoint");
                    }
                }
            }

            private int mSamplingInflation = 2;
            public int SamplingInflation
            {
                get { return mSamplingInflation; }
                set
                {
                    if(mSamplingInflation != value)
                    {
                        mSamplingInflation = value;
                        notify("SamplingInflation");
                    }
                }
            }

            private bool mHoldOnSampling = false;
            public bool HoldOnSampling
            {
                get { return mHoldOnSampling; }
                set
                {
                    mHoldOnSampling = value;
                    notify("HoldOnSampling");
                }
            }
        }

        private class SamplingColorDialog : DialogHelper
        {
            internal SamplingColorDialog(SamplingColor dlg)
                : base(dlg)
            {
            }

            public override void Close(bool result)
            {
                ((SamplingColor)Control).Term();
                base.Close(result);
            }
        }



        private HotKey mHotKey;
        private DispatcherTimer mTimer;
        private Point mPrevPoint;
        private int mMoveCheckCount;
        private SampleColors mSamplingContext;
        private DialogHelper mDialogHelper;
        public IDialog Dialog { get { return mDialogHelper; } }
    
        public ConditionList.Condition Result
        {
            get { return mSamplingContext.Condition; }
        }

        public SamplingColor()
        {
            InitializeComponent();
            mDialogHelper = new SamplingColorDialog(this);
            mSamplingContext = new SampleColors();
            mHotKey = null;
            DataContext = mSamplingContext;
            PositionTuner.OnCompleted += OnTuningPointCompleted;
        }


        public void Term()
        {
            if (null != mHotKey)
            {
                mHotKey.Dispose();
                mHotKey = null;
            }
            if (null != mTimer)
            {
                endTimer();
                mTimer = null;
            }
            endSampling();
        }

        private void prepareHotKey()
        {
            if (null == mHotKey)
            {
                HotKey.Proc succeeded = () =>
                {
                    endTimer();
                    mSamplingContext.SamplingPoint = System.Windows.Forms.Cursor.Position;
                    getColors();
                };
                HotKey.Proc cancelled = () =>
                {
                    Dialog.Close(false);
                };

                mHotKey = new HotKey();
                bool result;
                result = mHotKey.Register(0, (int)System.Windows.Forms.Keys.A, succeeded);
                result |= mHotKey.Register(0, (int)System.Windows.Forms.Keys.Add, succeeded);
                result |= mHotKey.Register(0, (int)System.Windows.Forms.Keys.Escape, cancelled);

                if (!result)
                {
                    Dialog.Close(false);
                }
            }
        }

        public void Init(ConditionList.Condition condition, Point initialPoint )
        {
            mSamplingContext.Reset();
            mSamplingContext.Condition = condition;
            // mSamplingContext.SamplingPoint = condition.ScreenPoint.AbsolutePoint;
            mSamplingContext.NotifyColorChanged();
            if (!mSamplingContext.Condition.IsValid)
            {
                mSamplingContext.Condition.IsValid = true;
                if(mSamplingContext.SamplingPoint.X == 0 && mSamplingContext.SamplingPoint.Y==0)
                {
                    mSamplingContext.SamplingPoint = initialPoint;
                    getColors();
                }
            }
            //mSamplingContext.SamplingPoint = initPoint;
            //mSamplingContext.ColorRange = initColorRange;


            GPTChk.IsChecked = false;
            prepareHotKey();
        }


        void getColors(bool append=false)
        {
            int d = mSamplingContext.SamplingInflation;
            int sz = d * 2 + 1;
            System.Drawing.Rectangle screen = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
            Point pos = new Point(mSamplingContext.SamplingPoint.X-d, mSamplingContext.SamplingPoint.Y - d);

            if (pos.X < 0)
            {
                pos.X = 0;
            }
            else if (pos.X + sz > screen.Right)
            {
                pos.X = screen.Right - sz;
            }
            if (pos.Y < 0)
            {
                pos.Y = 0;
            }
            else if (pos.Y + sz > screen.Bottom)
            {
                pos.Y = screen.Bottom - sz;
            }

            if (!append)
            {
                mSamplingContext.Condition.ColorRange.Clear();
            }
            using (Bitmap myBitmap = new Bitmap(sz, sz))
            {
                using (Graphics g = Graphics.FromImage(myBitmap))
                {
                    g.CopyFromScreen(pos, new Point(0, 0), new System.Drawing.Size(sz, sz));
                }
                for (int i = 0, ci = sz; i < ci; i++)
                {
                    for (int j = 0, cj = sz; j < cj; j++)
                    {
                        mSamplingContext.Condition.ColorRange.AddColor(myBitmap.GetPixel(i, j));
                    }
                }
            }
            mSamplingContext.NotifyColorChanged();
        }

        DispatcherTimer mSamplingTimer = null;

        private void startSampling()
        {
            if(null!=mSamplingTimer)
            {
                return;
            }
            getColors(KeyState.IsKeyDown(KeyState.VK_CONTROL));
            mSamplingTimer = new DispatcherTimer(DispatcherPriority.Normal);
            mSamplingTimer.Tick += (obj, args) =>
            {
                getColors(true);
            };
            mSamplingTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            mSamplingTimer.Start();
        }
        private void endSampling()
        {
            if(null==mSamplingTimer)
            {
                return;
            }
            mSamplingTimer.Stop();
            mSamplingTimer = null;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Dialog.Close(false);
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Result.ScreenPoint.AbsolutePoint = mSamplingContext.SamplingPoint;
            Dialog.Close(true);
        }

        public void startTimer()
        {
            if (null == mTimer)
            {
                mTimer = new DispatcherTimer(DispatcherPriority.Normal);
                mTimer.Tick += new EventHandler(onTimeout);
                mTimer.Interval = new TimeSpan(0, 0, 0, 0, GPTImeInterval);
            }
            if (mTimer.IsEnabled)
            {
                return;
            }

            mSamplingContext.Reset();
            mMoveCheckCount = 0;
            mPrevPoint = Point.Empty;
            mTimer.Start();
        }

        private void onTimeout(object sender, EventArgs e)
        {
            if (null == mTimer)
            {
                return;
            }
            mTimer.Stop();

            Point curPoint = System.Windows.Forms.Cursor.Position;
            if (mPrevPoint == curPoint)
            {
                mMoveCheckCount++;
                if (mMoveCheckCount >= GPTimerSec * 1000 / GPTImeInterval)
                {
                    // 100 ms x 30times = 3 sec =====  3 * 1000 msec / 100msec = 30 times
                    mSamplingContext.SamplingPoint = curPoint;
                    getColors();
                    GPTChk.IsChecked = false;
                    endTimer();
                    return;
                }
            }
            else
            {
                mMoveCheckCount = 0;
            }
            mSamplingContext.RemainSec = GPTimerSec - (int)((mMoveCheckCount * GPTImeInterval) / 1000);
            mPrevPoint = curPoint;
            mTimer.Start();
        }

        public void endTimer()
        {
            if (null == mTimer)
            {
                return;
            }
            if (mTimer.IsEnabled)
            {
                mTimer.Stop();
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            startTimer();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            endTimer();
        }

        private void Btn_SampleNow(object sender, RoutedEventArgs e)
        {
            //Debug.WriteLine("Button-Click");
            if (!mSamplingContext.HoldOnSampling)
            {
                endSampling();
                getColors(KeyState.IsKeyDown(KeyState.VK_CONTROL));
            }
        }

        private void Btn_MoveTo(object sender, RoutedEventArgs e)
        {
            //MouseEmulator.MoveTo(mSamplingContext.SamplingPoint);
            MouseCursorWindow.Show(mSamplingContext.SamplingPoint, mSamplingContext.Condition.TestAt(mSamplingContext.SamplingPoint, null, true));

        }

        private void Btn_Adjust(object sender, RoutedEventArgs e)
        {
            endTimer();
            GPTChk.IsChecked = false;

            if (null != mHotKey)
            {
                mHotKey.Dispose();
                mHotKey = null;
            }

            PositionTuner.Init(mSamplingContext.SamplingPoint);
            SamplingColorMain.Visibility = Visibility.Hidden;
            SamplingColorSub.Visibility = Visibility.Visible;
        }

        private void OnTuningPointCompleted(bool flag, PositionTuner pt)
        {
            if (flag && mSamplingContext.SamplingPoint != pt.Result)
            {
                mSamplingContext.SamplingPoint = pt.Result;
                getColors();
            }
            SamplingColorMain.Visibility = Visibility.Visible;
            SamplingColorSub.Visibility = Visibility.Hidden;

            prepareHotKey();
        }


        private void Btn_SamplingDown(object sender, MouseButtonEventArgs e)
        {
            //Debug.WriteLine("Button-Down");
            if (mSamplingContext.HoldOnSampling)
            {
                startSampling();
            }
        }

        private void Btn_SamplingUp(object sender, MouseButtonEventArgs e)
        {
            //Debug.WriteLine("Button-Up");
            endSampling();
        }

    }
}
