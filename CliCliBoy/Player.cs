using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CliCliBoy.model
{
    public class PlayerList
    {
        public delegate void TargetAt(TargetItem item);
        public event TargetAt Clicked;
        public event TargetAt Targeted;

        public delegate void PlayStateChangedHandler(bool playing);
        public event PlayStateChangedHandler PlayStateChanged;
        public int mPlayingCount;

        public class Player
        {
            DispatcherTimer mTimer;
            Project mProject;
            int mIndex;
            int mTargetRepeat;
            int mNextWait;
            int mRepeat;
            bool mPlaying;
            WeakReference<PlayerList> mParent;


            public Player(Project proj, PlayerList parent)
            {
                mIndex = 0;
                mProject = proj;
                mParent = new WeakReference<PlayerList>(parent);
                mTimer = new DispatcherTimer(DispatcherPriority.Normal);
                mTimer.Tick += new EventHandler(doAction);
                mPlaying = false;
            }

            private void onClicked(TargetItem item)
            {
                if (Globals.Instance.DataContext.UtlizationMode)
                {
                    item.Used();
                }
                PlayerList parent;
                if (mParent.TryGetTarget(out parent))
                {
                    parent.onClicked(item);
                }
            }
            private void onTargeted(TargetItem item)
            {
                PlayerList parent;
                if (mParent.TryGetTarget(out parent))
                {
                    parent.onTargeted(item);
                }
            }
            private void onStart()
            {
                PlayerList parent;
                if (mParent.TryGetTarget(out parent))
                {
                    parent.onStart(mProject);
                }
            }
            private void onStop()
            {
                PlayerList parent;
                if (mParent.TryGetTarget(out parent))
                {
                    parent.onStop(mProject);
                }
            }

            internal Project Project { get { return mProject; } }

            private void notifyParent()
            {
                PlayerList parent;
                if (mParent.TryGetTarget(out parent))
                {

                }
            }

            private void doAction(object obj, EventArgs args)
            {
                doAction();
            }

            private bool isReady()
            {
                return !KeyState.CheckSpecialKeyDown();
            }


            /**
             * TargetItemの実行条件(Condition)をチェックする
             * @param item    in  次に実行する予定のターゲット
             *                out 条件チェック＆スキップを行った結果、実際に実行することになったターゲット
             *                    null なら実行せずに処理を保留（次のタイミングでもう一度実行判断、または、処理終了）
             * @return        true: 処理継続可 / false: 処理終了
             * 
             */
            private bool checkCondition(ref TargetItem item)
            {
                var startItem = item;
                while (true)
                {
                    if (item.ConditionList.Decide(null, false))
                    {
                        return true;
                    }

                    if (item.ConditionList.Type == ConditionList.ActionType.WAIT)
                    {
                        //retryAfter();
                        item = null;
                        return true;
                    }
                    else
                    {
                        //return true;    // skip
                        if (!next())
                        {
                            //Stop();
                            return false;
                        }
                        item = mProject.Targets[mIndex];
                        if (item == startItem)
                        {
                            // 一巡した
                            item = null;
                            return true;
                        }
                    }
                }
            }

            private const int MIN_WAIT = 500;

            enum ActionResult {
                DONE,
                WAITING,
                EXIT
            }
            private ActionResult doit(TargetItem item)
            {
                mNextWait = 0;
                if(!checkCondition(ref item))
                {
                    // 終了
                    return ActionResult.EXIT;
                }
                if(null==item)
                {
                    // 保留
                    return ActionResult.WAITING;
                }

                if (isReady())
                {
                    mNextWait = item.Wait;
                    onTargeted(item);
                    if (item.Type == ClickType.NOOP)
                    {
                        return ActionResult.DONE;
                    }

                    if(item.MoveMouse)
                    {
                        interop.MouseEmulator.MoveMouse(item.Clicker.ClickPoint, isReady);
                    }
                    if (item.Type == ClickType.CLICK || item.Type == ClickType.DBLCLK)
                    {
                        if (interop.MouseEmulator.ClickAt(item.Clicker.ClickPoint, isReady, item.Type == ClickType.DBLCLK))
                        {
                            onClicked(item);
                            return ActionResult.DONE;
                        }
                    }
                    else if (item.Type == ClickType.WHEEL)
                    {
                        if (interop.MouseEmulator.WheelAt(item.Clicker.ClickPoint, item.WheelAmount, isReady))
                        {
                            onClicked(item);
                            return ActionResult.DONE;
                        }
                    }
                    else if (item.Type == ClickType.KEYPRESS)
                    {
                        if (interop.MouseEmulator.PressKey((int)item.PressKey, isReady))
                        {
                            onClicked(item);
                            return ActionResult.DONE;
                        }
                    }
                }
                return ActionResult.WAITING;
            }

            private void doAction()
            {
                StopTimer();

                TargetItem item = mProject.Targets[mIndex];
                switch(doit(item))
                {
                    case ActionResult.DONE:
                        if (!next())
                        {
                            Stop();
                            return;
                        }
                        break;
                    case ActionResult.EXIT:
                        Stop();
                        return;
                    case ActionResult.WAITING:
                        break;
                }

                mTimer.Interval = new TimeSpan(0, 0, 0, 0, Math.Max(mNextWait, MIN_WAIT));
                mTimer.Start();
            }

            //private bool check()
            //{
            //    if (mProject.Targets.Count <= 0)
            //    {
            //        return false;
            //    }
            //    for (int i = 0, ci = mProject.Targets.Count; i < ci; i++)
            //    {
            //        if (mProject.Targets[i].Enabled)
            //        {
            //            return true;
            //        }
            //    }
            //    return false;
            //}

            private bool next()
            {
                if (mProject.Targets.Count == 0)
                {
                    // ターゲットが空
                    return false;
                }

                bool begin = mIndex < 0;
                if(begin)
                {
                    // 開始直後(orプロジェクトリピート後の最初も含む）
                    if (mProject.Repeat > 0 && mRepeat >= mProject.Repeat)
                    {
                        return false;
                    }
                    mRepeat++;
                }
                else
                {
                    if (mIndex >= mProject.Targets.Count)
                    {
                        // 現在のインデックスがターゲット数を超えている（ありえないが念のため）
                        return false;
                    }

                    mTargetRepeat++;
                    if (mProject.Targets[mIndex].Repeat>0 && mTargetRepeat < mProject.Targets[mIndex].Repeat && mProject.Targets[mIndex].Enabled)
                    {
                        // ターゲットアイテムのリピート中（現在のアイテムにとどまる）
                        return true;
                    }
                }

                // 次に進む
                mTargetRepeat = 0;
                do
                {
                    mIndex++;
                    if (mIndex == mProject.Targets.Count)
                    {
                        // ターゲットの最後が終わった→先頭へ戻る
                        if (begin)
                        {
                            // 有効なアイテムが１つも見つからないうちに、末尾に達した
                            return false;
                        }
                        mIndex = -1;
                        return next();
                    }
                    
                } while (!mProject.Targets[mIndex].Enabled);

                return true;
            }


            public void StopTimer()
            {
                if (mTimer.IsEnabled)
                {
                    mTimer.Stop();
                }
            }

            public void Start()
            {
                if (mPlaying)
                {
                    return;
                }

                mIndex = -1;
                mTargetRepeat = 0;
                mRepeat = 0;
                mNextWait = 0;

                if (!next())
                {
                    return;
                }

                StopTimer();
                mPlaying = true;
                onStart();

                mTimer.Interval = new TimeSpan(0, 0, 0, 0, MIN_WAIT);
                mTimer.Start();
            }

            public void StartFrom(int index)
            {
                if (mPlaying)
                {
                    return;
                }

                mIndex = index;
                mTargetRepeat = 0;
                mRepeat = 0;
                mNextWait = 0;


                StopTimer();
                mPlaying = true;
                onStart();

                TargetItem item = mProject.Targets[mIndex];
                onTargeted(item);
                mTimer.Interval = new TimeSpan(0, 0, 0, 0, MIN_WAIT);
                mTimer.Start();
            }

            //public void StartFrom(int index)
            //{
            //    Stop();
            //    if (index >= mProject.Targets.Count)
            //    {
            //        index = mProject.Targets.Count - 1;
            //    }
            //    mIndex = index;
            //    mTargetRepeat = 0;
            //    Start();
            //}

            public void Stop()
            {
                StopTimer();
                if (!mPlaying)
                {
                    return;
                }

                mPlaying = false;
                onStop();
            }
        }

        private List<Player> mPlayers;
        private int mPlayCount;

        delegate void PlayerTrigger();
        private event PlayerTrigger StartTrigger;
        private event PlayerTrigger StopTrigger;

        public PlayerList()
        {
            mPlayers = new List<Player>();
            mPlayCount = 0;
        }

        Player playerFor(Project proj)
        {
            return mPlayers.Find((p) => { return proj == p.Project; });
        }

        public void Register(Project project)
        {
            if (null==playerFor(project))
            {
                Player player = new Player(project, this);
                StartTrigger += player.Start;
                StopTrigger += player.Stop;
                mPlayers.Add(player);
            }
        }

        public int Count
        {
            get { return mPlayers.Count; }
        }

        public Player this[int index]
        {
            get { return mPlayers[index]; }
        }

        public void Unregister(Project project)
        {
            Player player = playerFor(project);
            if(null!=player){
                StartTrigger -= player.Start;
                StopTrigger -= player.Stop;
                mPlayers.Remove(player);
            }
        }

        public void UnregisterAll()
        {
            mPlayers.Clear();
            StartTrigger = null;
            StopTrigger = null;
        }

        public bool IsAvailable
        {
            get { return mPlayers.Count > 0; }
        }

        public void Start()
        {
            if (null != StartTrigger)
            {
                StartTrigger();
            }
        }

        public void Stop()
        {
            if (null != StopTrigger)
            {
                StopTrigger();
            }
        }

        void onTargeted(TargetItem item)
        {
            if (null != Targeted)
            {
                Targeted(item);
            }
        }

        void onClicked(TargetItem item)
        {
            if (null != Clicked)
            {
                Clicked(item);
            }
        }

        void onStart(Project proj)
        {
            mPlayCount++;
            if (mPlayCount==1 && null != PlayStateChanged)
            {
                PlayStateChanged(true);
            }
        }
        void onStop(Project proj)
        {
            mPlayCount--;
            if (mPlayCount < 0)
            {
                mPlayCount = 0;
            }

            if (mPlayCount == 0 && null != PlayStateChanged)
            {
                PlayStateChanged(false);
            }
        }
    }

}
