using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace TappersWPF
{

    [Serializable]
    public class Character
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

        private string largeImage;
        public string LargeImage
        {
            get { return largeImage; }
            set { largeImage = value; }
        }

        private string smallImage;
        public string SmallImage
        {
            get { return smallImage; }
            set { smallImage = value; }
        }

        private int version;
        public int Version
        {
            get { return version; }
            set { version = value; }
        }



        public Character(int id, string name, Image largeImage, Image smallImage, int version)
        {
            this.id = id;
            this.name = name;
            //this.largeImage = largeImage;
           // this.smallImage = smallImage;
            this.version = version;
        }

        [NonSerialized]
        private BitmapImage largeBitmapImage;
        
        public BitmapImage LargeBitmapImage
        {
            get {  return largeBitmapImage; }
            set { largeBitmapImage = value; }
        }

        public void generateBitmaps()
        {
            Uri i = new Uri(Cache.DIRECTORY + "images/" + name + "_large.png");
            largeBitmapImage = new BitmapImage(i);
            Console.WriteLine("Generated bitmap " + i.ToString());
        }

    }
}
