using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DyCE;

namespace DyCE_Sandbox
{
	/// <summary>
	/// Interaction logic for EngineSelectionWindow.xaml
	/// </summary>
	public partial class EngineSelectionWindow : Window
	{

        private EngineBase _baseEngine { get; set; }

		public EngineSelectionWindow()
		{
			this.InitializeComponent();
			// Insert code required on object creation below this point.
		}

        public EngineSelectionWindow(EngineBase engine):this()
        {
            _baseEngine = engine;
            Loaded += EngineSelectionWindow_Loaded;
        }

        void EngineSelectionWindow_Loaded(object sender, RoutedEventArgs e)
        {
            cmb_DyCEBags.SelectionChanged += DyCEBagChanged;
            UpdateAvailableEngineList();
        }

	    private void DyCEBagChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs) { UpdateAvailableEngineList(); }
	    private void UpdateAvailableEngineList()
	    {
	        var item = cmb_DyCEBags.SelectedItem as DyCEBag;

	        if (item == null)
	            return;

	        lst_Engines.ItemsSource = item.DyCEList.Where(ValidEngine).ToList();
	    }

	    private bool ValidEngine(object o)
	    {
            var engine = o as EngineBase;
	        return engine != null && !engine.Has(_baseEngine);
	    }

	    private void btn_AddEngine(object sender, System.Windows.RoutedEventArgs e)
	    {
	        var engine = _baseEngine as EngineBase;

	        if (engine == null || lst_Engines.SelectedItem == null)
	            return;

	        engine.Add(lst_Engines.SelectedItem);
	        DialogResult = true;
	        Close();
	    }
	}
}