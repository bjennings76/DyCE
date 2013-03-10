using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DyCE;

namespace DyCE_Sandbox
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;
        }

        private ViewModel _vm;

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Get the 'ViewModel' resource
            _vm = (ViewModel)Application.Current.Resources["ViewModelDataSource"];

            //var prosperityList = new EngineList("Prosperity Options", new object[]{"Dirt", "Poor", "Moderate", "Wealthy", "Rich"});
            //var prosperityProperty = new EngineProperty("Prosperity", prosperityList);

            //var populationList = new EngineList("Population Options", new[]{"Exodus", "Shrinking", "Steady", "Growing", "Booming"});
            //var populationProperty = new EngineProperty("Population", populationList);

            //var defenseList = new EngineList(new[]{"None", "Militia", "Watch", "Guard", "Garrison", "Battalion", "Legion"});
            //var defenseProperty = new EngineProperty("Defenses", defenseList);

            //var steadingEngine = new EngineObject("Steading", prosperityProperty, populationProperty, defenseProperty);

            //ViewModel.Bag.DyCEList.Add(steadingEngine);
            ////ViewModel.Bag.DyCEList.Add(defenseList);
            //ViewModel.Bag.DyCEList.Add(populationList);
            //ViewModel.Bag.DyCEList.Add(prosperityList);

            //_vm.Bag = DyCEBag.Load(new FileInfo(@"Engines\Generic.xml"));

            var test = DB.Instance["General"]["Steading"].Go(55) as ResultObject;

            string result = test.Name + ":\r\n";

            foreach (var property in test.Properties)
                result += "    " + test[property.Name] + "\r\n";

            Console.WriteLine(result);

            // Can we create an 'options' object that can be passed down to promote/restrict specific options?
            // Can we create a top-level 'template' of options for a Village, Town, Keep, and City?

            _vm.Results.CollectionChanged += Results_CollectionChanged;
            _vm.SubscribeToChange(() => _vm.SelectedEngine, SelectedEngineChanged);
        }

        private void SelectedEngineChanged(ViewModel sender)
        {
            //control_EngineEditor.Focus();
        }

        void Results_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var border = System.Windows.Media.VisualTreeHelper.GetChild(tree_Results, 0) as Decorator;
            if (border == null) return;
            var scrollViewer = border.Child as ScrollViewer;
            if (scrollViewer != null) scrollViewer.ScrollToBottom();
        }

        private void txt_SelectAll(object sender, RoutedEventArgs e)
        {
        	var control = sender as TextBox;
			control.SelectAll();
        }

        private void btn_SelectEngine(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;

            if (button == null)
                return;

            var baseEngine = button.DataContext as EngineBase;

            if (baseEngine == null)
                return;

            new EngineSelectionWindow(baseEngine).ShowDialog();
        }

        private void btn_AddDyCEBag(object sender, System.Windows.RoutedEventArgs e)
        {
            var nameDialog = new NewDyCEBagWindow();
            if (nameDialog.ShowDialog() != true)
                return;

            var newBag = new DyCEBag {Name = nameDialog.Result};
            DB.Instance.Add(newBag);
            _vm.Bag = newBag;
        }
    }
}
