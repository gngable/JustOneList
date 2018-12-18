using System;
using System.Collections.Generic;
using System.Text;

namespace JustOneList
{
    public class MainPageViewModel
    {
        public List<ListItem> TheList { get; } = new List<ListItem>();

        public MainPageViewModel()
        {
            TheList.Add(new ListItem{Label = "One"});
            TheList.Add(new ListItem { Label = "Two" });
            TheList.Add(new ListItem { Label = "Three" });
        }
    }
}
