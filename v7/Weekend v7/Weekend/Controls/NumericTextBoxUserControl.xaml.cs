using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace Weekend.Controls
{
    public partial class NumericTextBoxUserControl : UserControl
    {
        // allowed keys
        private readonly Key[] numeric = new Key[] {Key.Back, Key.NumPad0, Key.NumPad1, Key.NumPad2, Key.NumPad3, Key.NumPad4,
                                    Key.NumPad5, Key.NumPad6, Key.NumPad7, Key.NumPad8, Key.NumPad9 };
        
        public NumericTextBoxUserControl()
        {
            InitializeComponent();
        }

        private void NumericTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            // handles non numeric
            if (Array.IndexOf(numeric, e.Key) == -1)
            {
                e.Handled = true;
            }
        }

        public string Text
        {
            get 
            {
                return NumericTextBox.Text; 
            }
            set 
            { 
                NumericTextBox.Text = value; 
            }
        }
    }
}
