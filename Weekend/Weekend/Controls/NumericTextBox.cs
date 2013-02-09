using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace Weekend.Controls
{
    public class NumericTextBox : TextBox
    {
        private readonly Key[] numeric = new Key[] {Key.Back, Key.NumPad0, Key.NumPad1, Key.NumPad2, Key.NumPad3, Key.NumPad4,
                                    Key.NumPad5, Key.NumPad6, Key.NumPad7, Key.NumPad8, Key.NumPad9 };
        
        public NumericTextBox()
        {
            this.InputScope = new InputScope();
            this.InputScope.Names.Add(new InputScopeName() { NameValue = InputScopeNameValue.TelephoneLocalNumber });
            this.MaxLength = 1;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {  
            if(Array.IndexOf(numeric,e.Key) == -1)
            {
                e.Handled = true;
            }
            base.OnKeyDown(e); // important, if not called the back button is not handled
        }
    }

}
