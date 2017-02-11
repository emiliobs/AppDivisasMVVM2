using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AppDivisasMVVMApi.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {




        #region Attributes




        #endregion


        #region Properties

        #endregion


        #region Commands

        #endregion

        #region Methods

        #endregion

        #region Event
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


        }

        #endregion    }
    }
