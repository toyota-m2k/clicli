using System.Windows;
using System.Windows.Controls;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;
using AutoClicker.model;
using System;
using AutoClicker.interop;
using System.Windows.Media;

namespace AutoClicker.view
{
    /// <summary>
    /// PositionTuner.xaml の相互作用ロジック
    /// </summary>
    public partial class PositionTuner : UserControl
    {
        //public static int MapHeight { get; set; } = 400;
        //public static int MapWidth { get; set; } = 400;

        //public static int CaptureHeight { get; set; } = 40;
        //public static int CaptureWidth { get; set; } = 40;


        //public static int UnitHeight { get { return MapHeight / CaptureHeight; } }
        //public static int UnitWidth { get { return MapWidth / CaptureWidth; } }
        private HotKey mHotKey;
        private bool mDragging = false;

        public enum Magnification
        {
            SMALL,   // 10倍
            MEDIUM,  // 15倍
            LARGE,   // 20倍
            EXTRA    // 30倍
        }


        public class PositionTunerContext : Notifier
        {
            private WeakReference<PositionTuner> mOwner;
            internal PositionTuner Owner
            {
                get
                {
                    PositionTuner v;
                    return (mOwner!=null && mOwner.TryGetTarget(out v)) ? v : null;
                }
            }

            public PositionTunerContext(PositionTuner owner, System.Drawing.Point capturePoint)
            {
                ViewMagnification = Globals.Instance.DataContext.PositionTunerMagnification;
                CapturePoint = capturePoint;
                TargetPoint = capturePoint;
                mOwner = new WeakReference<PositionTuner>(owner);
            }

            private int mCaptureWidth = 36;
            private int mCaptureHeight = 36;
            private int mZoomRatio = 10;
            public int ZoomRatio
            {
                get { return mZoomRatio; }
                set
                {
                    mZoomRatio = value;
                    notify("ZoomRatio");
                    notify("MapHeight");
                    notify("MapWidth");
                }
            }

            private Magnification mViewMagnification;
            public Magnification ViewMagnification
            {
                get { return mViewMagnification; }
                set
                {
                    if(mViewMagnification != value)
                    {
                        mViewMagnification = value;
                        Globals.Instance.DataContext.PositionTunerMagnification = value;
                        switch (value)
                        {
                            default:
                            case Magnification.SMALL:
                                mZoomRatio = 10;
                                mCaptureWidth = mCaptureHeight = 36;
                                break;
                            case Magnification.MEDIUM:
                                mZoomRatio = 15;
                                mCaptureWidth = mCaptureHeight = 24;
                                break;
                            case Magnification.LARGE:
                                mZoomRatio = 20;
                                mCaptureWidth = mCaptureHeight = 18;
                                break;
                            case Magnification.EXTRA:
                                mZoomRatio = 30;
                                mCaptureWidth = mCaptureHeight = 12;
                                break;
                        }
                        notify("ZoomRatio");
                        notify("MapHeight");
                        notify("MapWidth");
                        notify("CaptureWidth");
                        notify("CaptureHeight");
                        notify("ViewMagnification");
                        var p = Owner;
                        if (null != p)
                        {
                            p.CaptureNow();
                        }
                    }
                }
            }

            public int CaptureWidth
            {
                get { return mCaptureWidth; }
                set
                {
                    mCaptureWidth = value;
                    notify("CaptureWidth");
                    notify("MapWidth");
                }
            }
            public int CaptureHeight
            {
                get { return mCaptureHeight; }
                set
                {
                    mCaptureHeight = value;
                    notify("CaptureHeight");
                    notify("MapHeight");
                }
            }

            public int MapWidth
            {
                get { return mCaptureWidth * mZoomRatio; }
            }
            public int MapHeight
            {
                get { return mCaptureHeight * mZoomRatio; }
            }

            public System.Drawing.Point Org { get; set; } = new System.Drawing.Point();

            public System.Drawing.Point CapturePoint
            {
                get { return new System.Drawing.Point(Org.X + CaptureWidth / 2, Org.Y + CaptureHeight / 2); }
                set
                {
                    var org = new System.Drawing.Point(value.X - CaptureWidth / 2, value.Y - CaptureHeight / 2);
                    if (org.X < 0)
                    {
                        org.X = 0;
                    }
                    else if (org.X > System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - CaptureWidth / 2)
                    {
                        org.X = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - CaptureWidth / 2;
                    }

                    if (org.Y < 0)
                    {
                        org.Y = 0;
                    }
                    else if (org.Y > System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - CaptureHeight / 2)
                    {
                        org.Y = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - CaptureHeight / 2;
                    }
                    if (Org != org)
                    {
                        Org = org;
                        notify("Org");
                        notify("TargetOffset");
                        notify("CrosshairOffset");
                    }
                }
            }

            private System.Drawing.Point mTargetPoint = new System.Drawing.Point();
            public System.Drawing.Point TargetPoint
            {
                get
                {
                    return mTargetPoint;
                }
                set
                {
                    if (mTargetPoint != value)
                    {
                        mTargetPoint = value;
                        notify("TargetPoint");
                        notify("TargetOffset");
                        notify("CrosshairOffset");
                    }
                }
            }


            public System.Drawing.Point TargetOffset
            {
                get
                {
                    return new System.Drawing.Point(mTargetPoint.X - Org.X, mTargetPoint.Y - Org.Y);
                }

                set
                {
                    TargetPoint = new System.Drawing.Point(value.X + Org.X, value.Y + Org.Y);
                }
            }

            public System.Drawing.Point CrosshairOffset
            {
                get
                {
                    return new System.Drawing.Point(TargetOffset.X*ZoomRatio - ZoomRatio / 2, TargetOffset.Y* ZoomRatio - ZoomRatio / 2);
                }
            }
        }

        private PositionTunerContext mContext;

        public PositionTuner()
        {
            InitializeComponent();
            RenderOptions.SetBitmapScalingMode(CapturedImage, BitmapScalingMode.NearestNeighbor);
            RenderOptions.SetEdgeMode(CapturedImage, EdgeMode.Aliased);
        }

        public void Init(System.Drawing.Point point)
        {
            mContext = new PositionTunerContext(this, point);
            DataContext = mContext;
            capture();
            openHotKey();
        }

        private void capture()
        {
            var org = mContext.Org;
            using (Bitmap myBitmap = new Bitmap(mContext.CaptureWidth, mContext.CaptureHeight))
            {
                using (Graphics g = Graphics.FromImage(myBitmap))
                {
                    g.CopyFromScreen(org, new System.Drawing.Point(0, 0), new System.Drawing.Size(mContext.CaptureWidth, mContext.CaptureHeight));
                }
                using (Stream stream = new MemoryStream())
                {
                    myBitmap.Save(stream, ImageFormat.Png);
                    stream.Seek(0, SeekOrigin.Begin);
                    CapturedImage.Source = BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                }
            }
        }

        private void OnMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var pos = e.GetPosition(CapturedImage);
            mContext.TargetOffset = new System.Drawing.Point((int)Math.Round(pos.X/mContext.ZoomRatio), (int)Math.Round(pos.Y/mContext.ZoomRatio));
            mDragging = true;
        }
        private void OnMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (mDragging)
            {
                var pos = e.GetPosition(CapturedImage);
                mContext.TargetOffset = new System.Drawing.Point((int)Math.Round(pos.X / mContext.ZoomRatio), (int)Math.Round(pos.Y / mContext.ZoomRatio));
            }
        }

        private void OnMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (mDragging)
            {
                mDragging = false;
                var pos = e.GetPosition(CapturedImage);
                mContext.TargetOffset = new System.Drawing.Point((int)Math.Round(pos.X / mContext.ZoomRatio), (int)Math.Round(pos.Y / mContext.ZoomRatio));
                CaptureNow();
            }
        }


        public System.Drawing.Point Result
        {
            get
            {
                return mContext.TargetPoint;
            }
        }

        internal void CaptureNow()
        {
            mContext.CapturePoint = mContext.TargetPoint;
            capture();
        }

        public delegate void PositionCompleted(bool flag, PositionTuner pt);
        public event PositionCompleted OnCompleted;

        private void Button_Ok(object sender, RoutedEventArgs e)
        {
            Complete(true);
        }

        private void Button_Cancel(object sender, RoutedEventArgs e)
        {
            Complete(false);
        }

        private void Button_Capture(object sender, RoutedEventArgs e)
        {
            CaptureNow();
        }

        private void Complete(bool flag)
        {
            closeHotKey();
            OnCompleted?.Invoke(flag, this);
        }

        private void openHotKey()
        {
            if (null == mHotKey)
            {
                HotKey.Proc succeeded = () =>
                {
                    var pos = System.Windows.Forms.Cursor.Position;
                    mContext.CapturePoint = new System.Drawing.Point(pos.X, pos.Y);
                    mContext.TargetPoint = mContext.CapturePoint;
                    capture();
                };
                HotKey.Proc cancelled = () =>
                {
                };

                mHotKey = new HotKey();
                bool result;
                result = mHotKey.Register(0, (int)System.Windows.Forms.Keys.A, succeeded);
                result |= mHotKey.Register(0, (int)System.Windows.Forms.Keys.Add, succeeded);
                result |= mHotKey.Register(0, (int)System.Windows.Forms.Keys.Escape, cancelled);
            }

        }
        private void closeHotKey()
        {
            if (null != mHotKey)
            {
                mHotKey.Dispose();
                mHotKey = null;
            }
        }

    }
}
