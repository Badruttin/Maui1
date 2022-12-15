using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace MauiTest1
{
	public class ApplicationViewModel : INotifyPropertyChanged
	{

        bool initialized = false;   // была ли начальная инициализация
        VidRabot selectedVRabot;  // выбранный друг
        private bool isBusy;    // идет ли загрузка с сервера

        public ObservableCollection<VidRabot> VidRabots{ get; set; }
        VidrabotService vrService = new VidrabotService();
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand CreateVidRabotCommand { protected set; get; }
        public ICommand DeleteVidRabotCommand { protected set; get; }
        public ICommand SaveVidRabotCommand { protected set; get; }
        public ICommand BackCommand { protected set; get; }

        public INavigation Navigation { get; set; }

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged("IsBusy");
                OnPropertyChanged("IsLoaded");
            }
        }
        public bool IsLoaded
        {
            get { return !isBusy; }
        }

        public ApplicationViewModel()
        {
            VidRabots = new ObservableCollection<VidRabot>();
            IsBusy = false;
            CreateVidRabotCommand = new Command(CreateVR);
            DeleteVidRabotCommand = new Command(DeleteVR);
            SaveVidRabotCommand = new Command(SaveVR);
            BackCommand = new Command(Back);
        }

        public VidRabot SelectedVR        {
            get { return selectedVRabot; }
            set
            {
                if (selectedVRabot != value)
                {
                    VidRabot tempVR = new VidRabot()
                    {
                        Id = value.Id,
                        Name = value.Name,
                        ShortName = value.ShortName,
                        Description = value.Description
                    };
                    selectedVRabot = null;
                    OnPropertyChanged("SelectedVR");
                    Navigation.PushAsync(new VidRabotPage(tempVR, this));
                }
            }
        }
        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        private async void CreateVR()
        {
            await Navigation.PushAsync(new VidRabotPage(new VidRabot(), this));
        }
        private void Back()
        {
            Navigation.PopAsync();
        }

        public async Task GetVidRabot()
        {
            if (initialized == true) return;
            IsBusy = true;
            IEnumerable<VidRabot> vrabots = await vrService.Get();

            // очищаем список
            //Friends.Clear();
            while (VidRabots.Any())
                VidRabots.RemoveAt(VidRabots.Count - 1);

            // добавляем загруженные данные
            foreach (VidRabot f in vrabots)
                VidRabots.Add(f);
            IsBusy = false;
            initialized = true;
        }
        private async void SaveVR(object vidrabotObject)
        {
            VidRabot vidrabot = vidrabotObject as VidRabot;
            if (vidrabot != null)
            {
                IsBusy = true;
                // редактирование
                if (vidrabot.Id > 0)
                {
                    VidRabot updatedVR = await vrService.Update(vidrabot);
                    // заменяем объект в списке на новый
                    if (updatedVR != null)
                    {
                        int pos = VidRabots.IndexOf(updatedVR);
                        VidRabots.RemoveAt(pos);
                        VidRabots.Insert(pos, updatedVR);
                    }
                }
                // добавление
                else
                {
                    VidRabot addedVR = await vrService.Add(vidrabot);
                    if (addedVR != null)
                        VidRabots.Add(addedVR);
                }
                IsBusy = false;
            }
            Back();
        }
        private async void DeleteVR(object vidrabotObject)
        {
            VidRabot vidrabot = vidrabotObject as VidRabot;
            if (vidrabot != null)
            {
                IsBusy = true;
                VidRabot deletedVR = await vrService.Delete(vidrabot.Id);
                if (deletedVR != null)
                {
                    VidRabots.Remove(deletedVR);
                }
                IsBusy = false;
            }
            Back();
        }
    }
}

