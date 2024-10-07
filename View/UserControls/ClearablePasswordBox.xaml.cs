using System.Windows;
using System.Windows.Controls;

namespace Depo_Stok_Kontrol.View.UserControls
{
    public partial class ClearablePasswordBox : UserControl
    {
        public ClearablePasswordBox()
        {
            InitializeComponent();
        }
        private string placeholder;

        public string Placeholder
        {
            get { return placeholder; }
            set
            {
                placeholder = value;
                pwPlaceholder.Text = placeholder;
            }
        }

        public static readonly DependencyProperty MaxLengthProperty =
            DependencyProperty.Register(
                nameof(MaxLength),
                typeof(int),
                typeof(ClearablePasswordBox),
                new PropertyMetadata(int.MaxValue, OnMaxLengthChanged));

        public int MaxLength
        {
            get { return (int)GetValue(MaxLengthProperty); }
            set { SetValue(MaxLengthProperty, value); }
        }

        private static void OnMaxLengthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (ClearablePasswordBox)d;
            control.pwInput.MaxLength = (int)e.NewValue;
        }

        private void pwInput_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(pwInput.Password))
                pwPlaceholder.Visibility = Visibility.Visible;
            else
                pwPlaceholder.Visibility = Visibility.Hidden;

        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            pwInput.Clear();
            pwInput.Focus();
        }
    }
}
