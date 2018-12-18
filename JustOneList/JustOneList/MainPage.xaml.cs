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
        }
    }
}
