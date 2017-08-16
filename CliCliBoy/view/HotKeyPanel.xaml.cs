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
    /// OperationHotKey.xaml の相互作用ロジック
    /// </summary>
    public partial class HotKeyPanel : UserControl
    {
        private DialogHelper mDialogHelper;

        public IDialog Dialog { get { return mDialogHelper; } }

        public class HotKeyContext : Notifier
        {
            HotKeyIndex mHotKey;
            public HotKeyContext(HotKeyIndex hk = HotKeyIndex.HK_NONE)
            {
                mHotKey = hk;
            }
            public HotKeyIndex HotKey
            {
                get { return mHotKey; }
                set
                {
                    mHotKey = value;
                    notify("HotKey");
                }
            }
        }

        public HotKeyIndex HotKey { get { return ((HotKeyContext)this.DataContext).HotKey; } }

        public HotKeyPanel()
        {
            InitializeComponent();
            this.DataContext = new HotKeyContext();
            mDialogHelper = new DialogHelper(this);
        }

        public void Init(HotKeyIndex hk)
        {
            this.DataContext = new HotKeyContext(hk);
        }

        private void Button_Ok(object sender, RoutedEventArgs e)
        {
            mDialogHelper.Close(true);
        }

        private void Button_Cancel(object sender, RoutedEventArgs e)
        {
            mDialogHelper.Close(false);
        }
    }
}
