using CliCliBoy.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Point = System.Drawing.Point;

namespace CliCliBoy
{
    public delegate void DialogHandler(IDialog dlg);
    public delegate void PosGotCallback(Point p);
    public delegate void ColorGotCallback(Point p, HSVColorRange colors);
    public delegate void RequestGetPosition(IDialog dialog, PosGotCallback gotpos);
    public delegate void RatioGotCallback(uint ratio);
    
    public interface IDialog
    {
        UserControl Control { get; }
        RequestGetPosition RequestGetPositionHandler { get; set; }

        void Show(DialogHandler onOk, DialogHandler onClose);
        void Close(bool result);
        void GetPosition(PosGotCallback gotpos);
    }

    public class DialogHelper : IDialog
    {
        public RequestGetPosition RequestGetPositionHandler { get; set; }
        public UserControl Control { get; private set; }

        protected DialogHandler OnOk { get; set; }
        protected DialogHandler OnClose { get; set; }

        public DialogHelper(UserControl dlg)
        {
            Control = dlg;
        }

        public virtual void Show(DialogHandler onOk, DialogHandler onClose)
        {
            OnOk = onOk;
            OnClose = onClose;
            Control.Visibility = Visibility.Visible;
        }

        public virtual void Close(bool result)
        {
            if (result && null!=OnOk)
            {
                OnOk(this);
            }

            Control.Visibility = Visibility.Collapsed;
            if (null != OnClose)
            {
                OnClose(this);
            }
        }

        public void GetPosition(PosGotCallback gotpos)
        {
            if (null != RequestGetPositionHandler)
            {
                RequestGetPositionHandler(this, gotpos);
            }
        }
    }
}
