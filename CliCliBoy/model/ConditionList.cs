using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliCliBoy.model
{
    public class ConditionList : Notifier
    {
        List<ClickCondition> mList;

        public ConditionList()
        {
            mList = new List<ClickCondition>(3);
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
