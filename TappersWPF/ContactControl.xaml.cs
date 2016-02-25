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

namespace TappersWPF
{
    /// <summary>
    /// Interaction logic for ContactControl.xaml
    /// </summary>
    public partial class ContactControl : UserControl
    {

        private Contact bindedContact;

        public Contact BindedContact
        {
            get { return bindedContact; }
            private set { bindedContact = value; }
        }



        public ContactControl(Contact bindedContact)
        {
            this.bindedContact = bindedContact;
            InitializeComponent();

            lblName.Content = bindedContact.Name;

            if(bindedContact.Transactions == null)
            {
                lblTotal.Text = "You and " + bindedContact.Name + " don't owe each other anything!";
            }

            Background bg = Cache.Instance.getCachedBackground(bindedContact.BackgroundColour);

            LinearGradientBrush gradientBrush = new LinearGradientBrush(Utils.getBackgroundColour(bg.PrimaryColour
    ), Utils.getBackgroundColour(bg.SecondaryColour), new Point(0.5, 0), new Point(0.5, 1));

            Background = gradientBrush;

            recBackground.Fill = gradientBrush;

            imgSmallCharacter.Source = Cache.Instance.getLargeImageFor(bindedContact.Character);

            lblTotal.Text = getTotal();
        }

        public string getTotal()
        {
            double totalAm = 0;
            if (BindedContact.Transactions == null)
                {
                    BindedContact.Transactions = new List<Transaction>();
                }
            foreach (Transaction tran in BindedContact.Transactions)
                {
                    if (tran.Type == TransactionType.TO)
                    {
                        totalAm += tran.Amount;
                    }
                    else
                    {
                        totalAm -= tran.Amount;
                    }

                }
            if (totalAm < 0)
            {
                return "You owe "+ bindedContact.Name+"  total of £" + Math.Abs(totalAm);
            }
            else if (totalAm > 0)
            {
                return bindedContact.Name + " owes you a total of £" + Math.Abs(totalAm);
            }
            else
            {
                return "You both owe each other nothing!";
            }
        }

        private void recClickBoard_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void recClickBoard_MouseEnter(object sender, MouseEventArgs e)
        {
        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
        }

        private void UserControl_MouseUp(object sender, MouseButtonEventArgs e)
        {

            if (MainPage.Instance.SelectedPage == this)
            {
                MainPage.Instance.unselectAll();
                MainPage.Instance.SelectedPage = null;
                MainPage.Instance.rightGrid(Visibility.Hidden);
            }
            else
            {
                MainPage.Instance.unselectAll();
                grdMain.Background = new SolidColorBrush(Color.FromRgb(235, 236, 238));
                MainPage.Instance.SelectedPage = this;
                MainPage.Instance.refresh();
                MainPage.Instance.rightGrid(Visibility.Visible);
                MainPage.Instance.imgBigCharacter.Source = Cache.Instance.getLargeImageFor(BindedContact.Character);
                MainPage.Instance.populateTransactions();
            }


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Cache.Instance.GetLibrary.Contacts.Remove(bindedContact);
            MainPage.Instance.stkContacts.Children.Remove(this);
            MainPage.Instance.lblContactCounter.Content = Cache.Instance.GetLibrary.Contacts.Count;

            if (MainPage.Instance.SelectedPage == this)
            {
                MainPage.Instance.rightGrid(Visibility.Hidden);
            }

        }

        private void lblDelete_MouseEnter(object sender, MouseEventArgs e)
        {
            lblDelete.Foreground = new SolidColorBrush(Color.FromRgb(255, 107, 107));
        }

        private void lblDelete_MouseLeave(object sender, MouseEventArgs e)
        {
            lblDelete.Foreground = new SolidColorBrush(Color.FromRgb(185, 134, 134));
        }

        private void lblDelete_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MainPage.Instance.sendConfirmDelete(this);
            MainPage.Instance.unselectAll();
            MainPage.Instance.SelectedPage = null;
            MainPage.Instance.rightGrid(Visibility.Hidden);
        }
    }
}
