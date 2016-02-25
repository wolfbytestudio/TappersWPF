using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace TappersWPF
{
    [Serializable]
    public class Cache
    {

        public static readonly string DIRECTORY = Environment.ExpandEnvironmentVariables("%AppData%") + "/.tappers/";

        private Library library;

        public Library GetLibrary
        {
            get 
            { 
                if(library == null)
                {
                    library = new Library();
                }
                return library; 
            
            }
            set { library = value; }
        }


        private static Cache cache;

        public static Cache Instance
        {
            get 
            {
                if (cache == null) cache = new Cache();
                return cache;
            }
            private set { cache = value; }
        }

        public void init()
        {
            library = GetLibrary;
        }

        public Contact getContactForID(int id)
        {
            return library.getContactForID(id);
        }

        public Cache()
        {
            
            characters = new List<Character>();
            backgrounds = new List<Background>();
        }

        private List<Character> characters = new List<Character>();

        public List<Character> Characters
        {
            get { return characters; }
            private set { characters = value; }
        }

        private List<Background> backgrounds = new List<Background>();

        public List<Background> Backgrounds
        {
            get { return backgrounds; }
            private set { backgrounds = value; }
        }


        public void getAllCharacters()
        {
            if(characters.Count <= 1)
            {
                return;
            }
            characters = Cache.Instance.GetLibrary.getAllCharacters();
        }

        public Background getCachedBackground(int id)
        {
            foreach (Background bg in backgrounds)
            {
                if (bg.Id == id)
                {
                    return bg;
                }
            }
            return null;
        }

        public BitmapImage getLargeImageFor(int characterId)
        {

            foreach(Character ch in characters)
            {
                if(characterId == ch.Id)
                {
                    return ch.LargeBitmapImage;
                }
            }
            return null;
        }

        public void attemptCharacterUpdates()
        {
            if (characters.Count != Cache.Instance.GetLibrary.getCharacterCount())
            {
                characters = Cache.Instance.GetLibrary.getAllCharacters();
            }
            else
            {
                for (int i = 0; i < characters.Count; i++)
                {
                    if (characters[i].Version != Cache.Instance.GetLibrary.getVersionCharacter(characters[i].Id))
                    {
                        characters[i] = Cache.Instance.GetLibrary.getCharacter(characters[i].Id);
                    }
                    characters[i].generateBitmaps();
                }
            }

        }

        public void attemptBackgroundUpdates()
        {
            if(backgrounds == null)
            {
                return;
            }
            if (backgrounds.Count != Cache.Instance.GetLibrary.getBackgroundCount())
            {
                backgrounds = Cache.Instance.GetLibrary.getAllBackgrounds();
            }
            else
            {
                for (int i = 0; i < backgrounds.Count; i++)
                {
                    if (backgrounds[i].Version != Cache.Instance.GetLibrary.getVersionBackground(backgrounds[i].Id))
                    {
                        backgrounds[i] = Cache.Instance.GetLibrary.getBackground(backgrounds[i].Id);
                    }
                }
            }

        }

        public void getAllBackgrounds()
        {
            if(backgrounds.Count <= 1)
            {
                return;
            }
        }


        public static void saveCache()
        {
            if(!Directory.Exists(DIRECTORY))
            {
                Directory.CreateDirectory(DIRECTORY);
            }
            IFormatter formatter = new BinaryFormatter();

            Stream stream = new FileStream(
                DIRECTORY + "cache.dat", FileMode.Create,
                FileAccess.Write, FileShare.None);

            formatter.Serialize(stream, Instance);

            stream.Close();
        }

        public static void loadCache()
        {
            if (File.Exists(DIRECTORY + "cache.dat"))
            {
                IFormatter nformatter = new BinaryFormatter();

                Stream nstream = new FileStream(
                    DIRECTORY + "cache.dat", FileMode.Open,
                    FileAccess.Read, FileShare.Read);

                Instance = (Cache) nformatter.Deserialize(nstream);

                nstream.Close();
            }
            else
            {
                Instance.characters = Cache.Instance.GetLibrary.getAllCharacters();
            }
        }

        
    }
}
