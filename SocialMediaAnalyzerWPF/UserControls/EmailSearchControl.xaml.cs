using System.Windows;
using System.Windows.Controls;

namespace SocialMediaAnalyzerWPF.UserControls
{
    /// <summary>
    /// Interaction logic for EmailSearchControl.xaml
    /// </summary>
    public partial class EmailSearchControl : UserControl
    {
        public static readonly RoutedEvent BackButtonClickedEvent = EventManager.RegisterRoutedEvent(
            "BackButtonClicked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(EmailSearchControl));

        public event RoutedEventHandler BackButtonClicked
        {
            add { AddHandler(BackButtonClickedEvent, value); }
            remove { RemoveHandler(BackButtonClickedEvent, value); }
        }

        public EmailSearchControl()
        {
            InitializeComponent();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(BackButtonClickedEvent));
        }
        
        public void Cleanup()
        {
        }
    }
}