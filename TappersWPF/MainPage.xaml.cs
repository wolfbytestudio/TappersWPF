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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TappersWPF
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {

        private static MainPage instance;

        public static MainPage Instance
        {
            get { return MainPage.instance; }
            private set { MainPage.instance = value; }
        }

        private ContactControl selectedPage;

        public ContactControl SelectedPage
        {
            get { return selectedPage; }
            set { selectedPage = value; }
        }

        private ContactControl delete;

        public MainPage()
        {
            Instance = this;
            InitializeComponent();
            unpackContacts();
            if(selectedPage == null)
            {
                rightGrid(Visibility.Hidden);
            }
            populateCharactersBox();
            populateBackgroundBox();

            lblTotalEveryone.Text = getTotal();


        }

        public string getTotal()
        {
            double totalAm = 0;
            foreach(Contact c in Cache.Instance.GetLibrary.Contacts)
            {
                if(c.Transactions == null)
                {
                    c.Transactions = new List<Transaction>();
                }
                foreach(Transaction tran in c.Transactions)
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
            }
            if (totalAm < 0)
            {
                return "You owe a total of £" + Math.Abs(totalAm);
            }
            else if (totalAm > 0)
            {
                return "You are owed a total of £" + Math.Abs(totalAm);
            }
            else
            {
                return "Nobody owes anyone!";
            }
        }

        

        
        #region New Contact Stuff

        private int characterSelected = 1;
        private int backgroundSelected = 1;
        private Dictionary<Image, Character> characterBoxes = new Dictionary<Image, Character>();
        private Dictionary<Rectangle, Background> backgroundBoxes = new Dictionary<Rectangle, Background>();

        public void populateCharactersBox()
        {
            foreach (Character ch in Cache.Instance.Characters)
            {
                Image image = new Image();
                image.Width = 50;
                image.Height = 50;
                image.Source = Cache.Instance.getLargeImageFor(ch.Id);
                image.MouseUp += character_mouseUp;
                characterBoxes.Add(image, ch);
                stkCharacters.Children.Add(image);
            }

            imgCharacterPreview.Source = Cache.Instance.Characters[0].LargeBitmapImage;
        }

        public void populateBackgroundBox()
        {
            foreach (Background bg in Cache.Instance.Backgrounds)
            {
                Rectangle rec = new Rectangle();
                rec.Width = 35;
                rec.Height = 35;
                rec.RadiusX = 100;
                rec.StrokeThickness = 1;
                rec.RadiusY = 100;
                rec.Margin = new Thickness(0, 0, 5, 5);
                rec.Stroke = new SolidColorBrush(Colors.Black);
                rec.MouseUp += bg_mouseUp;
                rec.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(bg.PrimaryColour));
                backgroundBoxes.Add(rec, bg);
                stkBackgrounds.Children.Add(rec);
            }
        }

        private void bg_mouseUp(object sender, MouseButtonEventArgs e)
        {

            Rectangle sen = (Rectangle) sender;

            LinearGradientBrush gradientBrush = new LinearGradientBrush(Utils.getBackgroundColour(backgroundBoxes[sen].PrimaryColour
                ), Utils.getBackgroundColour(backgroundBoxes[sen].SecondaryColour), new Point(0.5, 0), new Point(0.5, 1));
            Background = gradientBrush;

            recBackgroundPreview.Fill = gradientBrush; 
            backgroundSelected = backgroundBoxes[sen].Id;
            
        }

        private void character_mouseUp(object sender, MouseButtonEventArgs e)
        {
            Image image = (Image)sender;
            image.Width = 50;
            imgCharacterPreview.Source = image.Source;
            characterSelected = characterBoxes[image].Id;
            image.Height = 50;
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            string name = txtNewContactName.Text;
            Contact c = new Contact(Contact.getRandomID(), name, characterSelected, backgroundSelected, null, Account.yourID);
            Cache.Instance.GetLibrary.addContact(c);
            addToScreen(c);
            exitNewContact();
        }


        private void exitNewContact()
        {
            grdNewContact.Visibility = Visibility.Hidden;
            recBlackbakground.Visibility = Visibility.Hidden;
            grdMain.Effect = null;
        }

        private void btnNewCancel_Click(object sender, RoutedEventArgs e)
        {
            exitNewContact();
        }

        #endregion



        public void rightGrid(Visibility vis)
        {
            grdRight.Visibility = vis;
        }

        private void pop()
        {
            foreach(ContactControl s in stkContacts.Children)
            {
                Cache.Instance.GetLibrary.addContact(s.BindedContact);
            }
            lblContactCounter.Content = Cache.Instance.GetLibrary.Contacts.Count;
        }

        private void unpackContacts()
        {
            foreach (Contact con in Cache.Instance.GetLibrary.Contacts)
            {
                if(con.YourID == Account.yourID)
                {
                    stkContacts.Children.Add(new ContactControl(con));
                }
            }
            lblContactCounter.Content = Cache.Instance.GetLibrary.Contacts.Count;
        }
        public void unselectAll()
        {
            foreach (ContactControl control in stkContacts.Children)
            {
                control.grdMain.Background = new SolidColorBrush(Color.FromRgb(255,255,255));
            }

        }
        public void addToScreen(Contact c)
        {
            stkContacts.Children.Add(new ContactControl(c));
            lblContactCounter.Content = Cache.Instance.GetLibrary.Contacts.Count;
        }


        public void refresh()
        {
            txtRightName.Text = selectedPage.BindedContact.Name;

            Background bg = Cache.Instance
                .getCachedBackground(selectedPage.BindedContact.BackgroundColour);

            LinearGradientBrush gradientBrush = new LinearGradientBrush(Utils.getBackgroundColour(bg.PrimaryColour
                ), Utils.getBackgroundColour(bg.SecondaryColour), new Point(0.5, 0), new Point(0.5, 1));
            Background = gradientBrush;

            recRightColour.Fill = gradientBrush;

            lblContactCounter.Content = Cache.Instance.GetLibrary.Contacts.Count;
            
        }


        private void btnNewContact_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                grdNewContact.Visibility = System.Windows.Visibility.Visible;
                recBlackbakground.Visibility = System.Windows.Visibility.Visible;
                BlurEffect s = new BlurEffect();
                s.RenderingBias = RenderingBias.Quality;
                s.Radius = 7;
                s.KernelType = KernelType.Gaussian;

                grdMain.Effect = s;
            }
            catch
            {

            }

        }

        public void sendConfirmDelete(ContactControl toDelete)
        {
            this.delete = toDelete;
            unselectAll();
            SelectedPage = null;
            rightGrid(Visibility.Hidden);

            grdMain.IsEnabled = false;
            recBlackbakground.Visibility = Visibility.Visible;
            grdConfirm.Visibility = Visibility.Visible;

            BlurEffect s = new BlurEffect();
            s.RenderingBias = RenderingBias.Quality;
            s.Radius = 7;
            s.KernelType = KernelType.Gaussian;

            grdMain.Effect = s;

            lblConfirmDeleteText.Text = "Are you sure you want to delete the contact " + toDelete.BindedContact.Name + "?";
            

        }


        private void imgUpload_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Cache.Instance.GetLibrary.deleteAllContacts();
            Cache.Instance.GetLibrary.syncContacts();
            MessageBox.Show("You successfully upload your contacts!");
        }

        private void imgDownload_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Cache.Instance.GetLibrary.getYourContacts();

            stkContacts.Children.Clear();

            foreach (Contact con in Cache.Instance.GetLibrary.Contacts)
            {
                stkContacts.Children.Add(new ContactControl(con));
            }
            lblContactCounter.Content = Cache.Instance.GetLibrary.Contacts.Count;
        }

        private void btnNewContact_MouseEnter(object sender, MouseEventArgs e)
        {
            btnNewContact.Background = new SolidColorBrush(Color.FromArgb(10, 0, 0, 0));
        }

        private void btnNewContact_MouseLeave(object sender, MouseEventArgs e)
        {
            btnNewContact.Background = null;
        }

        private void btnConfirmCancel_Click(object sender, RoutedEventArgs e)
        {
            grdMain.IsEnabled = true;
            recBlackbakground.Visibility = Visibility.Hidden;
            grdConfirm.Visibility = Visibility.Hidden;
            grdMain.Effect = null;
        }

        private void btnConfirmDelete_Click(object sender, RoutedEventArgs e)
        {
            grdMain.Effect = null;
            grdMain.IsEnabled = true;
            recBlackbakground.Visibility = Visibility.Hidden;
            grdConfirm.Visibility = Visibility.Hidden;

            Cache.Instance.GetLibrary.Contacts.Remove(delete.BindedContact);
            MainPage.Instance.stkContacts.Children.Remove(delete);
            MainPage.Instance.lblContactCounter.Content = Cache.Instance.GetLibrary.Contacts.Count;

            if (selectedPage == delete)
            {
                MainPage.Instance.rightGrid(Visibility.Hidden);
            }
            lblTotalEveryone.Text = getTotal();
        }

        private void lblNewTransaction_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.grdNewTransaction.Visibility = Visibility.Visible;
            this.recBlackbakground.Visibility = Visibility.Visible;
        }

        public void closeTransactionBox()
        {
            this.grdNewTransaction.Visibility = Visibility.Hidden;
            this.recBlackbakground.Visibility = Visibility.Hidden;
        }

        private void btnNewTranConfirm_Click(object sender, RoutedEventArgs e)
        {
            Transaction tran = new Transaction();

            tran.ContactID = selectedPage.BindedContact.Id;

            tran.Amount = double.Parse(txtNewTransAmount.Text);
            tran.Date = DateTime.Now.ToString();
            tran.Reason = txtNewTranReason.Text;

            if (rdbFrom.IsChecked == true)
            {
                tran.Type = TransactionType.FROM;
            }
            else
            {
                tran.Type = TransactionType.TO;
            }
            SelectedPage.BindedContact.addTransaction(tran);
            stkTransactions.Children.Add(new TransactionControl(tran));

            closeTransactionBox();

            lblTotalTransactions.Text = selectedPage.getTotal();
            lblTotalEveryone.Text = getTotal();
        }




        public void populateTransactions()
        {
            stkTransactions.Children.Clear();
            if (SelectedPage.BindedContact.Transactions == null)
            {
                return;
            }
            foreach(Transaction tran in SelectedPage.BindedContact.Transactions)
            {
                stkTransactions.Children.Add(new TransactionControl(tran));
            }

            lblTotalTransactions.Text = selectedPage.getTotal();
        }
    }
}
