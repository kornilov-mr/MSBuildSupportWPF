using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace MSBuildSupportWPF.UI.UIComponents
{
    public class ErrorPopup : Popup
    {
        Border border = new Border
        {
            Background = Brushes.LightYellow,
            BorderBrush = Brushes.Black,
            BorderThickness = new Thickness(1),
            Padding = new Thickness(5)
        };
        public ErrorPopup() {
            Child = border;
        }
        
        public void LoadError(Exception error)
        {
            TextBlock popupText = new TextBlock
            {
                Text = error.Message,
                TextWrapping = TextWrapping.Wrap
            };
            border.Child = popupText;
        }
    }
}
