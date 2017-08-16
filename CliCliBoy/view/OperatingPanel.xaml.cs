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
    /// UserControl1.xaml の相互作用ロジック
    /// </summary>
    public partial class OperatingPanel : UserControl
    {
        public WeakReference<Manager> mManager;

        public OperatingPanel()
        {
            InitializeComponent();
            mManager = null;
        }

        public Manager Manager
        {
            get
            {
                Manager m;
                if (null != mManager && mManager.TryGetTarget(out m))
                {
                    return m;
                }
                return null;
            }
            set
            {
                mManager = new WeakReference<Manager>(value);
            }
        }

        private MainWindow MainWindow
        {
            get
            {
                Manager m = this.Manager;
                return (null != m) ? m.MainWindow : null;
            }
        }

        private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {
            Manager m = this.Manager;
            if (null != m )
            {
                m.PlayToggle();
            }
        }

        private void BtnHideOparatingPanel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow win = this.MainWindow;
            if (null != win)
            {
                win.BackToInitialPanel();
            }
        }

        private void BtnHotkey_Click(object sender, RoutedEventArgs e)
        {
            MainWindow win = this.MainWindow;
            if (null != win)
            {
                win.EditHotKey();
            }
        }

        private void BtnRegist_Click(object sender, RoutedEventArgs e)
        {
            Manager m = this.Manager;
            RegistrationItem item = ((RadioButton)sender).DataContext as RegistrationItem;
            if (null != item && null!=m)
            {
                if (item.IsRegistered)
                {
                    m.ApplyRegistration(item);
                }
                else
                {
                    ((RadioButton)sender).IsChecked = false;
                }
            }
        }

        private void BtnRegist_RightClick(object sender, MouseButtonEventArgs e)
        {
            Manager m = this.Manager;
            RegistrationItem item = ((RadioButton)sender).DataContext as RegistrationItem;
            if (null != item && null != m)
            {
                if (item.IsRegistered)
                {
                    item.Unregister();
                }
                else
                {
                    m.CurrentProjectsToRegistration(item);
                }

            }
        }

        private void SelectRegistration(RegistrationItem ri)
        {
            Grid v = this.RegistrationPane;
            foreach(var c in v.Children){
                RadioButton r = c as RadioButton;
                if (null != r)
                {
                    if (r.DataContext == ri)
                    {
                        r.IsChecked = true;
                    }
                    else
                    {
                        r.IsChecked = false;
                    }
                }
            }
        }

        private void OnVisibilityChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //if ((bool)e.NewValue)
            //{
            //    Manager m = this.Manager;
            //    if(null!=m){
            //        RegistrationItem ri = null;
            //        List<int> ids = m.SelectedProjectIDs;
            //        if (null != ids)
            //        {
            //            for (int i = 0; i < Registration.REGIST_COUNT; i++)
            //            {
            //                if (ids.SequenceEqual(m.Registrations[i].Projects))
            //                {
            //                    ri = m.Registrations[i];
            //                    break;
            //                }
            //            }
            //        }
            //        SelectRegistration(ri);
            //    }
            //}
        }
    }
}
