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
using ExileFinder.Models;

namespace ExileFinder.Parts
{
    /// <summary>
    /// Interaction logic for List.xaml
    /// </summary>
    public partial class List : UserControl
    {
        private List<Party> partyList; 

        public List()
        {
            InitializeComponent();

            partyList = new List<Party>();

            partyList.Add(new Party("Come map with us!",PartyType.Maps));
            partyList.Add(new Party("Come farm Piety!",PartyType.Bosses));
            partyList.Add(new Party("Lets level together.",PartyType.Leveling));
            partyList.Add(new Party("Dominus and Piery Boost",PartyType.Other));
            partyList.Add(new Party("High maps, 85+",PartyType.Maps));

            ListBox.ItemsSource = partyList;

        }

        private void BackButton_OnClick(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new Start());
        }
    }
}
