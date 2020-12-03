using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NaijaConnect.Styles
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Buttons : ResourceDictionary
    {
        public Buttons()
        {
            InitializeComponent();
        }
    }
}