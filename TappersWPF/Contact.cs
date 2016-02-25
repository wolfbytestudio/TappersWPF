using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TappersWPF
{
    [Serializable]
    public class Contact
    {

        private int yourID;

        public int YourID
        {
            get { return yourID; }
            set { yourID = value; }
        }

        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string date;

        public string Date
        {
            get { return date; }
            set { date = value; }
        }


        private int character;

        public int Character
        {
            get { return character; }
            set { character = value; }
        }

        private int background;

        public int BackgroundColour
        {
            get { return background; }
            set { background = value; }
        }

        private List<Transaction> transactions;

        public List<Transaction> Transactions
        {
            get { return transactions; }
            set { transactions = value; }
        }

        public Contact(int id, string name, int character, int background, List<Transaction> transactions, int yourID)
        {
            this.id = System.Guid.NewGuid().ToString().GetHashCode();
            this.name = name;
            this.character = character;
            this.background = background;
            this.transactions = transactions;
            this.yourID = yourID;
        }


        public static int getRandomID()
        {
           return System.Guid.NewGuid().ToString().GetHashCode();
        }


        public void addTransaction(Transaction tran)
        {
            if(transactions == null)
            {
                transactions = new List<Transaction>();
            }

            transactions.Add(tran);
        }

    }
}
