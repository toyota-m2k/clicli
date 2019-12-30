using AutoClicker.model;
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

namespace AutoClicker.view
{
    /// <summary>
    /// ProjectEditPanel.xaml の相互作用ロジック
    /// </summary>
    public partial class ProjectEditPanel : UserControl
    {
        private Project mSettingProject;
        private DialogHelper mDialogHelper;
        public IDialog Dialog { get { return mDialogHelper; } }

        public ProjectEditPanel()
        {
            InitializeComponent();
            mDialogHelper = new DialogHelper(this);
            PositionTuner.OnCompleted += OnPositionCompleted;
            Clear();    // 無駄だがXAMLのランタイム警告メッセージがでるのを回避。
        }

        public void Clear()
        {
            mSettingProject = new Project();
            mSettingProject.Name = "(noname)";
            this.DataContext = mSettingProject;
            
        }

        public void InitByProject(Project proj)
        {
            mSettingProject        = new Project();
            mSettingProject.Name   = proj.Name;
            mSettingProject.Mode.CopyFrom(proj.Mode);
            mSettingProject.Repeat = proj.Repeat;
            mSettingProject.URL    = proj.URL;
            this.DataContext       = mSettingProject;
        }

        public void GetSettingResult(Project proj)
        {
            proj.Name   = mSettingProject.Name;
            proj.Mode.CopyFrom(mSettingProject.Mode);
            proj.Repeat = mSettingProject.Repeat;
            proj.URL    = mSettingProject.URL;
        }

        private void Button_Ok(object sender, RoutedEventArgs e)
        {
            mSettingProject.Name = TxName.Text;     // TextBox内からEnterキー(DefaultKey)で抜けるとき、バインドによる更新が行われない。（TextBoxはkillfocusで更新されるらしい）
            mSettingProject.URL = TxUrl.Text;
            mDialogHelper.Close(true);
        }

        private void Button_Cancel(object sender, RoutedEventArgs e)
        {
            mDialogHelper.Close(false);
        }

        private void Button_SetPoint(object sender, RoutedEventArgs e)
        {
            mDialogHelper.GetPosition((pos) =>
            {
                mSettingProject.Mode.BasePoint = pos;
            });
        }

        private void Button_ShowPos(object sender, RoutedEventArgs e)
        {
            MouseCursorWindow.Show(mSettingProject.Mode.BasePoint);
        }

        private void Button_Adjust(object sender, RoutedEventArgs e)
        {
            PositionTuner.Init(mSettingProject.Mode.BasePoint);
            ProjectEditMain.Visibility = Visibility.Hidden;
            ProjectEditSub.Visibility = Visibility.Visible;
        }

        private void OnPositionCompleted(bool flag, PositionTuner pt)
        {
            if (flag)
            {
                mSettingProject.Mode.BasePoint = pt.Result;
            }
            ProjectEditSub.Visibility = Visibility.Hidden;
            ProjectEditMain.Visibility = Visibility.Visible;
        }


    }
}
