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
using System.Windows.Shapes;

namespace CliCliBoy.view
{
    /// <summary>
    /// EditTargetPanel.xaml の相互作用ロジック
    /// </summary>
    public partial class TargetEditPanel : UserControl
    {
        private TargetItem mSettingTarget;
        private DialogHelper mDialogHelper;

        public SamplingColor SamplingColorPanel
        {
            get; set;
        }

        public IDialog Dialog { get { return mDialogHelper; } }

        public TargetEditPanel()
        {
            InitializeComponent();
            mDialogHelper = new DialogHelper(this);
            PositionTuner.OnCompleted += OnPositionCompleted;
            Init();
        }

        public void Init()
        {
            mSettingTarget = new TargetItem();
            mSettingTarget.Type = ClickType.NOOP;
            this.DataContext = mSettingTarget;
        }

        public void InitByTarget(TargetItem target)
        {
            mSettingTarget = new TargetItem(target);
            //mSettingTarget.ScreenPoint.ResetBasePoint();
            this.DataContext = mSettingTarget;
        }

        public TargetItem Result
        {
            get { return mSettingTarget; }
        }

        public void SetPoint()
        {
            mDialogHelper.GetPosition((pos) =>
            {
                mSettingTarget.Clicker.ClickPoint = pos;
                mSettingTarget.Type = ClickType.CLICK;
            });
        }

        private void Button_SetPoint(object sender, RoutedEventArgs e)
        {
            SetPoint();
        }

        private void Button_Ok(object sender, RoutedEventArgs e)
        {
            mSettingTarget.Wait = int.Parse(NtWait.Text);
            mSettingTarget.Repeat = int.Parse(NtRepeat.Text);
            mSettingTarget.Comment = TxComment.Text;

            mDialogHelper.Close(true);
        }

        private void Button_Cancel(object sender, RoutedEventArgs e)
        {
            mDialogHelper.Close(false);
        }

        private void BtnPreset_3000(object sender, RoutedEventArgs e)
        {
            mSettingTarget.Wait = 3000;
        }
        private void BtnPreset_1000(object sender, RoutedEventArgs e)
        {
            mSettingTarget.Wait = 1000;
        }
        private void BtnPreset_500(object sender, RoutedEventArgs e)
        {
            mSettingTarget.Wait = 500;
        }
        private void BtnPreset_100(object sender, RoutedEventArgs e)
        {
            mSettingTarget.Wait = 100;
        }
        private void BtnPreset_50(object sender, RoutedEventArgs e)
        {
            mSettingTarget.Wait = 50;
        }

        private void Button_Move(object sender, RoutedEventArgs e)
        {
            //MouseEmulator.MoveTo(mSettingTarget.Clicker.ClickPoint);
            var cm = MouseCursorWindow.Instance;
            cm.DecisionEnabled = false;
            cm.ShowAt(mSettingTarget.Clicker.ClickPoint, 5);
        }

        private void Button_Test(object sender, RoutedEventArgs e)
        {
            switch (mSettingTarget.Type)
            {
                case ClickType.CLICK:
                    MouseEmulator.ClickAt(mSettingTarget.Clicker.ClickPoint, null, false);
                    break;
                case ClickType.DBLCLK:
                    MouseEmulator.ClickAt(mSettingTarget.Clicker.ClickPoint, null, true);
                    break;
                case ClickType.WHEEL:
                    MouseEmulator.WheelAt(mSettingTarget.Clicker.ClickPoint, mSettingTarget.WheelAmount, null);
                    break;
                case ClickType.NOOP:
                default:
                    break;
            }
        }

        private void Button_SetCondition(object sender, RoutedEventArgs e)
        {
            SamplingColorPanel.Dialog.Show((dlg)=>
            {
                mSettingTarget.Condition = SamplingColorPanel.Result;
            }, (dlg)=>
            {
                this.Visibility = Visibility.Visible;
            });
            SamplingColorPanel.Init(mSettingTarget.Condition, mSettingTarget.ScreenPoint.AbsolutePoint);
            this.Visibility = Visibility.Hidden;
        }

        private void Button_TryDecide(object sender, RoutedEventArgs e)
        {
            MouseCursorWindow.Instance.DecisionEnabled = true;
            MouseCursorWindow.Instance.Decision = mSettingTarget.Condition.Decide();
            MouseCursorWindow.Instance.ShowAt(mSettingTarget.Condition.ScreenPoint.AbsolutePoint, 3);

        }

        private void Button_Adjust(object sender, RoutedEventArgs e)
        {
            PositionTuner.Init(mSettingTarget.ScreenPoint.AbsolutePoint);
            TargetEditMain.Visibility = Visibility.Hidden;
            TargetEditSub.Visibility = Visibility.Visible;
        }

        private void Button_TuneOk(object sender, RoutedEventArgs e)
        {
            TargetEditSub.Visibility = Visibility.Hidden;
            TargetEditMain.Visibility = Visibility.Visible;
        }

        private void OnPositionCompleted(bool flag, PositionTuner pt)
        {
            if(flag)
            {
                mSettingTarget.Clicker.ClickPoint = pt.Result;
            }
            TargetEditSub.Visibility = Visibility.Hidden;
            TargetEditMain.Visibility = Visibility.Visible;
        }
    }
}
