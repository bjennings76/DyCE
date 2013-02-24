using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace DyCE
{
    public static class Utilities
    {
        //public static void SyncToList(this ObservableCollection<ResultText> collection, IEnumerable<string> list)
        //{
        //    string[] listArray = list.ToArray();

        //    if (listArray.Length == 0)
        //        collection.Clear();

        //    for (int i = 0; i < listArray.Length; i++)
        //    {
        //        // At the end of the collection, so add the new items.
        //        if (collection.Count <= i)
        //        {
        //            collection.Add(new ResultText(null, listArray[i]));
        //            continue;
        //        }

        //        int compare = String.Compare(listArray[i], collection[i].Text, StringComparison.Ordinal);

        //        // Item names are the same, so continue.
        //        if (compare == 0)
        //            continue;

        //        // Remove all old items that are higher alphabetically.
        //        if (compare > 0)
        //        {
        //            while (compare > 0)
        //            {
        //                collection.RemoveAt(i);

        //                if (collection.Count <= i)
        //                {
        //                    collection.Add(new ResultText(null, listArray[i]));
        //                    break;
        //                }

        //                compare = String.Compare(listArray[i], collection[i].Text, StringComparison.Ordinal);
        //            }
        //        }

        //        // Item is lower alphabetially, so insert it.
        //        if (compare < 0)
        //            collection.Insert(i, new ResultText(null, listArray[i]));
        //    }

        //    // If collection is stil longer, remove extra items.
        //    while (collection.Count > listArray.Length)
        //        collection.RemoveAt(listArray.Length);
        //}

    }
}
