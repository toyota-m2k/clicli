using CliCliBoy.interop;
using CliCliBoy.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;
using Point = System.Drawing.Point;
using Rectangle = System.Drawing.Rectangle;


namespace CliCliBoy.view
{
    /// <summary>
    /// UserControl1.xaml の相互作用ロジック
    /// </summary>
    public partial class GetPositionPanel : UserControl
    {
        public delegate void OnCloseHandler(bool ok);

        public Point Result {get;private set;}

        const int GPTimerSec = 3;       // sec
        const int GPTImeInterval = 100; // msec

        public class GetPosData : Notifier
        {
            private int mRemainSec;

            public GetPosData()
            {
                mRemainSec = GPTimerSec;
            }
            public int RemainSec { 
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
            }

            /**
             * Ratioの設定欄を表示するかどうか。
             * 本来、↑の用途だが、これを表示するかどうかは、
             * BasePointを設定するダイアログか、それ以外（ターゲット位置や条件チェック位置の取得用ダイアログ）かの違いなので、
             * BasePoint設定用ダイアログかどうかのチェックに使ってしまっていてわかりにくい。
             * そこで、この用途には、isGettingBasePosDialog プロパティを用意した。
             */
            private bool mEnableRatio = false;
            public bool EnableRatio
            {
                get { return mEnableRatio; }
                set
                {
                    mEnableRatio = value;
                    notify("EnableRatio");
                }
            }

            public bool isGettingBasePosDialog
            {
                get { return mEnableRatio; }
            }

            private uint mRatio = 100;
            public uint Ratio
            {
                get
                {
                    return (!EnableRatio) ? 100 : mRatio;
                }
                set
                {
                    mRatio = value;
                    notify("Ratio");
                }
            }

            private Point mOrgPoint = new Point(0,0);
            public Point OrgPoint
            {
                set
                {
                    mOrgPoint = value;
                    notify("OrgPoint");
                }
                get
                {
                    return mOrgPoint;
                }
            }

            private TargetWinPos _targetWinPos = null;
            public TargetWinPos TargetWinPos
            {
                get { return _targetWinPos; }
                set
                {
                    if (_targetWinPos != value)
                    {
                        _targetWinPos = value;
                        notify("TargetWinPos");
                        notify("TargetWinPosEnabled");
                        notify("HasTargetWinPos");
                        notify("SettingTargetWinPos");
                    }
                }
            }

            public bool TargetWinPosEnabled { get { return null != TargetWinPos; } }

            public bool HasTargetWinPos { get { return null != TargetWinPos && TargetWinPos.hasValue; } }

            private bool mSettingTargetWinPos = false;
            public bool SettingTargetWinPos
            {
                get { return mSettingTargetWinPos; }
                set { mSettingTargetWinPos = value; notify("SettingTargetWinPos"); }
            }

        }

        private HotKey mHotKey;
        private DispatcherTimer mTimer;
        private Point mPrevPoint;
        private int mMoveCheckCount;
        private GetPosData mGPData;

        private OnCloseHandler mOnClose;
        private PosGotCallback mOnGotPos;
        private RatioGotCallback mOnGotRatio;

        public GetPositionPanel()
        {
            InitializeComponent();
            mHotKey = null;
            mGPData = new GetPosData();
            this.DataContext = mGPData;
            PositionTuner.OnCompleted += OnPositionTuned;
        }

        public bool EnableRatio
        {
            get { return mGPData.EnableRatio; }
            set { mGPData.EnableRatio = value; }
        }
        public uint Ratio
        {
            get { return mGPData.Ratio; }
            set { mGPData.Ratio = value; }
        }

        public TargetWinPos TargetWinPos
        {
            get { return mGPData.TargetWinPos; }
        }

        private void Close(bool result) {
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

            if(result &&  null != mOnGotRatio)
            {
                mOnGotRatio(Ratio);
            }

            if (null != mOnClose)
            {
                mOnClose(result);
            }
            if (result && null != mOnGotPos)
            {
                mOnGotPos(Result);
            }
        }

        private void setResultOrgPoint(Point pos)
        {
            if (mGPData.SettingTargetWinPos)
            {
                WinPlacement.SetWindowPositionAtPoint(pos, mGPData.TargetWinPos.Position);
            }
            else
            {
                if(mGPData.TargetWinPosEnabled)
                {
                    Rectangle? rc = WinPlacement.GetWindowPositionAtPoint(pos);
                    if(rc!=null)
                    {
                        mGPData.TargetWinPos = new TargetWinPos(rc.Value);
                    }
                }
                mGPData.OrgPoint = pos;
            }
        }

        private void prepareHotKey()
        {
            if(null == mHotKey)
            {
                HotKey.Proc succeeded = () =>
                {
                    Result = System.Windows.Forms.Cursor.Position;
                    if (mGPData.isGettingBasePosDialog)
                    {
                        endTimer();
                        setResultOrgPoint(Result);
                    }
                    else
                    {
                        Close(true);
                    }
                };
                HotKey.Proc cancelled = () =>
                {
                    Close(false);
                };

                mHotKey = new HotKey();
                bool result;
                result = mHotKey.Register(0, (int)System.Windows.Forms.Keys.A, succeeded);
                result |= mHotKey.Register(0, (int)System.Windows.Forms.Keys.Add, succeeded);
                result |= mHotKey.Register(0, (int)System.Windows.Forms.Keys.Escape, cancelled);

                if (!result)
                {
                    Close(false);
                }
            }
        }

        public void Open(OnCloseHandler onClose, PosGotCallback gotpos, RatioGotCallback gotratio, uint initRatio, int x, int y, ITargetWinPosProp twp)
        {
            mOnClose = onClose;
            mOnGotPos = gotpos;
            mOnGotRatio = gotratio;
            mGPData.OrgPoint = new Point(x, y);

            Result = Point.Empty;

            mGPData.Reset();
            mGPData.EnableRatio = (null != gotratio);
            mGPData.Ratio = initRatio;
            GPTChk.IsChecked = false;

            if (null != twp)
            {
                mGPData.TargetWinPos = twp.TargetWinPos.Clone();
            }

            prepareHotKey();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close(false);
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Close(true);
        }

        public void startTimer()
        {
            if (null==mTimer)
            {
                mTimer = new DispatcherTimer(DispatcherPriority.Normal);
                mTimer.Tick += new EventHandler(onTimeout);
                mTimer.Interval = new TimeSpan(0, 0, 0, 0, GPTImeInterval);
            }
            if (mTimer.IsEnabled)
            {
                return;
            }

            mGPData.Reset();
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
                if (mMoveCheckCount >= GPTimerSec*1000/GPTImeInterval)
                {
                    // 100 ms x 30times = 3 sec =====  3 * 1000 msec / 100msec = 30 times
                    Result = curPoint;
                    if (mGPData.isGettingBasePosDialog)
                    {
                        endTimer();
                        setResultOrgPoint(Result);
                        GPTChk.IsChecked = false;
                    }
                    else
                    {
                        Close(true);
                    }
                    return;
                }
            }
            else
            {
                mMoveCheckCount = 0;
            }
            mGPData.RemainSec = GPTimerSec - (int)((mMoveCheckCount * GPTImeInterval) / 1000);
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
            mGPData.Reset();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            startTimer();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            endTimer();
        }

        private void PresetRatio_Click(object sender, RoutedEventArgs e)
        {
            Ratio = uint.Parse( ((Button)sender).Content.ToString());
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

            PositionTuner.Init(mGPData.OrgPoint);
            GetPositionMain.Visibility = Visibility.Hidden;
            GetPositionSub.Visibility = Visibility.Visible;
        }

        private void OnPositionTuned(bool flag, PositionTuner pt)
        {
            if(flag)
            {
                Result = pt.Result;
                setResultOrgPoint(Result);
            }
            GetPositionMain.Visibility = Visibility.Visible;
            GetPositionSub.Visibility = Visibility.Hidden;

            if (!mGPData.isGettingBasePosDialog)
            {
                Close(flag);
            }
            else
            {
                prepareHotKey();
            }
        }

        private void Btn_ShowPos(object sender, RoutedEventArgs e)
        {
            MouseCursorWindow.Show(mGPData.OrgPoint);
        }

        private void Btn_ClearTargetWinPos(object sender, RoutedEventArgs e)
        {
            mGPData.TargetWinPos = new TargetWinPos(new Rectangle(-8,-8,0,0));
        }
    }
}
