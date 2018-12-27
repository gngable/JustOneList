using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace JustOneList
{
    public partial class MainPage : ContentPage
    {
        public MainPageViewModel MainPageViewModel { get; } = new MainPageViewModel();

        public MainPage()
        {
            InitializeComponent();

            BindingContext = MainPageViewModel;

            MainPageViewModel.PropertyChanged += MainPageViewModel_PropertyChanged;

            StaticData.CurrentPage = this;
        }

        private void MainPageViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var last = ListView.TemplatedItems.Last();

            if (last == null) return;

            var layout = (last as Cell).LogicalChildren.LastOrDefault();

            if (layout == null) return;

            var entry = layout.LogicalChildren.LastOrDefault();

            if (entry == null) return;

            (entry as Entry).Focus();
        }
    }
}
