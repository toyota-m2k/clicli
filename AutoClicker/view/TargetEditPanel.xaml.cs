using AutoClicker.interop;
using AutoClicker.model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace AutoClicker.view
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

            mSettingTarget.ConditionList.PrepareForEditing();
            CollectionViewSource source = new CollectionViewSource();
            source.Source = mSettingTarget.ConditionList.List;
            this.ConditionListView.DataContext = source;
        }

        public void InitByTarget(TargetItem target)
        {
            mSettingTarget = new TargetItem(target);
            //mSettingTarget.ScreenPoint.ResetBasePoint();
            this.DataContext = mSettingTarget;

            mSettingTarget.ConditionList.PrepareForEditing();
            CollectionViewSource source = new CollectionViewSource();
            source.Source = mSettingTarget.ConditionList.List;
            this.ConditionListView.DataContext = source;
        }

        public TargetItem Result
        {
            get
            {
                mSettingTarget.ConditionList.TrimAfterEditing();
                return mSettingTarget;
            }
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
            MouseCursorWindow.Show(mSettingTarget.Clicker.ClickPoint);
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

        //private void Button_SetCondition(object sender, RoutedEventArgs e)
        //{
        //    SamplingColorPanel.Dialog.Show((dlg)=>
        //    {
        //        mSettingTarget.Condition = SamplingColorPanel.Result;
        //    }, (dlg)=>
        //    {
        //        this.Visibility = Visibility.Visible;
        //    });
        //    SamplingColorPanel.Init(mSettingTarget.Condition, mSettingTarget.ScreenPoint.AbsolutePoint);
        //    this.Visibility = Visibility.Hidden;
        //}

        private void Button_TryDecide(object sender, RoutedEventArgs e)
        {
            //MouseCursorWindow.Instance.DecisionEnabled = true;
            //MouseCursorWindow.Instance.Decision = mSettingTarget.Condition.Decide();
            //MouseCursorWindow.Instance.ShowAt(mSettingTarget.Condition.ScreenPoint.AbsolutePoint, 3);

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
            if (flag)
            {
                mSettingTarget.Clicker.ClickPoint = pt.Result;
            }
            TargetEditSub.Visibility = Visibility.Hidden;
            TargetEditMain.Visibility = Visibility.Visible;
        }

        private void showConditionPosition(ConditionList.Condition condition)
        {
            if (null != condition)
            {
                MouseCursorWindow.Show(condition.ScreenPoint.AbsolutePoint, condition.Decide(null, true));
            }
        }

        private void addCondition()
        {
            SamplingColorPanel.Dialog.Show((dlg) =>
            {
                mSettingTarget.ConditionList.Add(SamplingColorPanel.Result);
            }, (dlg) =>
            {
                this.Visibility = Visibility.Visible;
            });
            SamplingColorPanel.Init(new ConditionList.Condition(), mSettingTarget.ScreenPoint.AbsolutePoint);
            this.Visibility = Visibility.Hidden;
        }

        private void editCondition(ConditionList.Condition condition)
        {
            SamplingColorPanel.Dialog.Show((dlg) =>
            {
                mSettingTarget.ConditionList.Update(condition, SamplingColorPanel.Result);
            }, (dlg) =>
            {
                this.Visibility = Visibility.Visible;
            });
            SamplingColorPanel.Init(condition.Clone(), mSettingTarget.ScreenPoint.AbsolutePoint);
            this.Visibility = Visibility.Hidden;
        }


        private void deleteCondition(IList conditions)
        {
            if (conditions.Count > 0)
            {
                var sel = new List<ConditionList.Condition>(conditions.Count);
                foreach (ConditionList.Condition c in conditions)
                {
                    sel.Add(c);
                }
                mSettingTarget.ConditionList.Remove(sel);
            }
        }


        private void onConditonCoordinateClicked(object sender, RoutedEventArgs e)
        {
            var condition = ((FrameworkContentElement)sender).DataContext as ConditionList.Condition;
            showConditionPosition(condition);
        }

        private void onAddCondition(object sender, RoutedEventArgs e)
        {
            addCondition();
        }

        private void onDeleteSelectedConditions(object sender, RoutedEventArgs e)
        {
            var selected = ConditionListView.SelectedItems;
            deleteCondition(selected);
        }

        private void ListViewItem_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is Hyperlink)
            {
                e.Handled = false;
                return;
            }

            var item = sender as ListViewItem;
            if (item != null)
            {
                var condition = item.DataContext as ConditionList.Condition;
                if(null!=condition && !condition.IsValid)
                {
                    addCondition();
                }
            }
        }
        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            if (null != item)
            {
                var condition = item.DataContext as ConditionList.Condition;
                if (null != condition)
                {
                    editCondition(condition);
                }
            }
        }

        private void onEditSelectedConditions(object sender, RoutedEventArgs e)
        {
            editCondition(ConditionListView.SelectedItem as ConditionList.Condition);
        }
    }
}
