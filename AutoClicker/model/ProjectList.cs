using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoClicker.model
{
    /**
     * Project List Class
     */
    public class ProjectList
    {
        // Fields ...
        private ObservableCollection<Project> mProjects;
        private bool mNeedsTruncateId;
        private bool mIsModified = false;

        #region Properties
        /**
         * Project Listをリストビューに表示するためのデータソース
         */
        public ObservableCollection<Project> Projects
        {
            get { return mProjects; }
        }

        /**
         * アイテム数
         */
        [System.Xml.Serialization.XmlIgnore]
        public int Count
        {
            get { return mProjects.Count; }
        }

        /**
         * プロジェクトID発行用プロパティ
         */
        private int mNextId = 0;

        public int NextId 
        { 
            get { return mNextId; } 
            set { if (mNextId != value) { mNextId = value; mIsModified = true; } } 
        }

        public Registration Registrations;

        [System.Xml.Serialization.XmlIgnore]
        public bool IsModified
        {
            get
            {
                if (mIsModified||Registrations.IsModified) return true;
                foreach (var p in mProjects)
                {
                    if (p.IsModified)
                    {
                        return true;
                    }
                }
                return false;
            }
            set 
            { 
                mIsModified = value;
                if (value == false)
                {
                    Registrations.IsModified = false;
                    foreach (var p in mProjects)
                    {
                        p.IsModified = false;
                    }
                }          
            }
        }
        #endregion


        #region Initializing

        /**
         * コンストラクタ
         */
        public ProjectList()
        {
            mProjects = new ObservableCollection<Project>();
            mNeedsTruncateId = false;
            NextId = 0;
            Registrations = new Registration();
        }


        /**
         * 最低限１個のプロジェクトがリスト内に存在することを保証する
         */
        public void AtLeastOneProject()
        {
            if (Projects.Count == 0)
            {
                // プロジェクトが空なら、デフォルトのプロジェクトを１つ作る
                Projects.Add(Project.DefaultProject);
                IsModified = true;
            }
        }
        #endregion

        #region Projects management
        /**
         * 複数のプロジェクトをマージして新たなデータソースを作成する。
         */
        public static ObservableCollection<TargetItem> MargeProjects(IList projects)
        {
            Debug.Assert(projects.Count > 1);


            List<TargetItem> wk = new List<TargetItem>(128);
            int timing;
            foreach (Project p in projects)
            {
                timing = 0;
                for (int i = 0, ci = p.Targets.Count; i < ci; i++)
                {
                    TargetItem t = p.Targets[i];
                    timing += t._RawWait;
                    t.Timing = timing;
                    wk.Add(t);
                }
            }
            wk.Sort((a, b) => { return a.Timing - b.Timing; });
            return new ObservableCollection<TargetItem>(wk);
        }

        //public void InitProjectId()
        //{
        //    if (NextId != 0)
        //        return;


        //    bool needsAsign = false;
        //    foreach (var proj in mProjects)
        //    {
        //        if (proj.ID == 0)
        //        {
        //            needsAsign = true;
        //        }
        //        else if(NextId < proj.ID)
        //        {
        //            NextId = proj.ID;
        //        }
        //    }
        //    NextId++;
        //    if (needsAsign)
        //    {
        //        foreach (var proj in mProjects)
        //        {
        //            if (proj.ID == 0)
        //            {
        //                proj.ID = NextId;
        //                NextId++;
        //            }
        //        }
        //    }
        //}

        /**
         * NextIdは、default.cli に保存されるので、正常に動作している限りは、正しい値を保持しているはずだが、
         * 万が一更新されていない値を保持していると、ＩＤの重複が発生してしまう。それを回避するため、
         * 安全のため、読み込み後にチェックして誤りを修正する。
         */
        public void CheckAndRepairNextId()
        {
            bool needsRepairId = false;
            int newid = 0;
            var hash = new HashSet<int>();
            foreach (var proj in mProjects)
            {
                if(proj.ID==0)
                {
                    // invalid ID
                    needsRepairId = true;
                    break;
                }
                if(hash.Contains(proj.ID))
                {
                    // duplicated
                    needsRepairId = true;
                    break;
                }
                hash.Add(proj.ID);
                if(newid<=proj.ID)
                {
                    newid = proj.ID+1;
                }
            }
            if(needsRepairId)
            {
                repairProjectId();
                return;
            }

            if (NextId<newid)
            {
                NextId = newid;
                IsModified = true;
            }
        }

        private void repairProjectId()
        {
            Registrations.UnregisterAll();

            int newid = 0;
            foreach (var proj in mProjects)
            {
                proj.ID = ++newid;
            }
            IsModified = true;
        }

        public void TruncateProjectId()
        {
            if (mNeedsTruncateId)
            {
                bool needsRepair = false;
                bool modified = false;
                int newid = 1;
                Dictionary<int, int> map = null;
                foreach (var proj in mProjects)
                {
                    if (proj.ID != newid)
                    {
                        if (null == map)
                        {
                            map = new Dictionary<int, int>();
                        }
                        if(map.ContainsKey(proj.ID))
                        {
                            // duplicate key
                            needsRepair = true;
                            break;
                        }
                        map.Add(proj.ID, newid);
                        proj.ID = newid;
                        modified = true;
                    }
                    newid++;
                }

                mNeedsTruncateId = false;
                if (needsRepair)
                {
                    repairProjectId();
                    return;
                }

                if(NextId == newid)
                {
                    NextId = newid;
                    modified = true;
                }
                if(modified)
                {
                    IsModified = true;
                }
                Registrations.ReMapProjectId(map);
            }
        }

        public void AddProject(Project proj)
        {
            proj.ID = NextId;
            NextId++;
            mProjects.Add(proj);
            IsModified = true;
        }

        public void InsertProject(int index, Project proj)
        {
            if (index < 0)
            {
                index = 0;
            }
            if (index >= mProjects.Count)
            {
                AddProject(proj);
            }
            else
            {
                proj.ID = NextId;
                NextId++;
                mProjects.Insert(index, proj);
                IsModified = true;
            }
        }

        public void RemoveProject(Project proj)
        {
            Registrations.OnRemoveProject(proj.ID);
            mProjects.Remove(proj);
            mNeedsTruncateId = true;
            IsModified = true;
        }

        public Project FindProjectById(int id)
        {
            foreach (var p in mProjects)
            {
                if (id == p.ID)
                {
                    return p;
                }
            }
            return null;
        }

        internal void ClearUtilizationCounter()
        {
            foreach(var p in mProjects)
            {
                p.ClearUtilizationCounter();
            }
        }


        #endregion

    }
}
