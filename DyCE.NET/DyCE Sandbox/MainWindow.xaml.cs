using System;
using System.Windows;
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

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var prosperityList = new EngineList("Properities");
            prosperityList.AddItems("Dirt", "Poor", "Moderate", "Wealthy", "Rich");
            var prosperityProperty = new EngineProperty("Prosperity", prosperityList);

            var populationList = new EngineList("Populations");
            populationList.AddItems("Exodus", "Shrinking", "Steady", "Growing", "Booming");
            var populationProperty = new EngineProperty("Population", populationList);

            var defenseList = new EngineList("Defenses");
            defenseList.AddItems("None", "Militia", "Watch", "Guard", "Garrison", "Battalion", "Legion");
            var defenseProperty = new EngineProperty("Defenses", defenseList);

            var steadingEngine = new EngineObject("Steading", prosperityProperty, populationProperty, defenseProperty);

            DyCEBag.Instance.DyCEList.Add(steadingEngine);
            DyCEBag.Instance.DyCEList.Add(defenseList);
            DyCEBag.Instance.DyCEList.Add(populationList);
            DyCEBag.Instance.DyCEList.Add(prosperityList);


            var test = steadingEngine.Go(55) as ResultObject;

            string result = test.Name + ":\r\n";

            foreach (var property in test.Properties)
                result += "    " + property.Name + ": " + test[property.Name] + "\r\n";

            Console.WriteLine(result);

            // Can we create an 'options' object that can be passed down to promote/restrict specific options?
            // Can we create a top-level 'template' of options for a Village, Town, Keep, and City?
        }
    }
}
