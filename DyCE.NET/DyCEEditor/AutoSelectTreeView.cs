using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace DynamicContent
{
    public class AutoSelectTreeView : TreeView
    {
        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);

            if (this.Items.Count > 0 && e.NewItems.Count > 0)
            {
                TreeViewItem item = (TreeViewItem)this.ItemContainerGenerator.ContainerFromItem(e.NewItems[0]);
                item.IsSelected = true;
            }
        }
    }
}
