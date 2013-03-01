using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;
using GalaSoft.MvvmLight;

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

        public static T LoadFromXML<T>(FileInfo file) where T : new()
        {
            if (!file.Exists)
                return new T();

            Exception exception = null;
            int tries = 0;

            while (tries < 50)
            {
                try
                {
                    using (var myFileStream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
                    {
                        var mySerializer = new XmlSerializer(typeof(T));
                        return (T)mySerializer.Deserialize(myFileStream);
                    }
                }
                catch (InvalidOperationException ex)
                {
                    throw new ApplicationException("Could not load '" + file.FullName + "'", ex);
                }
                catch (Exception ex)
                {
                    exception = ex;
                    Thread.Sleep(100);
                    tries++;
                }
            }

            throw new ApplicationException("Could not load '" + file.FullName + "'", exception);
        }

        public static void SaveToXML(FileInfo file, object data)
        {
            try
            {
                string fileXML;

                using (var stringWriter = new StringWriterWithEncoding(new StringBuilder(), Encoding.UTF8))
                {
                    var ns = new XmlSerializerNamespaces();
//                    ns.Add("", "");
                    var x = new XmlSerializer(data.GetType());
                    x.Serialize(stringWriter, data, ns);
                    fileXML = Regex.Replace(stringWriter.ToString(), @"\r\n?|\n", "\r\n");
                }

                if (file.Exists)
                {
                    string baseXML = File.ReadAllText(file.FullName).Trim();
                    if (fileXML.Trim() == baseXML) return;
                }

                File.WriteAllText(file.FullName, fileXML);
            }
            catch (Exception ex)
            {
                ShowExceptionBox("Could not save '" + file.FullName + "'", ex);
            }
        }

        public static void ShowExceptionBox(string message, Exception ex, bool thisIsBad = false)
        {
                if (thisIsBad)
                    message = "Doh! " + message + "\n\nPlease send a copy of this dialog box to: bjennings@maxis.com";
                var appEx = new ApplicationException(message, ex);
                ShowMessageBox(appEx);
        }

        private static void ShowMessageBox(Exception appEx)
        {
            string message = appEx.Message + "\n";

            while (appEx.InnerException != null)
            {
                appEx = appEx.InnerException;
                message += "\nBecause: " + appEx.Message;
            }

            if (!ViewModelBase.IsInDesignModeStatic)
                MessageBox.Show(message, "Weird Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
        }

    }
}
