using System.Windows;
using GeneratorSQL.ViewModels;

namespace GeneratorSQL.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
            
            // Initialize language button styling
            UpdateLanguageButtons("en");
        }

        private void OnLanguageClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var border = sender as System.Windows.Controls.Border;
            if (border != null && DataContext is MainViewModel viewModel)
            {
                var locale = border.Tag as string;
                if (!string.IsNullOrEmpty(locale))
                {
                    viewModel.CurrentLocale = locale;
                    UpdateLanguageButtons(locale);
                }
            }
        }

        private void UpdateLanguageButtons(string currentLocale)
        {
            var selectedBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(33, 150, 243));
            var transparentBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Transparent);
            var whiteBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White);
            var grayBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(100, 100, 100));

            if (currentLocale == "en")
            {
                EnButton.Background = selectedBrush;
                EnText.Foreground = whiteBrush;
                EnText.FontWeight = System.Windows.FontWeights.Bold;
                
                UaButton.Background = transparentBrush;
                UaText.Foreground = grayBrush;
                UaText.FontWeight = System.Windows.FontWeights.Normal;
            }
            else
            {
                UaButton.Background = selectedBrush;
                UaText.Foreground = whiteBrush;
                UaText.FontWeight = System.Windows.FontWeights.Bold;
                
                EnButton.Background = transparentBrush;
                EnText.Foreground = grayBrush;
                EnText.FontWeight = System.Windows.FontWeights.Normal;
            }
        }
    }
}
