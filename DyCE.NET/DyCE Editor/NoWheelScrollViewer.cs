using System.Windows.Controls;
using System.Windows.Input;

namespace DyCE.Editor
{
    public class NoWheelScrollViewer : ScrollViewer
    {
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            // Do nothing
        }
    }
}
