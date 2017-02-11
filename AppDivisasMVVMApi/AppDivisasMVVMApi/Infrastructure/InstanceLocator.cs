using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppDivisasMVVMApi.ViewModels;

namespace AppDivisasMVVMApi.Infrastructure
{
     public class InstanceLocator
    {
        #region Properties

        public MainViewModel Main { get; set; }

        #endregion

        #region Constructor
        public InstanceLocator()
        {
            Main = new MainViewModel();
        }
        #endregion

    }
}
