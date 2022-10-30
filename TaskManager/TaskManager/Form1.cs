using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using NLog;
using System.Linq;

namespace TaskManager
{
    public partial class Form1 : Form
    {
        public static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        System.Timers.Timer _refreshTimer;
        string[] oldList = null;
        public Form1()
        {
            InitializeComponent();
        }
        public void InitializeRefreshTimer()
        {
            _refreshTimer = new System.Timers.Timer(1000);

            _refreshTimer.SynchronizingObject = this;

            _refreshTimer.Elapsed += new System.Timers.ElapsedEventHandler(TimerToUpdate_List);

            _refreshTimer.Start();
        }
        void TimerToUpdate_List(object sender, System.Timers.ElapsedEventArgs e)
        {
            myProcessClass processes = new myProcessClass();
            Invoke(new Action(() =>
            {
                var index = listBox1.TopIndex;
                int selectedIndex = listBox1.SelectedIndex;
                string[] list = processes.getProcess("list");
                listBox1.Items.Clear();
                listBox1.Items.AddRange(list);
                listBox1.TopIndex = index;
                listBox1.Sorted = true;
                if (selectedIndex >= 0)
                    try
                    {
                        listBox1.SelectedIndex = selectedIndex;
                    }
                    catch
                    {
                        logger.Error("Ошибка индекса обновления списка процессов");
                    }

                //Write new and stop processes
                if (oldList != null)
                {
                    var processStop = oldList.Except(list);
                    foreach (string str in processStop)
                        logger.Info($"Процесс остановлен : {str}");

                    var processNew = list.Except(oldList);
                    foreach (string str in processNew)
                        logger.Info($"Процесс запущен : {str}");
                }
                oldList = list;


            }));
            label1.Text = Process.GetProcesses().Length.ToString();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            InitializeRefreshTimer();
            logger.Info("Запуск программы");
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int indexProc = listBox1.SelectedIndex;
            string nameProc = listBox1.SelectedItem.ToString();
            FormInfo frm = new FormInfo(indexProc, nameProc);
            frm.Show();
            logger.Info($"Запрос на отображение дополнительной информации о процессе : {nameProc}");
        }

    }
}
