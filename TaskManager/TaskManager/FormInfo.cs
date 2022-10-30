using System;
using System.Windows.Forms;
using NLog;

namespace TaskManager
{
    public partial class FormInfo : Form
    {
        public static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        System.Timers.Timer _refreshTimerInform;
        bool _buttonClick = true;
        int _indexProc;
        string _nameProc;
        public FormInfo(int indexProc, string nameProc)
        {
            InitializeComponent();
            InitializeRefreshTimer_Information();
            _indexProc = indexProc;
            _nameProc = nameProc;
        }

        void TimerToUpdate_Information(object sender, System.Timers.ElapsedEventArgs e)
        {
            listBox2.Items.Clear();
            myProcessClass processClass = new myProcessClass();
            listBox2.Items.AddRange(processClass.getProcess("process"));
            listBox2.Sorted = true;
            for (int i = listBox2.Items.Count - 1; i >= 0; i--)
            {
                if (i != _indexProc)
                {
                    listBox2.Items.RemoveAt(i);
                }
            }
        }
        public void InitializeRefreshTimer_Information()
        {
            _refreshTimerInform = new System.Timers.Timer(1000);

            _refreshTimerInform.SynchronizingObject = this;

            _refreshTimerInform.Elapsed += new System.Timers.ElapsedEventHandler(TimerToUpdate_Information);

            _refreshTimerInform.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_buttonClick == false)
            {
                _refreshTimerInform.Start();
                _buttonClick = true;
                logger.Info($"Запуск обновления процесса : {_nameProc}");
            }
            else
            {
                _refreshTimerInform.Stop();
                _buttonClick = false;
                logger.Info($"Остановка обновления процесса : {_nameProc}");
            }
        }
    }
}
