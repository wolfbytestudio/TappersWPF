using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TappersWPF
{
    [Serializable]
    public class Background
    {

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

        private string primaryColour;

        public string PrimaryColour
        {
            get { return primaryColour; }
            set { primaryColour = value; }
        }

        private string secondaryColour;

        public string SecondaryColour
        {
            get { return secondaryColour; }
            set { secondaryColour = value; }
        }

        private int version;

        public int Version
        {
            get { return version; }
            set { version = value; }
        }

        public Background(int id, string name, string primaryColour, string secondaryColour, int version)
        {
            this.id = id;
            this.name = name;
            this.primaryColour = primaryColour;
            this.secondaryColour = secondaryColour;
            this.version = version;
        }

    }

}
