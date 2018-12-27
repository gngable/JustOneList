using System;
using System.Collections.Generic;
using System.Text;
using JustOneList.Droid;
using Xamarin.Forms;

namespace JustOneList
{
    public static class StaticData
    {
        public static ContentPage CurrentPage { get; set; }
        public static IJ1LClipboard Clipboard { get; set; }
        public static Action<string> Toast { get; set; }
    }
}
