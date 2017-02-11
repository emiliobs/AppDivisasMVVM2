using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AppDivisasMVVMApi.Classes;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;

namespace AppDivisasMVVMApi.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {




        #region Attributes

        private ExchangeRates exchangeRates;

        private decimal amount;
        private double sourceRate;
        private double targetRate;
        private bool isEnable;
        private bool isRunning;
        private string message;

        #endregion


        #region Properties

        public ObservableCollection<Rate> Rates { get; set; }

        public decimal Amount
        {
            set
            {
                if (amount != value);
                {
                    amount = value;

                    PropertyChanged?.Invoke(this,new PropertyChangedEventArgs("Amount"));
                }
            }

            get { return amount; }
        }


        public double SourceRate
        {
            set
            {
                if (sourceRate != value);
                {
                    sourceRate = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SourceRate"));
                }
            }

            get { return sourceRate; }
        }

        public double TargetRate
        {
            set
            {
                if (targetRate != value) ;
                {
                    targetRate = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TargetRate"));
                }
            }

            get { return targetRate; }
        }

        public bool IsEnable
        {
            set
            {
                if (isEnable != value) ;
                {
                    isEnable = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsEnable"));
                }
            }

            get { return isEnable; }
        }

        public bool IsRunning
        {
            set
            {
                if (isRunning != value) ;
                {
                   isRunning= value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRunning"));
                }
            }

            get { return isRunning; }
        }

        public string  Message
        {
            set
            {
                if (message != value) ;
                {
                    message = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Message"));
                }
            }

            get { return message; }
        }



        #endregion

        #region Commands

        public ICommand ConvertMoneyCommand
        {
            get { return  new RelayCommand(ConvertMoney);}
        }

        private async void ConvertMoney()
        {
            //valido campos(propiedades)

            if (Amount <= 0)
            {
                await App.Current.MainPage.DisplayAlert("Error", "You must enter a`positive value in amount","Acept");
                return;
            }

            if (sourceRate == 0)
            {
                await App.Current.MainPage.DisplayAlert("Error", "You must select a source rate", "Acept");
                return;
            }

            if (TargetRate == 0)
            {
                await App.Current.MainPage.DisplayAlert("Error", "You must select a source target", "Acept");
                return;
            }

            var converted = (Amount / (decimal)SourceRate) * (decimal)TargetRate;

            Message = $"{Amount:C2} {converted:C2}";


        }

        #endregion


        #region Constructor
        public MainViewModel()
        {
            Rates = new ObservableCollection<Rate>();

            Message = "Enter an amount, select a source currency, select a target and press Convert button.";

            LoadRates();
        }



        #endregion



        #region Methods
        private async void LoadRates()
        {
            IsRunning = true;

            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri("https://openexchangerates.org");
                var url = "/api/latest.json?app_id=7ce0119051774ef5a478eefc8f8070d1";
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    Message = response.StatusCode.ToString();
                    IsRunning = false;

                    return;
                }


                var result = await response.Content.ReadAsStringAsync();
                exchangeRates = JsonConvert.DeserializeObject<ExchangeRates>(result);

            }
            catch (Exception e)
            {
                Message = e.Message;
                IsRunning = false;


                return;
            }


            ConvertRates();

            IsRunning = false;
            IsEnable = true;
        }

        private void ConvertRates()
        {
            Rates.Clear();

            var type = typeof(Rates);
            var properties = type.GetRuntimeFields();

            foreach (var property in properties)
            {
                var code = property.Name.Substring(1, 3);

                Rates.Add(new Rate()
                {
                    Code = code,
                    TaxRate = (double)property.GetValue(exchangeRates.Rates)
                });
            }
        }

        #endregion

        #region Event
        public event PropertyChangedEventHandler PropertyChanged;



        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


        }

        #endregion
    }
  }
