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

        Transaction bindedTransaction;

        Contact contact;

        public TransactionControl(Transaction bindedTransaction)
        {
            this.bindedTransaction = bindedTransaction;
            InitializeComponent();

            lblDate.Text = bindedTransaction.Date;
            lblReason.Text = bindedTransaction.Reason;
            if(lblReason.Text == "")
            {
                lblReason.Text = "Reason unspecific";
            }
            contact = Cache.Instance.getContactForID(bindedTransaction.ContactID);

            lblTitle.Text += generateText();

            if (bindedTransaction.Type == TransactionType.TO)
            {
                imgToFrom.Source = new BitmapImage(new Uri(@"/TappersWPF;component/images/to_icon.png", UriKind.Relative));
            }
            else
            {
               imgToFrom.Source = new BitmapImage(new Uri(@"/TappersWPF;component/images/from_icon.png", UriKind.Relative));
            }
        }

        public string generateText()
        {
            string strOut = "";

            if (bindedTransaction.Type == TransactionType.TO)
            {
                strOut = "You lent £" + bindedTransaction.Amount + " to " + contact.Name;
            }
            else
            {
                strOut = "You borrowed £" + bindedTransaction.Amount + " from " + contact.Name;
            }
            return strOut;
        }



        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
           
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {

        }
    }
}
