using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using DyCE;

namespace DyCETester
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		public MainWindow()
		{
			InitializeComponent();
            
			// Insert code required on object creation below this point.
            VM.Instance.Results.CollectionChanged += Results_CollectionChanged;
		}

        void Results_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var border = System.Windows.Media.VisualTreeHelper.GetChild(lst_Results, 0) as Decorator;
            if (border == null) return;
            var scrollViewer = border.Child as ScrollViewer;
            if (scrollViewer != null) scrollViewer.ScrollToBottom();
        }

		private void tree_Engines_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
		    var selectedEngine = e.NewValue as EngineBase;

		    if (selectedEngine != null) 
                VM.Instance.SelectedEngine = selectedEngine;
		}
	}
}