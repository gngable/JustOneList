using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace JustOneList.Droid
{
    public class J1LClipboard : IJ1LClipboard
    {
        private ClipboardManager _clipboardManager;

        public J1LClipboard(ClipboardManager clipboardManager)
        {
            _clipboardManager = clipboardManager;
        }

        public void Copy(string text)
        {
            var clip = ClipData.NewPlainText("J1L", text);

            _clipboardManager.PrimaryClip = clip;
        }

        public string Paste()
        {
            if (_clipboardManager.HasPrimaryClip)
            {
                return _clipboardManager.PrimaryClip.GetItemAt(0).Text;
            }

            return null;
        }
    }
}