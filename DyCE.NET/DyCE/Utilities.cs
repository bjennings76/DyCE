using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;
using DyCE.Annotations;
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

        public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach (T item in list)
                action(item);
        }

        public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> list, int num)
        {
            IEnumerable<T> enumerable = list as T[] ?? list.ToArray();
            return enumerable.Skip(Math.Max(0, enumerable.Count() - num)).Take(num);
        }

        public static string JoinToString<T>(this IEnumerable<T> list, string separator = null)
        {
            var array = list as T[] ?? list.ToArray();
            return array.Any() ? string.Join(separator, array.Select(i => i == null ? "" : i.ToString()).ToArray()) : null;
        }

        /// <summary>
        /// Returns true if the <paramref name="text"/> string contans the <paramref name="search"/> string using the supplied <paramref name="comparisonType"/>.
        /// </summary>
        /// <param name="text">The string to test for the check string.</param>
        /// <param name="search">The check string.</param>
        /// <param name="comparisonType">The comparison method to use which allows for IgnoreCase methods.</param>
        /// <returns>True if the search string is found in the given text.</returns>
        public static bool Contains(this string text, string search, StringComparison comparisonType)
        {
            if (search.IsNullOrEmpty())
                return true;

            if (text.IsNullOrEmpty())
                return false;

            return text.IndexOf(search, comparisonType) >= 0;
        }

        /// <summary>
        /// Returns true if the strings is null or an empty string.
        /// </summary>
        /// <param name="source">The string to test.</param>
        /// <returns>True if the string is null or empty, otherwise false.</returns>
        [ContractAnnotation("source:null => true")]
        public static bool IsNullOrEmpty(this string source)
        {
            return String.IsNullOrEmpty(source);
        }

        public static string Repeat(this char charToRepeat, int repeat)
        {
            return new string(charToRepeat, repeat);
        }
        public static string Repeat(this string stringToRepeat, int repeat)
        {
            var builder = new StringBuilder(repeat * stringToRepeat.Length);
            for (int i = 0; i < repeat; i++)
            {
                builder.Append(stringToRepeat);
            }
            return builder.ToString();
        }

        /// <summary>
        /// A convenience extension version of Regex.Replace.
        /// </summary>
        /// <param name="input">The string to search for a match.</param>
        /// <param name="pattern">The regular expression pattern to match.</param>
        /// <param name="replacement">The replacement string.</param>
        /// <param name="options">A bitwise combination of the enumeration values that provide options for matching.</param>
        /// <returns>A new string that is identical to the input string, except that the replacement string takes the place of each matched string.</returns>
        public static string ReplaceRegex(this string input, string pattern, string replacement, RegexOptions options = RegexOptions.None)
        {
            return Regex.Replace(input, pattern, replacement, options);
        }

        /// <summary>
        /// Replaces a string's old value with a new value using the string comparison type.
        /// </summary>
        /// <param name="originalString">The string to run the search/replace on.</param>
        /// <param name="oldValue">The old value to find.</param>
        /// <param name="newValue">The new value to replace.</param>
        /// <param name="comparisonType">The type of comparison to use.</param>
        /// <returns></returns>
        public static string Replace(this string originalString, string oldValue, string newValue, StringComparison comparisonType)
        {
            int startIndex = 0;
            while (true)
            {
                startIndex = originalString.IndexOf(oldValue, startIndex, comparisonType);
                if (startIndex == -1)
                    break;

                originalString = originalString.Substring(0, startIndex) + newValue + originalString.Substring(startIndex + oldValue.Length);

                startIndex += newValue.Length;
            }

            return originalString;
        }

        /// <summary>
        /// Parses out a string (e.g. file name or camel case ID) into a readable name.
        /// </summary>
        /// <param name="text">The text to convert.</param>
        /// <param name="capitalize">If true, will capitalize the first character of words.</param>
        /// <param name="removeNumbers">If true, will remove numbers.</param>
        /// <returns>The converted human-readable string.</returns>
        public static string ToSpacedName(this string text, bool capitalize = true, bool removeNumbers = true)
        {
            text = text.ReplaceRegex(@"[A-Z][a-z]", " $0").ReplaceRegex(@"([0-9])([A-Za-z])|([A-Za-z])([0-9])", "$1$3 $2$4");

            if (removeNumbers)
            {
                string test = text.Split(new[] { ' ', '\t', '.', '_', '#', '$', '%', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' }, StringSplitOptions.RemoveEmptyEntries).Where(s => s.Length > 1).ToArray().JoinToString(" ");

                if (test.Length > 0)
                    text = test;
            }
            else
            {
                text = text.Split(new[] { ' ', '\t', '.', '_', '#', '$', '%' }, StringSplitOptions.RemoveEmptyEntries).Where(s => s.Length > 1 || (s.Length == 1 && char.IsDigit(s[0]))).ToArray().JoinToString(" ");
            }

            return capitalize ? text.ToCapitalized().Trim() : text.Trim();
        }

        /// <summary>
        /// Capitalizes the first letter of each word in the supplied string.
        /// </summary>
        /// <param name="text">The text to capitalize.</param>
        /// <returns>The text with capitalized first characters.</returns>
        public static string ToCapitalized(this string text)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text);
        }

    }

}
