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

namespace MD5_Implementation_WPF
{
    /// <summary>
    /// Interaction logic for DialogSocial.xaml
    /// </summary>
    public partial class DialogSocial
    {
        public DialogSocial(string name)
        {
            InitializeComponent();
            tbxTitle.Text = $"Are you sure go to {name}?";
        }

        public string RedirectLink { get; set; }

        private void btnGoSocial_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(RedirectLink);
        }
    }
}
