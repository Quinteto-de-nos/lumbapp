using LumbApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GUI
{
    /// <summary>
    /// Interaction logic for UserDataInput.xaml
    /// </summary>
    public partial class UserDataInput : Page
    {
        public GUIController controller { get; set; }
        public UserDataInput(GUIController guiController)
        {
            InitializeComponent();
            controller = guiController;
        }

        public void ProcesarDatos()
        {
            var studentData = new StudentData();
            studentData.NombreCompleto = fullName.Text;
        }
    }
}
