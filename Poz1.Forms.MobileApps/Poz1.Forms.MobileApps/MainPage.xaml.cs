using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Poz1.Forms.MobileApps
{
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        public static MobileServiceClient MobileService = new MobileServiceClient("__INSERT_YOUR_MOBILE_SERVICE__" );

        private ObservableCollection<TodoItem> _toDoList;
        public ObservableCollection<TodoItem> ToDoList
        {
            get { return _toDoList; }
            set
            {
                if (_toDoList != value)
                {
                    _toDoList = value;
                    RaisePropertyChanged();
                }
            }
        }

        private string _currentItem;
        public string CurrentItem
        {
            get { return _currentItem; }
            set
            {
                if (_currentItem != value)
                {
                    _currentItem = value;
                    RaisePropertyChanged();
                }
            }
        }

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;

            LoadToDoList();
        }

        private async Task LoadToDoList()
        {
            ToDoList = new ObservableCollection<TodoItem>(await MobileService.GetTable<TodoItem>().ToListAsync());          
            AddButton.Clicked += AddButton_Clicked;
        }

        private async void AddButton_Clicked(object sender, System.EventArgs e)
        {
            var item = new TodoItem() { Text = CurrentItem };
            ToDoList.Add(item);
            await MobileService.GetTable<TodoItem>().InsertAsync(item);
        }

        //-- INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
