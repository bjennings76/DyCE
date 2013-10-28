using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using DyCE;
using mshtml;

namespace DyCE.Editor
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

            var test = DB.Instance["General"]["Steading"].Go(55) as ResultObject;

            string result = test.Engine.Name + ":\r\n";

            foreach (var property in test.Properties)
                result += "    " + test[property.Engine.ID] + "\r\n";

            Console.WriteLine(result);

            // Can we create an 'options' object that can be passed down to promote/restrict specific options?
            // Can we create a top-level 'template' of options for a Village, Town, Keep, and City?

            _vm.Results.CollectionChanged += Results_CollectionChanged;
            _vm.SubscribeToChange(() => _vm.SelectedEngine, SelectedEngineChanged);
            web_Results.LoadCompleted += web_Results_LoadCompleted;
        }

        void Items_CurrentChanged(object sender, EventArgs e)
        {
            if (!_vm.Results.Any())
                return;

            var tvi = tree_Results.ItemContainerGenerator.ContainerFromItem(_vm.Results.Last()) as TreeViewItem;
            if (tvi != null)
                tvi.BringIntoView();
        }

        void web_Results_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            var htmlDocument = web_Results.Document as HTMLDocument;
            if (htmlDocument != null)
                htmlDocument.parentWindow.scroll(0, int.MaxValue);
        }
        private void SelectedEngineChanged(ViewModel sender)
        {
            if (sender.SelectedEngine == null)
                return;

            ExpandToSelected();

            //control_EngineEditor.Focus();
        }

        private void ExpandToSelected()
        {
            TreeViewItem node = tree_Engines.ItemContainerGenerator.ContainerFromItem(tree_Engines.SelectedItem) as TreeViewItem;

            if (node == null)
            {
                Console.WriteLine("Could not convert the selected item into a tree view item.");
                return;
            }

            node = node.Parent as TreeViewItem;

            while (node != null)
            {
                node.IsExpanded = true;
                node = node.Parent as TreeViewItem;
            } 
        }

        private static bool SelectItem(object o, TreeViewItem parentItem)
        {
            if (parentItem == null)
                return false;

            bool isExpanded = parentItem.IsExpanded;
            if (!isExpanded)
            {
                parentItem.IsExpanded = true;
                parentItem.UpdateLayout();
            }

            TreeViewItem item = parentItem.ItemContainerGenerator.ContainerFromItem(o) as TreeViewItem;
            if (item != null)
            {
                item.IsSelected = true;
                return true;
            }

            bool wasFound = false;
            for (int i = 0; i < parentItem.Items.Count; i++)
            {
                TreeViewItem itm = parentItem.ItemContainerGenerator.ContainerFromIndex(i) as TreeViewItem;
                var found = SelectItem(o, itm);
                if (!found && itm != null)
                    itm.IsExpanded = false;
                else
                    wasFound = true;
            }

            return wasFound;
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

        private void web_Results_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            if (e.Uri == null) 
                return;

            var queries = HttpUtility.ParseQueryString(e.Uri.Query);

            int seed;
            if (!int.TryParse(queries["seed"], out seed))
                return;

            var creator = queries["creator"];
            var bag = queries["bag"];
            var engine = queries["engine"];
            
            if (creator != null && bag != null && engine != null)
            {
                var bagObj = DyCE.DB.Instance.DyCEBags.FirstOrDefault(d => d.Name == bag);

                if (bagObj == null)
                    return;

                var engineObj = bagObj.DyCEList.FirstOrDefault(dyce => dyce.ID == engine);

                if (engineObj == null)
                    return;

                _vm.SelectedEngine = engineObj;
                _vm.Results.Clear();
                _vm.Results.Add(engineObj.Go(seed));
                _vm.Paused = true;
                e.Cancel = true;
            }
        }
    }
}
