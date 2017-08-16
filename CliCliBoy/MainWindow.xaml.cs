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
using System.Collections;

using System.Runtime.InteropServices;
using System.Collections.ObjectModel;
using System.Diagnostics;
using CliCliBoy.interop;
using Point = System.Drawing.Point;
using System.ComponentModel;
using CliCliBoy.model;
using System.Globalization;
using System.IO;
using CliCliBoy.view;

namespace CliCliBoy
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        /**
         * ビューの状態
         */
        enum Status {
            INITIAL,
            //ADD_TARGET,
            //EDIT_TARGET,
            GETTING_POINT,      // マウス位置取得中
            //BASE_POINT,
            OPERATING,
            SHOW_DIALOG,
        }

        private Status mStatus;         // ビューの状態（currentStatusプロパティの中の人）
        private Manager mContext;       // バインドするデータのおやびん

        /**
         * コンストラクタ
         */
        public MainWindow()
        {
            InitializeComponent();
            mStatus = Status.INITIAL;
            mContext = Globals.Instance.DataContext;
            mContext.BindView(this);
            EditTargetPanel.SamplingColorPanel = SamplingColor;
            this.DataContext = mContext;
        }

        private void updateTitle()
        {
            string fname = Globals.Instance.DataFilePath != null ? System.IO.Path.GetFileName(Globals.Instance.DataFilePath) : "<untitled>";
            var v = FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
            //Debug.WriteLine(v.ToString());
            this.Title = String.Format("{0} (v{1}.{2}.{3})  - {4}", v.ProductName, v.FileMajorPart, v.FileMinorPart, v.FileBuildPart, fname);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            // プロジェクト選択状態を復元する。
            mContext.SelectProjectsFromIdList();

            updateTitle();
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            if(mContext.IsModified)
            {
                HandleFileSave(sender, null);
            }
            MouseCursorWindow.Terminate();
        }


        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            Globals.Instance.Settings.Placement.ApplyPlacementTo(this);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Globals.Instance.Settings.Placement.GetPlacementFrom(this);

            mContext.UpdateSelectedProjectIdList(ProjectListView.SelectedItems, false);
            //if (ProjectListView.SelectedIndex >= 0)
            //{
            //    foreach (Project proj in ProjectListView.SelectedItems)
            //    {
            //        mContext.SelectedProjectIDs.Add(proj.ID);
            //    }
            //}

            base.OnClosing(e);
        }

        #region View State
        /**
         * ビューの状態取得/変更操作用プロパティ
         */
        private Status currentStatus
        {
            get
            {
                return mStatus;
            }
            set
            {
                if (mStatus != value)
                {
                    mStatus = value;
                    UIElement activePanel = this.activePanel(mStatus);
                    foreach(UIElement e in MainContainer.Children) {
                        if (activePanel == e)
                        {
                            e.Visibility = System.Windows.Visibility.Visible;
                        }
                        else
                        {
                            e.Visibility = System.Windows.Visibility.Hidden;
                        }
                    }
                }

            }
        }

        /**
         * 状態に対応するパネルを取得する
         */
        private UIElement activePanel(Status status)
        {
            switch (status)
            {
                default:
                case Status.INITIAL:
                    return this.SettingPanel;
                case Status.GETTING_POINT:
                    return this.GettingPointPanel;
                //case Status.ADD_TARGET:
                //case Status.EDIT_TARGET:
                //    return this.EditTargetPanel;
                case Status.OPERATING:
                    return this.OperatingPanel;
                case Status.SHOW_DIALOG:
                    return this.Dialog;
            }
        }

        public void BackToInitialPanel()
        {
            currentStatus = Status.INITIAL;
        }

        public void EditHotKey()
        {
            HotKeyPanel.Init(mContext.HotKey);
            ShowDialog(HotKeyPanel.Dialog, (dlg) =>
            {
                mContext.HotKey = HotKeyPanel.HotKey;
            });
        }

        #endregion

        #region Projects
        /**
         * プロジェクトの作成
         * @param   index   リスト上の追加位置 / -1なら最後に追加
         */
        private void createProject(int index)
        {
            mContext.PlayStop();

            EditProjectPanel.Clear();
            ShowDialog(EditProjectPanel.Dialog, (dlg) =>
            {
                Project proj = new Project();
                EditProjectPanel.GetSettingResult(proj);
                if (index < 0)
                {
                    mContext.Projects.AddProject(proj);
                    index = mContext.Projects.Count-1;
                }
                else
                {
                    mContext.Projects.InsertProject(index, proj);
                }
                this.ProjectListView.SelectedItems.Clear();
                this.ProjectListView.SelectedItems.Add(proj);
                this.ProjectListView.ScrollIntoView(proj);
            }, ProjectListView);
        }

        /**
         * 現在選択中のプロジェクト（単一選択時のみ）の編集
         */
        private void modifyCurrentProject()
        {
            if (!mContext.IsSingleProjectSelection)
            {
                return;
            }
            mContext.PlayStop();

            EditProjectPanel.InitByProject(mContext.CurrentProject);
            ShowDialog(EditProjectPanel.Dialog, (dlg) =>
            {
                //Project proj = new Project();
                EditProjectPanel.GetSettingResult(mContext.CurrentProject);
                ProjectListView.ScrollIntoView(mContext.CurrentProject);
                mContext.NotifyPropsChangedDependOnProject();   // Absolute-->Relative n
            }, ProjectListView);
        }

        /**
         * 選択されたプロジェクト（複数可）を削除する
         */
        private void deleteSelectedProjects()
        {
            int sel = this.ProjectListView.SelectedIndex;
            if (sel < 0)
            {
                return;
            }
            object[] items = new ArrayList(this.ProjectListView.SelectedItems).ToArray();
            if (items.Length <= 0)
            {
                return;
            }
            mContext.PlayStop();
            mContext.SuspendHotKey();
            if( MessageBoxResult.Yes == System.Windows.MessageBox.Show( items.Length==1 ? "Are you sure to delete this project?" : "Are you sure to delete these projects?", "Delete Project(s)", MessageBoxButton.YesNo)) {
                foreach (Project p in items)
                {
                    mContext.Projects.RemoveProject(p);
                }
                // 最低１つのプロジェクトが存在することを保証
                mContext.Projects.AtLeastOneProject();

                if (sel >= mContext.Projects.Count)
                {
                    sel = mContext.Projects.Count - 1;
                }

                ProjectListView.SelectedIndex = sel;
            }
            mContext.ResumeHotKey();
        }

        /**
         * 現在選択中のRelativeモードプロジェクト（単一選択時のみ）のBasePointを平行移動（adjust）する。
         */
        private void translateProjectBasePoint()
        {
            mContext.PlayStop();

            if (!mContext.IsRelative)
            {
                return;
            }
            int x=0, y=0;
            uint initRatio = 100;
            var top = ProjectListView.SelectedItem as Project;
            if (null != top)
            {
                initRatio = top.Ratio;
                x = top.Mode.BasePoint.X;
                y = top.Mode.BasePoint.Y;
            }

            GetPosition((pos) =>
            {
                foreach (Project p in ProjectListView.SelectedItems)
                {
                    p.TranslateBasePoint(pos);
                }
            }, 
            (ratio)=>
            {
                foreach (Project p in ProjectListView.SelectedItems)
                {
                    p.Ratio = ratio;
                }
            }, initRatio, x, y, Keyboard.FocusedElement);
        }

        #endregion

        #region Click Target

        /**
         * Click Target を作成する
         * @param index 作成位置（-1ならリストの最後に追加）
         */
        private void createTarget(int index)
        {
            if (!mContext.IsSingleProjectSelection)
            {
                return;
            }
            mContext.PlayStop();

            EditTargetPanel.Init();
            ShowDialog(EditTargetPanel.Dialog, (dlg) =>
            {
                TargetItem item = new TargetItem(EditTargetPanel.Result);
                if (index < 0)
                {
                    mContext.CurrentProject.AddTarget(item);
                }
                else
                {
                    mContext.CurrentProject.InsertTarget(index, item);
                }
                TargetListView.SelectedItems.Clear();
                TargetListView.SelectedItems.Add(item);
                TargetListView.ScrollIntoView(item);
            }, TargetListView);
            EditTargetPanel.SetPoint();
        }

        /**
         * Click Targetの編集
         */
        private void modifyTargetItem(TargetItem item)
        {
            //if (!mContext.IsSingleProjectSelection)
            //{
            //    return;
            //}

            int index = mContext.CurrentTargets.IndexOf(item);
            if (index < 0)
            {
                return;
            }
            mContext.PlayStop();

            EditTargetPanel.InitByTarget(item);
            ShowDialog(EditTargetPanel.Dialog, (dlg) =>
            {
                //mContext.CurrentProject.ReplaceTarget(index, new TargetItem(EditTargetPanel.Result));
                item.CopyFrom(EditTargetPanel.Result);
                mContext.CurrentProject.AdjustPoint(item);
                TargetListView.ScrollIntoView(item);
            }, TargetListView);
            if (item.Type != ClickType.NOOP &&
                (Keyboard.GetKeyStates(Key.LeftCtrl) & KeyStates.Down) == KeyStates.Down )
            {
                EditTargetPanel.SetPoint();
            }
        }

        /**
         * クリックターゲットを削除
         */
        private void deleteSelectedTargetItems()
        {
            if (!mContext.IsSingleProjectSelection)
            {
                return;
            }

            int sel = this.TargetListView.SelectedIndex;
            if (sel < 0)
            {
                return;
            }
            object[] items = new ArrayList(this.TargetListView.SelectedItems).ToArray();
            if (items.Length <= 0)
            {
                return;
            }

            mContext.PlayStop();


            mContext.SuspendHotKey();
            if( MessageBoxResult.Yes == System.Windows.MessageBox.Show( items.Length==1 ? "Are you sure to delete this item?" : "Are you sure to delete these items?",  "Delete Item(s)", MessageBoxButton.YesNo)) {
                foreach (TargetItem p in items)
                {
                    mContext.CurrentTargets.Remove(p);
                }
                if (sel >= mContext.CurrentTargets.Count)
                {
                    sel = mContext.CurrentTargets.Count - 1;
                }

                TargetListView.SelectedIndex = sel;
            }
            mContext.ResumeHotKey();
        }

        /**
         * Playerによってターゲット指定されたときの処理
         */
        private void onItemTargeted(int index, TargetItem item)
        {
            //Debug.WriteLine("Click At : " + index.ToString());
            TargetListView.SelectedItem = item;
        }


        #endregion

        #region List Handling (shared with projects and targets)

        class CompareIndex : IComparer
        {
            private IList mSource;
            public CompareIndex(IList source)
            {
                mSource = source;
            }
            public int Compare(object x, object y)
            {
                return mSource.IndexOf(x) - mSource.IndexOf(y);
            }
        }

        delegate void MoveItem(int oldIndex, int newIndex);
        private void moveUp(System.Windows.Controls.ListView view, IList source, MoveItem move)
        {
            int index = view.SelectedIndex;
            if (index <= 0)
            {
                return;
            }
            mContext.PlayStop();

            var sel = new ArrayList(view.SelectedItems);

            //Debug.WriteLine("Before Sort");
            //foreach (var item in sel)
            //{
            //    Debug.WriteLine("Select: {0}", source.IndexOf(item));
            //}

            sel.Sort(new CompareIndex(source));
            //Debug.WriteLine("After Sort");
            //foreach (var item in sel)
            //{
            //    Debug.WriteLine("Select: {0}", source.IndexOf(item));
            //}


            index = source.IndexOf(sel[0]);
            foreach (var item in sel)
            {
                move(source.IndexOf(item), index - 1);
                index++;
            }
        }

        private void moveDown(System.Windows.Controls.ListView view, IList source, MoveItem move)
        {
            int index = view.SelectedIndex;
            if (index <= 0)
            {
                return;
            }
            mContext.PlayStop();

            var sel = new ArrayList(view.SelectedItems);
            sel.Sort(new CompareIndex(source));

            index = source.IndexOf(sel[sel.Count - 1]);
            if (index >= source.Count - 1)
            {
                return;
            }

            for (int i = sel.Count - 1; i >= 0; i-- )
            {
                move(source.IndexOf(sel[i]), index + 1);
                index--;
            }
        }
        #endregion

        #region Dialog Panels


        /**
         * 位置設定パネルを開く
         */
        private void GetPosition(PosGotCallback callback, RatioGotCallback ratioCallback=null, uint initRatio=100, int x=0, int y=0, IInputElement nextFocus=null)
        {
            Status orgStatus = this.currentStatus;
            var focus = nextFocus!=null ? nextFocus : Keyboard.FocusedElement;

            this.currentStatus = Status.GETTING_POINT;

            mContext.PlayStop();
            mContext.SuspendHotKey();

           
            GettingPointPanel.Open((ok) =>
            {
                this.currentStatus = orgStatus;
                mContext.ResumeHotKey();
                if (ok&&null!=ratioCallback)
                {
                    ratioCallback(GettingPointPanel.Ratio);
                }
                if (null != focus)
                {
                    Keyboard.Focus(focus);
                }
            }, callback, ratioCallback, initRatio, x, y);
        }

        /**
         * ダイアログパネルを表示する
         */
        private void ShowDialog(IDialog dlg, DialogHandler onOk, IInputElement nextFocus=null)
        {
            Status org = currentStatus;

            var focus = null!= nextFocus ? nextFocus : Keyboard.FocusedElement;

            mContext.PlayStop();
            mContext.SuspendHotKey();
            dlg.RequestGetPositionHandler = (c, gotpos) =>
            {
                GetPosition(gotpos);
            };
            dlg.Show(onOk, (c) =>
            {
                currentStatus = org;
                dlg.RequestGetPositionHandler = null;
                mContext.ResumeHotKey();

                if(null!=focus)
                {
                    Keyboard.Focus(focus);
                }
            });

            currentStatus = Status.SHOW_DIALOG;
        }
        #endregion

        #region Event Handler for Project


        private void BtnAddProj_Click(object sender, RoutedEventArgs e)
        {
            createProject(-1);
        }

        //private void BtnTranslateBasePoint_Click(object sender, RoutedEventArgs e)
        //{
        //    translateProjectBasePoint();
        //}

        private void BtnProjUp_Click(object sender, RoutedEventArgs e)
        {
            moveUp(ProjectListView, mContext.Projects.Projects, (oldIndex, newIndex) =>
            {
                mContext.Projects.Projects.Move(oldIndex, newIndex);
            });
        }

        private void BtnProjDown_Click(object sender, RoutedEventArgs e)
        {
            moveDown(ProjectListView, mContext.Projects.Projects, (oldIndex, newIndex) =>
            {
                mContext.Projects.Projects.Move(oldIndex, newIndex);
            });
        }

        #endregion
        
        #region Event Handler for Click Targets

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            createTarget(-1);
        }

        private void BtnUp_Click(object sender, RoutedEventArgs e)
        {
            if (!mContext.IsSingleProjectSelection)
            {
                return;
            }

            moveUp(TargetListView, mContext.CurrentTargets, (oldIndex, newIndex) =>
            {
                mContext.CurrentTargets.Move(oldIndex, newIndex);
            });
        }

        private void BtnDown_Click(object sender, RoutedEventArgs e)
        {
            if (!mContext.IsSingleProjectSelection)
            {
                return;
            }

            moveDown(TargetListView, mContext.CurrentTargets, (oldIndex, newIndex) =>
            {
                mContext.CurrentTargets.Move(oldIndex, newIndex);
            });
        }

        #endregion

        #region Other Event Handlers

        /**
         * リスト（Projects/Targets)ダブルクリック時の処理
         */
        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (((System.Windows.Controls.ListViewItem)sender).Content is TargetItem)
            {
                TargetItem item = (TargetItem)((System.Windows.Controls.ListViewItem)sender).Content;
                if (null != item)
                {
                    System.Windows.Point pos = e.GetPosition(TargetListView);
                    if (pos.X < 40)
                    {
                        item.Enabled = !item.Enabled;
                    }
                    else
                    {
                        modifyTargetItem(item);
                    }
                }
            }
            else if (((System.Windows.Controls.ListViewItem)sender).Content is Project)
            {
                translateProjectBasePoint();
            }
        }

        /**
         * ホットキー設定ボタンクリック
         */
        private void BtnHotkey_Click(object sender, RoutedEventArgs e)
        {
            EditHotKey();
        }

        /**
         * Playボタンクリック
         */
        private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {
            if (!mContext.IsPlaying &&
                (Keyboard.GetKeyStates(Key.LeftCtrl) & KeyStates.Down) == KeyStates.Down &&
                TargetListView.SelectedIndex >= 0)
            {
                if (mContext.PlayStartFrom(TargetListView.SelectedIndex))
                {
                    return;
                }
            }
            mContext.PlayToggle();
        }

        /**
         * 操作パネルを開くボタンクリック
         */
        private void BtnShowOparatingPanel_Click(object sender, RoutedEventArgs e)
        {
            currentStatus = Status.OPERATING;
        }


        private void Button_Registration(object sender, RoutedEventArgs e)
        {
            mContext.CurrentProjectsToRegistration(int.Parse((string)((System.Windows.Controls.MenuItem)sender).CommandParameter));
        }


        private void BtnRegist_Click(object sender, RoutedEventArgs e)
        {
            RegistrationItem item = ((System.Windows.Controls.RadioButton)sender).DataContext as RegistrationItem;
            if (null != item)
            {
                if (item.IsRegistered)
                {
                    mContext.ApplyRegistration(item);
                }
                else
                {
                    ((System.Windows.Controls.RadioButton)sender).IsChecked = false;
                }
            }
        }

        private void BtnRegist_RightClick(object sender, MouseButtonEventArgs e)
        {
            RegistrationItem item = ((System.Windows.Controls.RadioButton)sender).DataContext as RegistrationItem;
            if (null != item )
            {
                if (item.IsRegistered)
                {
                    item.Unregister();
                }
                else
                {
                    mContext.CurrentProjectsToRegistration(item);
                }

            }
        }

        #endregion

        private void TargetPointCheck(object sender, MouseButtonEventArgs e)
        {
            if (!mContext.CheckMode)
            {
                return;
            }
            var target = ((FrameworkElement)sender).DataContext as TargetItem;
            if (null != target)
            {
                MouseCursorWindow.Instance.DecisionEnabled = false;
                MouseCursorWindow.Instance.ShowAt(target.Clicker.ClickPoint, 3);
            }
        }

        private void ConditionPointCheck(object sender, MouseButtonEventArgs e)
        {
            if (!mContext.CheckMode)
            {
                return;
            }
            var target = ((FrameworkElement)sender).DataContext as TargetItem;
            if (null != target && target.Condition.Type!=ClickCondition.ConditionType.NONE)
            {
                StringBuilder sb = new StringBuilder(DebugOutput.Text);
                MouseCursorWindow.Instance.DecisionEnabled = true;
                MouseCursorWindow.Instance.Decision = target.Condition.Decide(sb);
                MouseCursorWindow.Instance.ShowAt(target.Condition.ScreenPoint.AbsolutePoint, 3);
                DebugOutput.Text = sb.ToString();
                DebugOutput.ScrollToEnd();
            }
        }

        /**
         * キー操作/SCメニュー用コマンド定義
         */
        // for Project Pane
        public readonly static RoutedCommand CopyProject      = new RoutedCommand("CopyProject", typeof(MainWindow));
        public readonly static RoutedCommand CutProject       = new RoutedCommand("CutProject", typeof(MainWindow));
        public readonly static RoutedCommand PasteProject     = new RoutedCommand("PasteProject", typeof(MainWindow));
        public readonly static RoutedCommand EditProject      = new RoutedCommand("EditProject", typeof(MainWindow));
        public readonly static RoutedCommand TranslateProject = new RoutedCommand("TranslateProject", typeof(MainWindow));
        public readonly static RoutedCommand InsertProject    = new RoutedCommand("InsertProject", typeof(MainWindow));
        public readonly static RoutedCommand DeleteProject    = new RoutedCommand("DeleteProject", typeof(MainWindow));
        public readonly static RoutedCommand ExportProject    = new RoutedCommand("ExportProject", typeof(MainWindow));
        public readonly static RoutedCommand ImportProject    = new RoutedCommand("ImportProject", typeof(MainWindow));
        // for Target Pane
        public readonly static RoutedCommand CopyTarget       = new RoutedCommand("CopyTarget", typeof(MainWindow));
        public readonly static RoutedCommand CutTarget        = new RoutedCommand("CutTarget", typeof(MainWindow));
        public readonly static RoutedCommand PasteTarget      = new RoutedCommand("PasteTarget", typeof(MainWindow));
        public readonly static RoutedCommand EditTarget       = new RoutedCommand("EditTarget", typeof(MainWindow));
        public readonly static RoutedCommand InsertTarget     = new RoutedCommand("InsertTarget", typeof(MainWindow));
        public readonly static RoutedCommand DeleteTarget     = new RoutedCommand("DeleteTarget", typeof(MainWindow));
        // for File Operation
        public readonly static RoutedCommand FileNew          = new RoutedCommand("FileNew", typeof(MainWindow));
        public readonly static RoutedCommand FileOpen         = new RoutedCommand("FileOpen", typeof(MainWindow));
        public readonly static RoutedCommand FileSave         = new RoutedCommand("FileSave", typeof(MainWindow));
        public readonly static RoutedCommand FileSaveAs       = new RoutedCommand("FileSaveAs", typeof(MainWindow));


        private void copySelectedProject(bool cut)
        {
            if(ProjectClipboardAccessor.Instance.PutToClipboard(ProjectListView.SelectedItems)&&cut)
            {
                deleteSelectedProjects();
            }
        }
        // コマンドハンドラ
        private void HandleCopyProject(object sender, ExecutedRoutedEventArgs e)
        {
            copySelectedProject(false);
        }
        private void HandleCutProject(object sender, ExecutedRoutedEventArgs e)
        {
            copySelectedProject(true);
        }

        /**
         * 複数のプロジェクトオブジェクト（配列）をプロジェクトリストの指定位置に挿入。
         */
        private void insertProjects(object[] items)
        {
            int index = ProjectListView.SelectedIndex;
            if(index<0)
            {
                index = ProjectListView.Items.Count-1;
            }
            ProjectListView.SelectedItems.Clear();
            foreach(var item in items)
            {
                index++;
                mContext.Projects.InsertProject(index, (Project)item);
                ProjectListView.SelectedItems.Add(item);
            }

        }

        private void HandlePasteProject(object sender, ExecutedRoutedEventArgs e)
        {
            var projs = ProjectClipboardAccessor.Instance.GetFromClipboard();
            if (null != projs)
            {
                insertProjects(projs);
                return;
            }

            HandlePasteTarget(sender, e);
        }
        private void HandleTranslateProject(object sender, ExecutedRoutedEventArgs e)
        {
            translateProjectBasePoint();
        }
        private void HandleEditProject(object sender, ExecutedRoutedEventArgs e)
        {
            modifyCurrentProject();
        }

        private void HandleInsertProject(object sender, ExecutedRoutedEventArgs e)
        {
            int sel = this.ProjectListView.SelectedIndex;
            createProject(sel);
        }
        private void HandleDeleteProject(object sender, ExecutedRoutedEventArgs e)
        {
            deleteSelectedProjects();
        }

        private void HandleExportProject(object sender, ExecutedRoutedEventArgs e)
        {
            if (ProjectListView.SelectedItems.Count > 0)
            {

                var dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.FileName = "clicli.cli";
                dlg.Filter = "Cli Files(*.cli)|*.cli|All Files(*.*)|*.*";
                dlg.RestoreDirectory = true;
                if (dlg.ShowDialog() == true)
                {
                    var lists = new List<Project>(ProjectListView.SelectedItems.Count);
                    foreach (var p in ProjectListView.SelectedItems)
                    {
                        lists.Add((Project)p);
                    }

                    System.IO.StreamWriter sw = null;
                    try
                    {
                        System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(List<Project>));
                        //書き込むファイルを開く（UTF-8 BOM無し）
                        sw = new System.IO.StreamWriter(dlg.FileName, false, new System.Text.UTF8Encoding(false));
                        //シリアル化し、XMLファイルに保存する
                        serializer.Serialize(sw, lists);
                    }
                    catch (Exception ex)
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
                }
            }
        }
        private void HandleImportProject(object sender, ExecutedRoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "clicli.cli";
            dlg.Filter = "Cli Files(*.cli)|*.cli|All Files(*.*)|*.*";
            dlg.RestoreDirectory = true;
            if (dlg.ShowDialog() == true)
            {
                System.IO.StreamReader sr = null;
                try
                {
                    //XmlSerializerオブジェクトを作成
                    System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(List<Project>));

                    //読み込むファイルを開く
                    sr = new System.IO.StreamReader(dlg.FileName, new System.Text.UTF8Encoding(false));

                    //XMLファイルから読み込み、逆シリアル化する
                    var lists = serializer.Deserialize(sr) as List<Project>;
                    if (null != lists && lists.Count > 0)
                    {
                        insertProjects(lists.ToArray());

                        //int index = ProjectListView.SelectedIndex;
                        //for (int i = lists.Count - 1; i >= 0; i--)
                        //{
                        //    mContext.Projects.InsertProject(index, lists[i]);
                        //}
                    }

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
                finally
                {
                    if (null != sr)
                    {
                        //ファイルを閉じる
                        sr.Close();
                    }
                }

            }
        }

        private void copySelectedTarget(bool cut)
        {
            int sel = this.TargetListView.SelectedIndex;
            if (sel < 0)
            {
                return;
            }
            if(TargetClipboardAccessor.Instance.PutToClipboard(TargetListView.SelectedItems) && cut)
            {
                deleteSelectedTargetItems();
            }
            
        }


        private void HandleCopyTarget(object sender, ExecutedRoutedEventArgs e)
        {
            copySelectedTarget(false);
        }

        private void HandleCutTarget(object sender, ExecutedRoutedEventArgs e)
        {
            copySelectedTarget(true);
        }

        private void HandlePasteTarget(object sender, ExecutedRoutedEventArgs e)
        {
            var clip = TargetClipboardAccessor.Instance;
            var items = clip.GetFromClipboard();


            //if(System.Windows.Clipboard.ContainsData("CliCliTarget"))
            //{
            //    string cd = System.Windows.Clipboard.GetData("CliCliTarget") as string;
            //    if(null!=cd)
            //    {
            //var items = TargetArray.Deserialize(cd);
            if (null != items)
            {
                int index = TargetListView.SelectedIndex;
                if(index<0)
                {
                    index = TargetListView.Items.Count - 1;
                }
                TargetListView.SelectedItems.Clear();
                foreach (TargetItem item in items)
                {
                    index++;
                    mContext.CurrentProject.InsertTarget(index, item);
                    TargetListView.SelectedItems.Add(item);
                }

            }

            //    }
            //}
        }

        private void HandleEditTarget(object sender, ExecutedRoutedEventArgs e)
        {
            //if (!mContext.IsSingleProjectSelection)
            //{
            //    return;
            //}

            var items = this.TargetListView.SelectedItems;
            if (items.Count != 1)
            {
                return;
            }
            modifyTargetItem((TargetItem)items[0]);
        }


        private void HandleDeleteTarget(object sender, ExecutedRoutedEventArgs e)
        {
            deleteSelectedTargetItems();
        }

        private void HandleInsertTarget(object sender, ExecutedRoutedEventArgs e)
        {
            int index = TargetListView.SelectedIndex;
            createTarget(index);
        }

        void CanInsertTarget(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = mContext.IsSingleProjectSelection;
        }

        void CanDeleteTarget(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = mContext.IsSingleProjectSelection && this.TargetListView.SelectedItems.Count > 0;
        }

        void CanEditTarget(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.TargetListView.SelectedItems.Count == 1;
        }

        void CanExecOnSingleProjTarget(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = e.CanExecute = mContext.IsSingleProjectSelection && this.TargetListView.SelectedItems.Count == 1;
        }

        void CanExecOnProjSelected(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !mContext?.NoProjectSelected ?? false;
        }
        void CanExecAlways(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        
        void CanExecOnTargetSelected(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.TargetListView.SelectedItems.Count > 0;
        }

        void CanPasteProject(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ProjectClipboardAccessor.Instance.HasDataOnClipboard ||
                           (mContext.IsSingleProjectSelection && TargetClipboardAccessor.Instance.HasDataOnClipboard);
        }

        void CanPasteTarget(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = mContext.IsSingleProjectSelection && TargetClipboardAccessor.Instance.HasDataOnClipboard;
        }

        private double mDebugOutputHeight = 80d;
        private void checkMode_Checked(object sender, RoutedEventArgs e)
        {
            TargetGrid.RowDefinitions[2].Height = new GridLength(mDebugOutputHeight);
        }

        private void checkMode_Unchecked(object sender, RoutedEventArgs e)
        {
            var v = TargetGrid.RowDefinitions[2].Height.Value;
            if(v>0)
            {
                mDebugOutputHeight = v;
            }
            DebugOutput.Text = "";  // クリアボタンを用意するのが面倒なので、offにするときにクリアしてみる。
            TargetGrid.RowDefinitions[2].Height = new GridLength(0d);
        }

        private void HandleFileNew(object sender, ExecutedRoutedEventArgs e)
        {
            if(mContext.IsModified)
            {
                HandleFileSave(sender, e);
            }
            Globals.Instance.DataFilePath = null;
            var manager = new Manager();
            mContext.UnbindView();

            Globals.Instance.DataContext = manager;
            mContext = manager;
            mContext.BindView(this);
            this.DataContext = mContext;

            mContext.SelectProjectsFromIdList();
            updateTitle();
        }

        private void HandleFileOpen(object sender, ExecutedRoutedEventArgs e)
        {
            if (mContext.IsModified)
            {
                HandleFileSave(sender, e);
            }
            var dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = Globals.Instance.DataFilePath ?? "";
            dlg.Filter = "CliCli Files(*.clicli)|*.clicli|All Files(*.*)|*.*";
            dlg.RestoreDirectory = true;
            if (dlg.ShowDialog() == true)
            {
                Globals.Instance.DataFilePath = dlg.FileName;
                var manager = Manager.Deserialize();

                mContext.UnbindView();

                Globals.Instance.DataContext = manager;
                mContext = manager;
                mContext.BindView(this);
                this.DataContext = mContext;

                mContext.SelectProjectsFromIdList();
                updateTitle();
            }
        }

        private void HandleFileSave(object sender, ExecutedRoutedEventArgs e)
        {
            if(Globals.Instance.DataFilePath == null)
            {
                HandleFileSaveAs(sender, e);
                return;
            }
            Globals.Instance.DataContext.Serialize();
        }

        private void HandleFileSaveAs(object sender, ExecutedRoutedEventArgs e)
        {
            if (ProjectListView.SelectedItems.Count > 0)
            {
                var dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.FileName = Globals.Instance.DataFilePath;
                dlg.Filter = "CliCli Files(*.clicli)|*.clicli|All Files(*.*)|*.*";
                dlg.RestoreDirectory = true;
                if (dlg.ShowDialog() == true)
                {
                    Globals.Instance.DataFilePath = dlg.FileName;
                    Globals.Instance.DataContext.Serialize();
                    updateTitle();
                }
            }
        }

        private void FileMenu_Opened(object sender, RoutedEventArgs e)
        {
            var menu = sender as ContextMenu;
            if(null!=menu)
            {
                while(menu.Items.Count>4)
                {
                    menu.Items.RemoveAt(4);
                }
                if(Globals.Instance.Settings.HasMru)
                {
                    MenuItem item;
                    menu.Items.Add(new Separator());
                    foreach(var s in Globals.Instance.Settings.MRU)
                    {
                        if(s != Globals.Instance.DataFilePath)
                        {
                            item = new MenuItem();
                            item.Header = s;
                            item.CommandParameter = s;
                            item.Click += OnMruCommand;
                            menu.Items.Add(item);
                        }
                    }
                    menu.Items.Add(new Separator());

                    item = new MenuItem();
                    item.Header = "Clear MRU";
                    item.Click += ClearMru;
                    menu.Items.Add(item);
                }
            }
        }

        private void ClearMru(object sender, RoutedEventArgs e)
        {
            Globals.Instance.Settings.MRU.Clear();
            Globals.Instance.Settings.AddMru(Globals.Instance.DataFilePath);
        }

        private void OnMruCommand(object sender, RoutedEventArgs e)
        {
            var item = sender as MenuItem;
            if(null!=item)
            {
                Globals.Instance.DataFilePath = (string)item.CommandParameter;
                var manager = Manager.Deserialize();

                mContext.UnbindView();

                Globals.Instance.DataContext = manager;
                mContext = manager;
                mContext.BindView(this);
                this.DataContext = mContext;

                mContext.SelectProjectsFromIdList();
                updateTitle();

            }
        }
    }
}
