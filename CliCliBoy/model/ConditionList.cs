using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliCliBoy.model
{
    public class ConditionList : Notifier
    {
        ObservableCollection<ClickCondition> mList;

        public ConditionList()
        {
            mList = new ObservableCollection<ClickCondition>();
        }

        public enum ConditionCombination {
            CMB_AND,
            CMB_OR,
        }



        private ConditionCombination mCombination = ConditionCombination.CMB_AND;
        public ConditionCombination Combination
        {
            get
            {
                return mCombination;
            }
            set
            {
                mCombination = value;
            }
        }

    }
}
