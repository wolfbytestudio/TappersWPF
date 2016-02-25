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
    /// Interaction logic for TransactionControl.xaml
    /// </summary>
    public partial class TransactionControl : UserControl
    {


        private readonly static LinearGradientBrush BOTTOM_BLUE = new LinearGradientBrush(
                (Color)ColorConverter.ConvertFromString("#37A3FF"),
                (Color)ColorConverter.ConvertFromString("#CBCBCB"),
                new Point(0.5, 0),
                new Point(0.5, 1)
            );


        private readonly static LinearGradientBrush BOTTOM_RED = new LinearGradientBrush(
                (Color)ColorConverter.ConvertFromString("#DE6868"),
                (Color)ColorConverter.ConvertFromString("#CBCBCB"),
                new Point(0.5, 0),
                new Point(0.5, 1)
            );

        private readonly static LinearGradientBrush TOP_RED = new LinearGradientBrush(
                (Color)ColorConverter.ConvertFromString("#CBCBCB"),
                (Color)ColorConverter.ConvertFromString("#DE6868"),
                new Point(0.5, 0),
                new Point(0.5, 1)
            );

        private readonly static LinearGradientBrush TOP_BLUE = new LinearGradientBrush(
                (Color)ColorConverter.ConvertFromString("#CBCBCB"),
                (Color)ColorConverter.ConvertFromString("#37A3FF"),
                new Point(0.5, 0),
                new Point(0.5, 1)
            );

        private readonly static SolidColorBrush BLUE = new SolidColorBrush(Color.FromRgb(55, 163, 255));
        
        private readonly static SolidColorBrush RED = new SolidColorBrush(Color.FromRgb(222, 104, 104));

        Transaction bindedTransaction;

        private Contact contact;
        public Transaction BindedTransaction
        {
            get
            {
                return bindedTransaction;
            }

            set
            {
                bindedTransaction = value;
            }
        }

        public Contact _Contact
        {
            get
            {
                return contact;
            }

            set
            {
                contact = value;
            }
        }

        

        public TransactionControl(Transaction bindedTransaction)
        {
            this.BindedTransaction = bindedTransaction;
            InitializeComponent();

            lblDate.Text = bindedTransaction.Date;
            lblReason.Text = bindedTransaction.Reason;
            if(lblReason.Text == "")
            {
                lblReason.Text = "Reason unspecific";
            }
            _Contact = Cache.Instance.getContactForID(bindedTransaction.ContactID);

            lblTitle.Text += generateText();

            if (bindedTransaction.Type == TransactionType.TO)
            {
                imgToFrom.Source = new BitmapImage(new Uri(@"/TappersWPF;component/images/to_icon.png", UriKind.Relative));
            }
            else
            {
               imgToFrom.Source = new BitmapImage(new Uri(@"/TappersWPF;component/images/from_icon.png", UriKind.Relative));
            }



            if (bindedTransaction.Type == TransactionType.TO)
            {
                recTop.Fill = TOP_BLUE;
                recBottom.Fill = BOTTOM_BLUE;
                recCircle.Stroke = BLUE;
            }
            else
            {
                recTop.Fill = TOP_RED;
                recBottom.Fill = BOTTOM_RED;
                recCircle.Stroke = RED;
            }
        }

        public string generateText()
        {
            string strOut = "";

            if (BindedTransaction.Type == TransactionType.TO)
            {
                strOut = "You lent £" + BindedTransaction.Amount + " to " + _Contact.Name;
            }
            else
            {
                strOut = "You borrowed £" + BindedTransaction.Amount + " from " + _Contact.Name;
            }
            return strOut;
        }



        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
           
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {

        }

        private void lblDelete_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MainPage.Instance.ToDelteTransaction = this;
            MainPage.Instance.sendConfirmDeleteTransaction();
        }
    }
}
