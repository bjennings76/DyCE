using System.Windows;
using System.Windows.Controls;

namespace DyCE.Editor
{
    public class BrowserBehavior
    {
        public static readonly DependencyProperty HtmlProperty = DependencyProperty.RegisterAttached(
                "Html",
                typeof(string),
                typeof(BrowserBehavior),
                new FrameworkPropertyMetadata(OnHtmlChanged));

        [AttachedPropertyBrowsableForType(typeof(WebBrowser))]
        public static string GetHtml(WebBrowser d)
        {
            return (string)d.GetValue(HtmlProperty);
        }

        public static void SetHtml(WebBrowser d, string value)
        {
            d.SetValue(HtmlProperty, value);
        }

        static void OnHtmlChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var webBrowser = dependencyObject as WebBrowser;
            var html = e.NewValue as string;

            if (webBrowser == null || string.IsNullOrWhiteSpace(html)) 
                return;

            var doc = (mshtml.HTMLDocument) webBrowser.Document;

            if (doc == null)
            {
                webBrowser.NavigateToString(GetFullHtml(html));
                return;
            }

            var div = doc.getElementById("Inner");
            var bottomDiv = doc.getElementById("Bottom");

            if (div == null)
            {
                webBrowser.NavigateToString(GetFullHtml(html));
                return;
            }

            div.innerHTML = html;

            if (bottomDiv != null)
                bottomDiv.scrollIntoView();
        }

        private static string GetFullHtml(string innerHtml)
        {
            const string prefix = @"<html>
    <body style=""font-family:segoe ui;font-size:12px;"">
        <div id=""Inner"">";

            const string postfix = @"        </div>
        <div id=""Bottom""/>
    </body>
<html>";

            return prefix + innerHtml + postfix;

        }
    }
}
