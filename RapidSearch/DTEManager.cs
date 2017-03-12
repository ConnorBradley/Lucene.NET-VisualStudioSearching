using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;
using Microsoft.VisualStudio.Shell;

namespace RapidSearch
{
    class DTEManager
    {

       public static DTE getCurrentDTE(IServiceProvider provider)
       {
            DTE vs = (DTE)provider.GetService(typeof(DTE));
            return vs;
       }

       public static DTE getCurrentDTE()
       {
           return getCurrentDTE(ServiceProvider.GlobalProvider);
       }
    }
}
