namespace DyCE_Sandbox
{
	/// <summary>
	/// Interaction logic for NewDyCEBagWindow.xaml
	/// </summary>
	public partial class NewDyCEBagWindow
	{

	    public NewDyCEBagWindow()
		{
			InitializeComponent();
            Loaded += NewDyCEBagWindow_Loaded;
		}

        void NewDyCEBagWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            txt_BagName.Focus();
            txt_BagName.SelectAll();
        }

	    private string _result = "New DyCE Bag";
        public string Result { get { return _result; } set { _result = value; } }

        private void btn_OK(object sender, System.Windows.RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_BagName.Text))
                return;

            Result = txt_BagName.Text;
            DialogResult = true;
            Close();
        }
	}
}