using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AutoClicker.view
{
    /// <summary>
    /// MouseCursorWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MouseCursorWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void notify(string propertyName)
        {
            if (null != PropertyChanged)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private bool mDecisionEnabled = false;
        private bool mDecision = false;

        public bool DecisionEnabled
        {
            get { return mDecisionEnabled; }
            set
            {
                if (value != mDecisionEnabled)
                {
                    mDecisionEnabled = value;
                    notify("DecisionEnabled");
                }
            }
        }


        public bool Decision
        {
            get { return mDecision; }
            set {
                if (value != mDecision)
                {
                    mDecision = value;
                    notify("Decision");
                }
            }
        }

        public MouseCursorWindow()
        {
            InitializeComponent();
            DataContext = this;
            Visibility = Visibility.Hidden;
        }

        //private static MouseCursorWindow sInstance;
        //public static MouseCursorWindow Instance
        //{
        //    get
        //    {
        //        if (null == sInstance)
        //        {
        //            sInstance = new MouseCursorWindow();
        //        }
        //        return sInstance;
        //    }
        //}

        //public static void Terminate()
        //{
        //    if(null!=sInstance)
        //    {
        //        sInstance.Close();
        //        sInstance = null;
        //    }
        //}

        public void SetPos(System.Drawing.Point pos)
        {
            Left = pos.X - Width / 2;
            Top = pos.Y - Height / 2;
        }

        private int mWatchCount = 0;
        private int mLastCount = 0;
        public void ShowAt(System.Drawing.Point pos, int sec)
        {
            SetPos(pos);

            mLastCount = sec;
            mWatchCount = 0;

            if (Visibility == Visibility.Visible)
            {
                return;
            }

            if (sec <= 0)
            {
                //Visibility = Visibility.Visible;
                sec = 3; 
            }
            else
            {
                var timer = new DispatcherTimer();
                timer.Interval = new TimeSpan(0, 0, 1);
                timer.Tick += (s, e) =>
                {
                    mWatchCount++;
                    if (mWatchCount >= mLastCount)
                    {
                        timer.Stop();
                        //Visibility = Visibility.Hidden;
                        Close();
                    }
                };
                timer.Start();
                Visibility = Visibility.Visible;
            }
        }

        //public void Dismiss()
        //{
        //    Visibility = Visibility.Hidden;
        //}

        public static void Show(System.Drawing.Point pos)
        {
            var w = new MouseCursorWindow();
            w.DecisionEnabled = false;
            w.ShowAt(pos, 3);
        }

        public static void Show(System.Drawing.Point pos, bool decision)
        {
            var w = new MouseCursorWindow();
            w.DecisionEnabled = true;
            w.Decision = decision;
            w.ShowAt(pos, 3);
        }

    }
}
