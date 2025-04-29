using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Automate.ViewModels;

namespace Automate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainVM ViewModel {  get; set; }
        public MainWindow()
        {
            MainVM viewModel = new();
            this.DataContext = viewModel;
            ViewModel = viewModel;
            InitializeComponent();
        }

        private void MainBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) { DragMove(); }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void AButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Input += 'A';
        }

        private void BButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Input += 'B';
        }

        private void CButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Input += 'C';
        }

        private void DButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Input += 'D';
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Input = null;
        }
    }
}