using CliCliBoy.interop;
using CliCliBoy.view;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace CliCliBoy.model
{
    public class Manager : Notifier
    {
        #region fields
        
        private ProjectList mProjects;
        private Project mCurrentProject;
        private Project mEmptyProject;
        private ObservableCollection<TargetItem> mCurrentTargets;
        private WeakReference<CliCliBoy.MainWindow> mMainWindow;
        private PlayerList mPlayers;
        private bool mIsPlaying;
        private HotKeyDriver mHotKeyDriver;
        private List<int> mSelectedProjectIds;
        
        private static readonly string[] sPropsDependOnProject = { "CurrentProject", "CurrentTargets", "IsMargedTarget", "IsSingleProjectSelection", "NoProjectSelected", "IsRelative" };
        
        #endregion

        #region Initialize

        /**
         * コンストラクタ
         */
        public Manager()
        {
            mProjects = new ProjectList();
            mEmptyProject = new Project();
            mCurrentProject = mEmptyProject;
            mCurrentTargets = mEmptyProject.Targets;
            mPlayers = new PlayerList();
            mHotKeyDriver = new HotKeyDriver();
            mSelectedProjectIds = new List<int>(8);
        }

        /**
         * Viewへの参照、イベントハンドラなどを接続する
         */
        public void BindView(MainWindow mainWindow)
        {
            mPlayers.Targeted += onTargeted;
            mPlayers.PlayStateChanged += onPlayStateChanged;
            mHotKeyDriver.TogglePlay = PlayToggle;

            mMainWindow = new WeakReference<CliCliBoy.MainWindow>(mainWindow);

            CollectionViewSource source = new CollectionViewSource();
            source.Source = mProjects.Projects;

            mainWindow.ProjectListView.DataContext = source;
            mainWindow.ProjectListView.SelectionChanged += projectListView_SelectionChanged;

            // プロジェクトが空なら、デフォルトのプロジェクトを１つ作る
            // mProjects.AtLeastOneProject();
            // 先頭のプロジェクトを選択
            // mainWindow.ProjectListView.SelectedIndex = 0;

            mainWindow.OperatingPanel.Manager = this;
        }

        public void UnbindView()
        {
            mPlayers.Targeted -= onTargeted;
            mPlayers.PlayStateChanged -= onPlayStateChanged;
            mHotKeyDriver.Dispose();

            var mainWindow = MainWindow;
            if (null != mainWindow)
            {
                mainWindow.ProjectListView.DataContext = null;
                mainWindow.ProjectListView.SelectionChanged -= projectListView_SelectionChanged;
                mMainWindow.SetTarget(null);
            }
        }
        #endregion

        #region View References (Properties)
        /**
         * MainWindow
         */
        [System.Xml.Serialization.XmlIgnore]
        public MainWindow MainWindow
        {
            get
            {
                MainWindow win;
                if (mMainWindow.TryGetTarget(out win))
                {
                    return win;
                }
                return null;
            }
        }

        /**
         * Targets ListView
         */
        [System.Xml.Serialization.XmlIgnore]
        public ListView TargetListView
        {
            get
            {
                MainWindow win = this.MainWindow;
                return (null != win) ? win.TargetListView : null;
            }
        }

        /**
         * Projects ListView
         */
        [System.Xml.Serialization.XmlIgnore]
        public ListView ProjectListView
        {
            get
            {
                MainWindow win = this.MainWindow;
                return (null != win) ? win.ProjectListView: null;
            }
        }

        /**
         * OperatingPanel View
         */
        [System.Xml.Serialization.XmlIgnore]
        public OperatingPanel OperatingPanel
        {
            get
            {
                MainWindow win = this.MainWindow;
                return (null != win) ? win.OperatingPanel : null;
            }
        }

        #endregion

        #region Hot Key Management

        /**
         * 選択されている HotKey を特定するプロパティ
         */
        public HotKeyIndex HotKey
        {
            get { return mHotKeyDriver.CurrentHotKey; }
            set { 
                mHotKeyDriver.CurrentHotKey = value;
                notify("HotKey");
                notify("HotKeyName");
                IsModified = true;
            }
        }

        /**
         * HotKey の名前
         */
        [System.Xml.Serialization.XmlIgnore]
        public string HotKeyName
        {
            get { return mHotKeyDriver.CurrentHotKeyName; }
        }

        /**
         * HotKeyの動作を一時的に停止（解除）する
         */
        public void SuspendHotKey()
        {
            mHotKeyDriver.Suspending = true;
        }

        /**
         * 停止中のHotKey動作を再開する
         */
        public void ResumeHotKey()
        {
            mHotKeyDriver.Suspending = false;
        }
        #endregion

        #region Project Management

        /**
         * プロジェクトリスト プロパティ
         */
        public ProjectList Projects
        {
            get { return mProjects; }
            set { 
                mProjects = value; 
                IsModified = true;
            }      // XML Serialize用に必要
        }

        /**
         * 複数プロジェクトが選択（マージ）されているか？
         */
        [System.Xml.Serialization.XmlIgnore]
        public bool IsMargedTarget
        {
            get { return mCurrentTargets != mCurrentProject.Targets; }
        }

        /**
         * 単一プロジェクトが選択されているか？
         */
        [System.Xml.Serialization.XmlIgnore]
        public bool IsSingleProjectSelection
        {
            get { return mCurrentProject != mEmptyProject && mCurrentTargets == mCurrentProject.Targets; }
        }

        /**
         * プロジェクトが一つも選択されていないか？
         */
        [System.Xml.Serialization.XmlIgnore]
        public bool NoProjectSelected
        {
            get { return !IsMargedTarget && mCurrentProject == mEmptyProject; }
        }

        /**
         * 現在選択中のプロジェクト
         */
        [System.Xml.Serialization.XmlIgnore]
        public Project CurrentProject
        {
            get { return mCurrentProject; }
        }

        /**
         * 現在選択中のターゲットアイテムの配列
         */
        [System.Xml.Serialization.XmlIgnore]
        public ObservableCollection<TargetItem> CurrentTargets
        {
            get { return mCurrentTargets; }
        }

        /**
         * 現在選択されているプロジェクトのIDの配列（ソートして取得）
         */
        //[System.Xml.Serialization.XmlIgnore]
        public List<int> SelectedProjectIDs
        {
            get
            {
                return mSelectedProjectIds;
            }
            set
            {
                mSelectedProjectIds = value;
            }
        }

        [System.Xml.Serialization.XmlIgnore]
        public bool IsRelative
        {
            get
            {
                ListView view;
                if ((view = this.ProjectListView) != null)
                {
                    var list = view.SelectedItems;
                    if (list.Count > 0)
                    {
                        foreach (Project p in list)
                        {
                            if (p.Mode.IsAbsolute)
                            {
                                return false;
                            }
                        }
                        return true;
                    }
                }
                return false;

            }
        }

        [System.Xml.Serialization.XmlIgnore]
        public bool CheckMode { get; set; } = false;

        [System.Xml.Serialization.XmlIgnore]
        public bool UtlizationMode { get; set; } = false;


        /**
         * プロジェクトが選択されたときの処理
         */
        private void onSelectProjects(IList projects)
        {
            PlayStop();

            if (null == projects || projects.Count == 0)
            {
                mCurrentProject = mEmptyProject;
                mCurrentTargets = mEmptyProject.Targets;
            }
            else if (projects.Count == 1)
            {
                mCurrentProject = (Project)projects[0];
                mCurrentTargets = mCurrentProject.Targets;
            }
            else
            {
                mCurrentProject = mEmptyProject;
                mCurrentTargets = ProjectList.MargeProjects(projects);
            }
            ListView view = this.TargetListView;
            if (null!=view)
            {
                CollectionViewSource source = new CollectionViewSource();
                source.Source = mCurrentTargets;
                view.DataContext = source;
            }
            mPlayers.UnregisterAll();

            UpdateSelectedProjectIdList(projects, true);
        }

        public void UpdateSelectedProjectIdList(IList projects, bool updateView)
        {
            mSelectedProjectIds.Clear();
            foreach (Project p in projects)
            {
                mSelectedProjectIds.Add(p.ID);
            }
            mSelectedProjectIds.Sort();

            if (updateView)
            {
                Registrations.OnProjectSelected();
                notify(sPropsDependOnProject);
            }
        }
        public void SelectProjectsFromIdList()
        {
            if (mSelectedProjectIds.Count > 0)
            {
                // ProjectListView.SelectedItemsを変更すると、ListChangedイベント経由で、mSelectedProjectIds に変更が加わるので、
                // ほかの変数に退避してからリストを更新する。
                var ids = mSelectedProjectIds;
                mSelectedProjectIds = new List<int>(Math.Max(8, ids.Count));

                ProjectListView.SelectedItems.Clear();
                foreach (int id in ids)
                {
                    Project proj = Projects.FindProjectById(id);
                    if (null != proj)
                    {
                        ProjectListView.SelectedItems.Add(proj);
                    }
                }
            }
            // 一つも選択されていなければ先頭を選択
            if (ProjectListView.SelectedItems.Count == 0)
            {
                if( ProjectListView.Items.Count == 0)
                {
                    mProjects.AtLeastOneProject();
                }
                ProjectListView.SelectedItems.Add(ProjectListView.Items[0]);
            }

        }

        public void NotifyPropsChangedDependOnProject()
        {
            notify(sPropsDependOnProject);
        }

        /**
         * ProjectListView の選択状態が変化したときのイベントハンドラ
         */
        void projectListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView view = this.ProjectListView;
            if (null != view)
            {
                onSelectProjects(view.SelectedItems);
            }
        }
        #endregion

        #region Player Management

        /**
         * くりくり実行中か？
         */
        [System.Xml.Serialization.XmlIgnore]
        public bool IsPlaying
        {
            get { return mIsPlaying; }
            private set { 
                mIsPlaying = value; 
                notify("IsPlaying"); 
            }
        }


        /**
         * くりくりスタート
         */
        public void PlayStart()
        {
            if (IsModified)
            {
                if (Serialize())
                {
                    Debug.WriteLine("Saved.");
                    IsModified = false;
                }

            }

            ListView view;
            if (!mPlayers.IsAvailable && (view=this.ProjectListView)!=null)
            {
                var list = view.SelectedItems;
                foreach (Project p in list)
                {
                    mPlayers.Register(p);
                }
            }
            mPlayers.Start();
        }

        public bool PlayStartFrom(int index)
        {
            if (IsPlaying || mPlayers.Count!=1)
            {
                return false;
            }
            mPlayers[0].StartFrom(index);
            return true;
        }

        /**
         * くりくりストップ
         */
        public void PlayStop()
        {
            mPlayers.Stop();
        }

        /**
         * くりくりのスタート・ストップをトグル
         */
        public void PlayToggle()
        {
            if (IsPlaying)
            {
                PlayStop();
            }
            else
            {
                PlayStart();
            }
        }

        /**
         * Playerの実行状態が変更したときのイベントハンドラ
         */
        private void onPlayStateChanged(bool playing)
        {
            IsPlaying = playing;
        }

        /**
         * Playerの次の標的が決まったときのイベントハンドラ
         */
        private void onTargeted(TargetItem item)
        {
            int index = mCurrentTargets.IndexOf(item);
            if (index >= 0)
            {
                ListView view = this.TargetListView;
                if (null != view)
                {
                    view.SelectedItem = item;
                }
            }
        }
        #endregion

        #region Registration Management

        /**
         * レジストレーションインスタンス取得
         */
        [System.Xml.Serialization.XmlIgnore]
        public Registration Registrations
        {
            get { return Projects.Registrations; }
        }

        /**
         * 指定されたレジストレーションを適用（プロジェクトを選択）
         */
        public void ApplyRegistration(RegistrationItem ri)
        {
            ListView view = this.ProjectListView;
            if (null == view)
            {
                return;

            }

            view.SelectedItems.Clear();
            foreach (int id in ri.Projects) 
            {
                Project proj = Projects.FindProjectById(id);
                if(null!=proj)
                {
                    view.SelectedItems.Add(proj);
                }
            }
        }

        /**
         * 現在のプロジェクト選択をレジストレーションに登録する。
         */
        public void CurrentProjectsToRegistration(RegistrationItem ri)
        {
            ListView view = this.ProjectListView;
            if (null != view)
            {
                ri.Register(view.SelectedItems);
            }
        }

        /**
         * 現在のプロジェクト選択をレジストレーションに登録する。
         */
        public void CurrentProjectsToRegistration(int regno)
        {
            if (regno <= 0 || Registration.REGIST_COUNT < regno)
            {

            }
            this.CurrentProjectsToRegistration(Registrations[regno-1]);
        }

        /**
         * レジストレーションを解除する。
         */
        public void ResetRegistration(RegistrationItem ri)
        {
            ri.Unregister();
        }
        #endregion

        #region Serialization

        private bool mIsModified = false;
        [System.Xml.Serialization.XmlIgnore]
        public bool IsModified
        {
            get { return mIsModified || Projects.IsModified; }
            private set
            {
                mIsModified = value;
                if (value == false)
                {
                    Projects.IsModified = false;
                }
            }
        }

        /**
         * PositionTuner
         */
        private PositionTuner.Magnification mPTMagnification = PositionTuner.Magnification.SMALL;
        public PositionTuner.Magnification PositionTunerMagnification
        {
            get { return mPTMagnification; }
            set { mPTMagnification = value; IsModified = true; }
        }

        /**
         * XMLファイルに書き出す
         */
        public bool Serialize()
        {
            bool result = false;
            //var curdir = Directory.GetCurrentDirectory();
            //Debug.WriteLine(curdir);
            Projects.TruncateProjectId();

            System.IO.StreamWriter sw = null;
            try
            {
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(Manager));
                //書き込むファイルを開く（UTF-8 BOM無し）
                sw = new System.IO.StreamWriter(Globals.Instance.DataFilePath, false, new System.Text.UTF8Encoding(false));
                //シリアル化し、XMLファイルに保存する
                serializer.Serialize(sw, this);
                result = true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            finally
            {
                //ファイルを閉じる
                if (null != sw)
                {
                    sw.Close();
                }
            }
            Globals.Instance.Settings.AddMru(Globals.Instance.DataFilePath);
            return result;

        }

        public static string makeUniqueFilePath(string orgPath, string ext)
        {
            string name = Path.GetFileNameWithoutExtension(orgPath);
            string newPath;
            int n = 0;
            do
            {
                newPath = name + ((n > 0) ? ('('+ n.ToString()+')') : "") + '.' + ext;
                n++;
            } while (File.Exists(newPath));
            return newPath;
        }

        /**
         * XMLファイルから読み出す
         */
        public static Manager Deserialize()
        {
            System.IO.StreamReader sr = null;
            Object obj = null;
            bool error = false;

            try
            {
                //XmlSerializerオブジェクトを作成
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(Manager));

                //読み込むファイルを開く
                sr = new System.IO.StreamReader(Globals.Instance.DataFilePath, new System.Text.UTF8Encoding(false));

                //XMLファイルから読み込み、逆シリアル化する
                obj = serializer.Deserialize(sr);

                if( obj is Manager)
                {
                    ((Manager)obj).IsModified = false;
                    ((Manager)obj).Projects.CheckAndRepairNextId();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                error = true;
                obj = new Manager();
            }
            finally
            {
                if (null != sr)
                {
                    //ファイルを閉じる
                    sr.Close();
                }
                // 旧バージョンからのコンバート用のため、ここでプロジェクトIDの初期化を実行しておく。（将来は不要になる予定）
                //((Manager)obj).Projects.InitProjectId();

                if (error)
                {
                    // エラーを起こしたソースファイルをバックアップしておく
                    if (File.Exists(Globals.Instance.DataFilePath))
                    {
                        File.Copy(Globals.Instance.DataFilePath, makeUniqueFilePath(Globals.Instance.DataFilePath, "bak"));
                    }
                }
            }
            Globals.Instance.Settings.AddMru(Globals.Instance.DataFilePath);
            return (Manager)obj;
        }
        #endregion

        void ClearUtilizationCounter()
        {
            mProjects.ClearUtilizationCounter();
        }

    }
}
