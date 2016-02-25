using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TappersWPF
{
    [Serializable]
    public class Library
    {

        private List<Contact> contacts = new List<Contact>();
        public List<Contact> Contacts
        {
            get { return contacts; }
            set { contacts = value; }
        }

        public List<Contact> getYourContacts()
        {
            contacts = db.getAllContacts();
            return contacts;
        }


        public Contact getContactForID(int id)
        {
            foreach(Contact con in contacts)
            {
                if(con.Id == id)
                {
                    return con;
                }
            }
            return null;
        }

        public void addContact(Contact contact)
        {
            contacts.Add(contact);
        }

        public void syncContacts()
        {
            foreach (Contact c in contacts)
            {
                insetContact(c);
            }
            
        }

        public void deleteAllContacts()
        {
            db.deleteContacts();
        }

        public void insetContact(Contact contact)
        {
            db.insertContact(contact);
        }

        [NonSerialized]
        private DBConnection db = new DBConnection();

        public Library() 
        {
            db = new DBConnection();
        }

        public bool attemptLogin(string user, string pass)
        {
            if(db.attemptLogin(user, pass))
            {
                return true;
            }

            return false;
        }

        public Character donwloadContact(int id)
        {
            return db.getCharacter(id);
        }

        public List<Background> getAllBackgrounds()
        {
            return db.getAllBackgrounds();
        }



        public int getVersionCharacter(int id)
        {
            return db.getVersionCharacter(id);
        }

        public int getVersionBackground(int id)
        {
            return db.getVersionBackground(id);
        }

        public Character getCharacter(int id)
        {
            return db.getCharacter(id);
        }

        public Background getBackground(int id)
        {
            return db.getBackground(id);
        }

        public List<Character> getAllCharacters()
        {
            return db.getAllCharacters();
        }

        public int getCharacterCount()
        {
            if(db == null)
            {
                db = new DBConnection();
            }
            return db.characterCount();
        }

        public int getBackgroundCount()
        {
            return db.backgroundCount();
        }



        /// <summary>
        /// Private connection class
        /// </summary>
        private class DBConnection
        {
            private string ConnString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=..\..\Tappers.accdb;Persist Security Info=False";

            private OleDbConnection conn;
            private OleDbCommand cmd;

            public DBConnection()
            {
                try
                {
                    conn = new OleDbConnection(ConnString);
                    cmd = conn.CreateCommand();
                }
                catch
                {
                    throw new Exception("non");
                }
            }



            public int characterCount()
            {
                try
                {
                    int counter = 0;
                    cmd.CommandText = "Select * From Characters";
                    cmd.CommandType = CommandType.Text;

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    OleDbDataReader reader = cmd.ExecuteReader();


                    while (reader.Read())
                    {
                        counter++;
                    }

                    return counter;
                }
                catch{ throw; }
                finally
                {
                    if (conn != null)
                    {
                        conn.Close();
                    }
                }
            }

            public int backgroundCount()
            {
                try
                {
                    int counter = 0;
                    cmd.CommandText = "Select * From Backgrounds";
                    cmd.CommandType = CommandType.Text;

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    OleDbDataReader reader = cmd.ExecuteReader();


                    while (reader.Read())
                    {
                        counter++;
                    }

                    return counter;
                }
                catch { throw; }
                finally
                {
                    if (conn != null)
                    {
                        conn.Close();
                    }
                }
            }

            public List<Contact> getAllContacts()
            {
                try
                {
                    List<Contact> list = new List<Contact>();
                    cmd.CommandText = "Select * From Contact Where LoginID=" + Account.yourID;
                    cmd.CommandType = CommandType.Text;

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    OleDbDataReader reader = cmd.ExecuteReader();


                    while (reader.Read())
                    {
                        Contact character = new Contact(0, "", 0, 0, null, Account.yourID);
                        character.Id = Convert.ToInt32(reader["ContactID"].ToString());
                        character.Character = Convert.ToInt32(reader["CharacterID"].ToString());
                        character.BackgroundColour = Convert.ToInt32(reader["BackgroundID"].ToString());
                        character.Name = reader["Name"].ToString();
                        character.Date = reader["DateCreated"].ToString();

                        list.Add(character);
                    }

                    return list;
                }
                catch { throw; }
                finally
                {
                    if (conn != null)
                    {
                        conn.Close();
                    }
                }
            }

            private Bitmap bitmap;

            public List<Character> getAllCharacters()
            {
                try
                {
                    List<Character> list = new List<Character>();
                    cmd.CommandText = "Select * From Characters";
                    cmd.CommandType = CommandType.Text;

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    OleDbDataReader reader = cmd.ExecuteReader();


                    while (reader.Read())
                    {
                        Character character = new Character(1, "", null, null, 0);
                        character.Id = Convert.ToInt32(reader["CharacterID"].ToString());
                        character.Name = reader["Name"].ToString();
                        character.LargeImage = reader["LargeImage"].ToString();
                        character.SmallImage = reader["SmallImage"].ToString();
                        character.Version = Convert.ToInt32(reader["Version"].ToString());
                        using (var client = new WebClient())
                        {
                            if (!Directory.Exists(Cache.DIRECTORY + "/images/"))
                                Directory.CreateDirectory(Cache.DIRECTORY + "/images/");
                            try
                            {
                                client.DownloadFile(
                                    character.LargeImage,
                                     Cache.DIRECTORY + "/images/" + character.Name + "_large.png");
                            }
                            catch { }
                            try
                            {
                                client.DownloadFile(
                                    character.SmallImage,
                                     Cache.DIRECTORY + "/images/" + character.Name + "_small.png");
                            }
                            catch { }
                        }

                        list.Add(character);
                    }

                    return list;
                }
                catch { throw; }
                finally
                {
                    if (conn != null)
                    {
                        conn.Close();
                    }
                }
                
            }

            bool contactContains(Contact contact)
            {
                try
                {
                    cmd.CommandText = "Select ContactID From Contact";
                    cmd.CommandType = CommandType.Text;

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    OleDbDataReader reader = cmd.ExecuteReader();

                    bool found = false;
                    while (reader.Read())
                    {
                        found = contact.Id == Convert.ToInt32(reader["ContactID"].ToString());
                    }

                    return found;
                }
                catch { throw; }
                finally
                {
                    if (conn != null)
                    {
                        conn.Close();
                    }
                }
            }

            internal void deleteContacts()
            {
                try
                {
                    cmd.CommandText = "Delete * From Contact Where LoginID= " + Account.yourID;
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();

                }
                catch { throw; }
                finally
                {
                    if (conn != null)
                    {
                        conn.Close();
                    }
                }
            }

            internal void insertContact(Contact st)
            {
                try
                {

                    cmd.CommandText = "INSERT INTO Contact (ContactID,LoginID,Name,CharacterID,BackgroundID,DateCreated) VALUES ('"
                        + st.Id + "', '" + Account.yourID + "', '" + st.Name + "', '" + st.Character + "', '" + st.BackgroundColour + "', '" + st.Date + "')";
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    

                }
                catch
                {
                    throw;
                }
                finally
                {
                    //make sure connection is closed
                    if (conn != null)
                    {
                        conn.Close();
                    }
                }
            }

            public List<Background> getAllBackgrounds()
            {
                try
                {
                    List<Background> list = new List<Background>();
                    cmd.CommandText = "Select * From Backgrounds";
                    cmd.CommandType = CommandType.Text;

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    OleDbDataReader reader = cmd.ExecuteReader();


                    while (reader.Read())
                    {
                        Background background = new Background(1, "", "", "", 0);
                        background.Id = Convert.ToInt32(reader["BackgroundID"].ToString());
                        background.Name = reader["Name"].ToString();
                        background.PrimaryColour = reader["PrimaryColour"].ToString();
                        background.SecondaryColour = reader["SecondaryColour"].ToString();
                        background.Version = Convert.ToInt32(reader["Version"].ToString());
                        list.Add(background);
                    }

                    return list;
                }
                catch { throw; }
                finally
                {
                    if (conn != null)
                    {
                        conn.Close();
                    }
                }

            }

            public Character getCharacter(int id)
            {
                try
                {
                    Character character = new Character(1, "", null, null, 0);
                    cmd.CommandText = "Select * FROM Characters Where CharacterID=" + id;
                    cmd.CommandType = CommandType.Text;
                    
                    conn.Open();
                    cmd.ExecuteNonQuery();

                    OleDbDataReader reader = cmd.ExecuteReader();
                    
                    while (reader.Read())
                    {
                        character.Id = Convert.ToInt32(reader["CharacterID"].ToString());
                        character.Name = reader["Name"].ToString();
                        character.Version = Convert.ToInt32(reader["Version"].ToString());
                        character.LargeImage = reader["LargeImage"].ToString();
                        using (var client = new WebClient())
                        {
                            if (!Directory.Exists(Cache.DIRECTORY + "/images/"))
                                Directory.CreateDirectory(Cache.DIRECTORY + "/images/");
                            try
                            {
                                client.DownloadFile(
                                    character.LargeImage,
                                     Cache.DIRECTORY + "/images/" + character.Name + "_large.png");
                            }
                            catch { }
                            try
                            {
                                client.DownloadFile(
                                    character.SmallImage,
                                     Cache.DIRECTORY + "/images/" + character.Name + "_small.png");
                            }
                            catch { }
                        }
                    }

                    return character;
                }
                catch { throw; }
                finally
                {
                    if (conn != null)
                    {
                        conn.Close();
                    }
                }
            }

            public Background getBackground(int id)
            {
                try
                {
                    Background background = new Background(1, "", "", "", 0);
                    cmd.CommandText = "Select * FROM Backgrounds Where BackgroundID =" + id;
                    cmd.CommandType = CommandType.Text;

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    OleDbDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        background.Id = Convert.ToInt32(reader["BackgroundID"].ToString());
                        background.Name = reader["Name"].ToString();
                        background.PrimaryColour = reader["PrimaryColour"].ToString();
                        background.SecondaryColour = reader["SecondaryColour"].ToString();
                        background.Version = Convert.ToInt32(reader["Version"].ToString());
                    }

                    return background;
                }
                catch { throw; }
                finally
                {
                    if (conn != null)
                    {
                        conn.Close();
                    }
                }
            }

            public int getVersionCharacter(int checkID)
            {
                try
                {
                    cmd.CommandText = "SELECT Version FROM Characters WHERE CharacterID =" + checkID;
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    cmd.ExecuteNonQuery();

                    OleDbDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        return Convert.ToInt32(reader["Version"].ToString());
                    }
                }
                catch { throw; }
                finally
                {
                    if (conn != null)
                    {
                        conn.Close();
                    }
                }

                return 0;

            }

            public int getVersionBackground(int checkID)
            {
                try
                {
                    cmd.CommandText = "SELECT Version FROM Backgrounds WHERE BackgroundID =" + checkID;
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    cmd.ExecuteNonQuery();

                    OleDbDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        return Convert.ToInt32(reader["Version"].ToString());
                    }
                }
                catch { throw; }
                finally
                {
                    if (conn != null)
                    {
                        conn.Close();
                    }
                }

                return 0;

            }

            public bool attemptLogin(string name, string pass)
            {
                try
                {
                    cmd.CommandText = "Select * From Login Where Username='" + name + "' AND Password='" + pass + "'";
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    cmd.ExecuteNonQuery();

                    bool found = false;

                    OleDbDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Account.yourID = Convert.ToInt32(reader["UserID"].ToString());
                        found = reader["Username"].ToString().Length >= 1;
                        
                    }

                    return found;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (conn != null)
                    {
                        conn.Close();
                    }
                }
            }




        }//End of ConnectionClass


    }//End of Library Class

    
}
