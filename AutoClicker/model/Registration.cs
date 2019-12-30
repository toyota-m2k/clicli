using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoClicker.model
{
    /**
     * プロジェクトの組み合わせを登録しておくための仕掛け
     * １組分の登録情報
     */
    public class RegistrationItem : Notifier
    {
        private List<int> mProjects;

        [System.Xml.Serialization.XmlIgnore]
        public bool IsModified { get; set; }

        
        public List<int> Projects
        {
            get { return mProjects; }
            set { mProjects = value; }
        }

        public int RegistNo { get; set; }

        [System.Xml.Serialization.XmlIgnore]
        public bool IsRegistered { get { return mProjects.Count!=0; } }

        [System.Xml.Serialization.XmlIgnore]
        public bool IsSelected
        {
            get
            {
                if (mProjects.Count > 0)
                {
                    return Globals.Instance.DataContext.SelectedProjectIDs.SequenceEqual(mProjects);
                }
                return false;
            }
            set { }
        }

        public RegistrationItem()
        {
            mProjects = null;
            RegistNo = 0;
            IsModified = false;
        }

        public RegistrationItem(int no)
        {
            mProjects = new List<int>();
            RegistNo = no;
            IsModified = false;
        }

        public void Register(IList projs)
        {
            if (null==projs || projs.Count == 0)
            {
                Unregister();
                return;
            }

            mProjects.Clear();
            mProjects.Capacity = projs.Count;
            foreach (Project p in projs)
            {
                mProjects.Add(p.ID);
            }
            mProjects.Sort();
            notify("IsRegistered");
            notify("IsSelected");
            IsModified = true;
        }

        public void Unregister(int id)
        {
            if (mProjects.Count > 0)
            {
                mProjects.Remove(id);
                if (mProjects.Count == 0)
                {
                    notify("IsRegistered");
                    notify("IsSelected");
                }
                IsModified = true;
            }
        }

        public void Unregister()
        {
            mProjects.Clear();
            notify("IsRegistered");
            notify("IsSelected");
            IsModified = true;
        }

        public void UpdateSelection()
        {
            notify("IsSelected");
        }

        public void ReMapProjectId(Dictionary<int, int> map)
        {
            bool modified = false;
            for(int i=mProjects.Count-1; i>=0; i--)
            {
                int nid;
                if(map.TryGetValue(mProjects[i], out nid)) {
                    mProjects[i] = nid;
                    modified = true;
                }
            }
            if(modified)
            {
                IsModified = true;
            }
        }
    }

    /**
     * プロジェクトの組み合わせを登録しておくための仕掛け
     * RegistrationItemを配列として保持するクラス
     */
    public class Registration
    {
        public const int REGIST_COUNT = 5;
        private RegistrationItem[] mRegItems;
        private bool mIsModified = false;

        [System.Xml.Serialization.XmlIgnore]
        public bool IsModified
        {
            get
            {
                if (mIsModified) return true;
                foreach (var r in mRegItems)
                {
                    if (r.IsModified)
                    {
                        return true;
                    }
                    return false;
                }
                return false;
            }
            set
            {
                mIsModified = value;
                if (value == false)
                {
                    foreach (var r in mRegItems)
                    {
                        r.IsModified = false;
                    }
                }
            }
        }

        public RegistrationItem[] RegItems
        {
            get { return mRegItems; }
            set { mRegItems = value; }
        }

        public Registration()
        {
            mRegItems = new RegistrationItem[REGIST_COUNT];
            for (int i = 0; i < REGIST_COUNT; i++)
            {
                mRegItems[i] = new RegistrationItem(i+1);
            }
        }

        public void UnregisterAll()
        {
            for (int i = 0; i < REGIST_COUNT; i++)
            {
                mRegItems[i].Unregister();
            }
        }

        public RegistrationItem this[int index]
        {
            get { return mRegItems[index]; }
        }

        public void OnRemoveProject(int id)
        {
            foreach (RegistrationItem ri in mRegItems)
            {
                ri.Unregister(id);
            }
        }


        internal void OnProjectSelected()
        {
            foreach (RegistrationItem ri in mRegItems)
            {
                ri.UpdateSelection();
            }
        }

        internal void ReMapProjectId(Dictionary<int, int> map)
        {
            mIsModified = true;
            foreach (RegistrationItem ri in mRegItems)
            {
                ri.ReMapProjectId(map);
            }
        }

    }
}
