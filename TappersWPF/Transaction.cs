using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TappersWPF
{
    [Serializable]
    public class Transaction
    {

        private int contactID;

        public int ContactID
        {
            get { return contactID; }
            set { contactID = value; }
        }



        private string date;

        public string Date
        {
            get { return date; }
            set { date = value; }
        }

        private string reason;

        public string Reason
        {
            get { return reason; }
            set { reason = value; }
        }

        private double amount;

        public double Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        private TransactionType type;

        public TransactionType Type
        {
            get { return type; }
            set { type = value; }
        }


    }
}
