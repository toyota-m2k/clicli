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
using System.Windows.Shapes;

namespace CliCliBoy.view
{
    /// <summary>
    /// ShowConditions.xaml の相互作用ロジック
    /// </summary>
    public partial class ConditionView : Window
    {
        private ConditionList mConditionList;

        public ConditionView(ConditionList list)
        {
            InitializeComponent();
            mConditionList = list;
        }



        private void Window_LostFocus(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public static void Show(ConditionList list, Window owner, FrameworkElement anchor)
        {
            var dlg = new ConditionView(list);
            Point lt = owner.PointFromScreen(anchor.PointToScreen(new Point(0, 0)));
            Point pos = new Point(0, 0);
            if(lt.Y>owner.Height/2)
            {
                // anchorの上に表示
                pos.Y = lt.Y - dlg.Height;
            }
            else
            {
                // anchorの下に表示
                pos.Y = lt.Y + anchor.Height;
                if(pos.Y + dlg.Height > owner.Height)
                {
                    pos.Y = owner.Height - dlg.Height;
                }
            }

            // Xはanchor右合わせ
            pos.X = lt.X + anchor.Width - dlg.Width;

            if (pos.Y < 0)
            {
                pos.Y = 0;
            }
            if (pos.X < 0)
            {
                pos.X = 0;
            }

            pos = owner.PointToScreen(pos);
            dlg.WindowStartupLocation = WindowStartupLocation.Manual;
            //dlg.Owner = owner;
            dlg.Left = pos.X;
            dlg.Top = pos.Y;
            dlg.Show();
        }

        private void onConditonCoordinateClicked(object sender, RoutedEventArgs e)
        {
            var condition = ((FrameworkContentElement)sender).DataContext as ConditionList.Condition;
            if (null != condition)
            {
                MouseCursorWindow.Show(condition.ScreenPoint.AbsolutePoint, condition.Decide(null, true));
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = mConditionList;

            CollectionViewSource source = new CollectionViewSource();
            source.Source = mConditionList.List;
            this.ConditionListView.DataContext = source;
        }


        private void Window_Deactivated(object sender, EventArgs e)
        {
            Close();
        }
    }
}
