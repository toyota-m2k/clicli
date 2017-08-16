using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace CliCliBoy.model
{
    public class Notifier : INotifyPropertyChanged
    {
        // 変更通知用
        public event PropertyChangedEventHandler PropertyChanged;
        protected void notify(string propertyName)
        {
            if (null != PropertyChanged)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected void notify(string[] propertyNames)
        {
            if (null != PropertyChanged)
            {
                foreach (var n in propertyNames)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(n));
                }
            }
        }
    }
}
