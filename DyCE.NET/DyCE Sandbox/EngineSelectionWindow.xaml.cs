using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DyCE;

namespace DyCE_Sandbox
{
	/// <summary>
	/// Interaction logic for EngineSelectionWindow.xaml
	/// </summary>
	public partial class EngineSelectionWindow
	{
        private EngineBase _baseEngine { get; set; }

	    public EngineSelectionWindow() { InitializeComponent(); }

	    public EngineSelectionWindow(EngineBase engine)
	        : this()
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

	    private void btn_AddEngine(object sender, RoutedEventArgs e)
	    {
	        DialogResult = AddItem(lst_Engines.SelectedItem);
	        Close();
	    }

	    private bool AddItem(object selectedItem)
	    {
            if (_baseEngine == null || lst_Engines.SelectedItem == null)
                return false;

            // Handle list engine case.
            var listEngine = _baseEngine as EngineList;
            if (listEngine != null)
            {
                listEngine.Add(lst_Engines.SelectedItem);
                return true;
            }

            // Handle object engine case.
            var objectEngine = _baseEngine as EngineObject;
            if (objectEngine != null)
            {
                objectEngine.Add(lst_Engines.SelectedItem);
                return true;
            }

	        return false;
	    }
	}
}