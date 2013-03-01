using System;
using System.Collections.Specialized;
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

            var prosperityList = new EngineList("Prosperity Options");
            prosperityList.AddItems("Dirt", "Poor", "Moderate", "Wealthy", "Rich");
            var prosperityProperty = new EngineProperty("Prosperity", prosperityList);

            var populationList = new EngineList("Population Options");
            populationList.AddItems("Exodus", "Shrinking", "Steady", "Growing", "Booming");
            var populationProperty = new EngineProperty("Population", populationList);

            var defenseList = new EngineList("Defense Options");
            defenseList.AddItems("None", "Militia", "Watch", "Guard", "Garrison", "Battalion", "Legion");
            var defenseProperty = new EngineProperty("Defenses", defenseList);

            var steadingEngine = new EngineObject("Steading", prosperityProperty, populationProperty, defenseProperty);

            ViewModel.Bag.DyCEList.Add(steadingEngine);
            ViewModel.Bag.DyCEList.Add(defenseList);
            ViewModel.Bag.DyCEList.Add(populationList);
            ViewModel.Bag.DyCEList.Add(prosperityList);
            defenseList.Items.ToList().ForEach(i => ViewModel.Bag.DyCEList.Add(i));
            populationList.Items.ToList().ForEach(i => ViewModel.Bag.DyCEList.Add(i));
            prosperityList.Items.ToList().ForEach(i => ViewModel.Bag.DyCEList.Add(i));

            var test = steadingEngine.Go(55) as ResultObject;

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

        private void txt_SelectAll(object sender, System.Windows.RoutedEventArgs e)
        {
        	var control = sender as TextBox;
			control.SelectAll();
        }
    }
}
