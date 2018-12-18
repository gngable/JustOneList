using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustOneList
{
    public class MainPageViewModel
    {
        public ObservableCollection<ListItem> UncheckedList { get; } = new ObservableCollection<ListItem>();
        public ObservableCollection<ListItem> CheckedList { get; } = new ObservableCollection<ListItem>();

        public MainPageViewModel()
        {
            UncheckedList.Add(new ListItem{Label = "One"});
            UncheckedList.Add(new ListItem { Label = "Two" });
            UncheckedList.Add(new ListItem { Label = "Three" });
            UncheckedList.Add(new ListItem());

            UncheckedList.CollectionChanged += UncheckedList_CollectionChanged;
            CheckedList.CollectionChanged += CheckedList_CollectionChanged;

            AddListeners();
        }

        private void CheckedList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            AddListeners();
        }

        private void UncheckedList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            AddListeners();
        }

        private void AddListeners()
        {
            foreach (var listItem in UncheckedList)
            {
                listItem.PropertyChanged -= UncheckedListItemOnPropertyChanged;
                listItem.PropertyChanged += UncheckedListItemOnPropertyChanged;
            }

            foreach (var listItem in CheckedList)
            {
                listItem.PropertyChanged -= CheckedListItemPropertyChanged;
                listItem.PropertyChanged += CheckedListItemPropertyChanged;
            }
        }

        private void CheckedListItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsChecked")
            {
                var item = sender as ListItem;

                Task.Run(() =>
                {
                    try
                    {
                        if (item.IsChecked)
                        {
                            item.PropertyChanged -= CheckedListItemPropertyChanged;
                            CheckedList.Remove(item);
                            UncheckedList.Add(item);
                        }
                    }
                    catch (Exception ex)
                    {
                        
                    }
                });
            }
        }

        private void UncheckedListItemOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (UncheckedList.All(i => !string.IsNullOrWhiteSpace(i.Label)))
            {
                UncheckedList.Add(new ListItem());
            }

            if (e.PropertyName == "IsChecked")
            {
                var item = sender as ListItem;

                Task.Run(() =>
                {
                    try
                    {
                        if (item.IsChecked)
                        {
                            item.PropertyChanged -= UncheckedListItemOnPropertyChanged;
                            UncheckedList.Remove(item);
                            CheckedList.Add(item);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                });
            }
        }
    }
}
