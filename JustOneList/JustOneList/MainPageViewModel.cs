using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using JustOneList.Annotations;
using Newtonsoft.Json;
using Xamarin.Forms.PlatformConfiguration;

namespace JustOneList
{
    public class MainPageViewModel
    {
        private Task _checkTask = null;
        private DateTime _lastTypedTime = DateTime.Now;
        public ObservableCollection<ListItem> UncheckedList { get; } = new ObservableCollection<ListItem>();
        public ObservableCollection<ListItem> CheckedList { get; } = new ObservableCollection<ListItem>();

        public ICommand ClearCommand { get; }

        public string CheckedPath => Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "checked.json");
        public string UncheckedPath => Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "unchecked.json");

        public MainPageViewModel()
        {
            Load();

            UncheckedList.CollectionChanged += UncheckedList_CollectionChanged;
            CheckedList.CollectionChanged += CheckedList_CollectionChanged;

            AddListeners();

            ClearCommand = new DelegateCommand(async () =>
            {
                var answer = await ShowDialog("Clear List?", "Are you SURE you want to clear your list??", "Yes, clear it", "No");

                if (answer != "Yes, clear it") return;

                if (File.Exists(UncheckedPath))
                {
                    File.Delete(UncheckedPath);
                }

                if (File.Exists(CheckedPath))
                {
                    File.Delete(CheckedPath);
                }

                ClearLists();
                UncheckedList.Add(new ListItem());
            });
        }

        private void ClearLists()
        {
            try
            {
                foreach (var listItem in UncheckedList)
                {
                    listItem.PropertyChanged -= UncheckedListItemOnPropertyChanged;
                }

                foreach (var listItem in CheckedList)
                {
                    listItem.PropertyChanged -= CheckedListItemPropertyChanged;
                }

                UncheckedList.CollectionChanged -= UncheckedList_CollectionChanged;
                CheckedList.CollectionChanged -= CheckedList_CollectionChanged;
                UncheckedList.Clear();
                CheckedList.Clear();
                UncheckedList.CollectionChanged += UncheckedList_CollectionChanged;
                CheckedList.CollectionChanged += CheckedList_CollectionChanged;
                AddListeners();
            }
            catch (Exception ex)
            {

            }
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
            //if (e.PropertyName == "IsChecked")
            //{
            //    var item = sender as ListItem;

            //    Task.Run(() =>
            //    {
            //        try
            //        {
            //            if (item.IsChecked)
            //            {
            //                item.PropertyChanged -= CheckedListItemPropertyChanged;
            //                CheckedList.Remove(item);
            //                UncheckedList.Add(item);
            //            }
            //        }
            //        catch (Exception ex)
            //        {
                        
            //        }
            //    });
            //}
        }

        public async Task<string> ShowDialog(string title, string message, string buttonOne, string buttonTwo)
        {
            return await StaticData.CurrentPage.DisplayAlert(title, message, buttonOne, buttonTwo) ? buttonOne : buttonTwo;
        }

        private void UncheckedListItemOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            _lastTypedTime = DateTime.Now;

            if (_checkTask == null)
            {
                _checkTask = Task.Run(async () =>
                {
                    try
                    {
                        while ((DateTime.Now - _lastTypedTime) < TimeSpan.FromMilliseconds(500))
                        {
                            await Task.Delay(500);
                        }

                        if (UncheckedList.All(i => !string.IsNullOrWhiteSpace(i.Label)))
                        {
                            UncheckedList.Add(new ListItem());
                        }

                        Save();
                    }
                    catch (Exception)
                    {

                    }
                    finally
                    {
                        _checkTask = null;
                    }
                });

            }

            

            //if (e.PropertyName == "IsChecked")
            //{
            //    var item = sender as ListItem;

            //    Task.Run(() =>
            //    {
            //        try
            //        {
            //            if (item.IsChecked)
            //            {
            //                item.PropertyChanged -= UncheckedListItemOnPropertyChanged;
            //                UncheckedList.Remove(item);
            //                CheckedList.Add(item);
            //            }
            //        }
            //        catch (Exception ex)
            //        {

            //        }
            //    });
            //}
        }

        public void Save()
        {
            try
            {
                if (UncheckedList.Any())
                {
                    var serializedUnchecked = JsonConvert.SerializeObject(UncheckedList.Where(l => !string.IsNullOrWhiteSpace(l.Label)).ToList());
                    File.WriteAllText(UncheckedPath, serializedUnchecked);
                }
                else if (File.Exists(UncheckedPath))
                {
                    File.Delete(UncheckedPath);
                }

                if (CheckedList.Any())
                {
                    var serializedChecked = JsonConvert.SerializeObject(CheckedList.Where(l => !string.IsNullOrWhiteSpace(l.Label)).ToList());
                    File.WriteAllText(CheckedPath, serializedChecked);
                }
                else if (File.Exists(CheckedPath))
                {
                    File.Delete(CheckedPath);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void Load()
        {
            try
            {
                ClearLists();

                if (File.Exists(UncheckedPath))
                {
                    var serializedUnchecked = File.ReadAllText(UncheckedPath);

                    var list = JsonConvert.DeserializeObject<List<ListItem>>(serializedUnchecked);

                    foreach (var listItem in list)
                    {
                        UncheckedList.Add(listItem);
                    }
                }

                if (File.Exists(CheckedPath))
                {
                    var serializedChecked = File.ReadAllText(CheckedPath);

                    var list = JsonConvert.DeserializeObject<List<ListItem>>(serializedChecked);

                    foreach (var listItem in list)
                    {
                        CheckedList.Add(listItem);
                    }
                }

                if (!UncheckedList.Any() || !string.IsNullOrWhiteSpace(UncheckedList.Last().Label))
                {
                    UncheckedList.Add(new ListItem());
                }
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
