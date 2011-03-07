using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Timers;
using System.Windows.Threading;

namespace DynamicContent
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			this.InitializeComponent();
            wait.Elapsed += new ElapsedEventHandler(wait_Elapsed);
            wait.AutoReset = false;
		}

        Timer wait = new Timer(500);
		private void btn_NewEngine_Click(object sender, RoutedEventArgs e)
		{
            wait.Start();
		}

        void wait_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke((Action)(() => txt_EngineName.Focus()));
        }
	}
}