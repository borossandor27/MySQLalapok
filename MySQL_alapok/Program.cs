using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace MySQL_alapok
{
    class Program
    {
        class Tanulo
        {
            int id;
            string nev;
            int magassag;

            public int Id { get => id; set => id = value; }
            public string Nev { get => nev; set => nev = value; }
            public int Magassag { get => magassag; set => magassag = value; }

            public Tanulo(string nev, int magassag)
            {
                Nev = nev;
                Magassag = magassag;
            }
        }
        static MySqlConnection con = null;
        static void Main(string[] args)
        {
            MySqlConnectionStringBuilder stringBuilder = new MySqlConnectionStringBuilder();
            stringBuilder.Server = "localhost";
            stringBuilder.UserID = "root";
            stringBuilder.Password = "";
            stringBuilder.CharacterSet = "utf8";
            con = new MySqlConnection(stringBuilder.ToString());
            try
            {
                con.Open();
                //-- Adatbázis létrehozása --------------
                MySqlCommand SQLcommand = con.CreateCommand();
                SQLcommand.CommandText = "CREATE DATABASE IF NOT EXISTS Tanulok CHARACTER SET utf8 COLLATE utf8_general_ci;";
                SQLcommand.ExecuteNonQuery();   //-- Létrehozza az adatbázist
                SQLcommand.CommandText = "USE Tanulok"; //-- Használatba vesszük
                SQLcommand.ExecuteNonQuery();
                //-- Adattabla letrehozasa -------------------------
                SQLcommand.CommandText = "CREATE TABLE IF NOT EXISTS `tanulok`.`Tanulok` (" +
                    " `id` INT NOT NULL AUTO_INCREMENT , " +
                    "`Nev` VARCHAR(50) NOT NULL , " +
                    "`Magassag` INT NOT NULL , " +
                    "PRIMARY KEY (`id`)) ENGINE = InnoDB;";

                //-- Adatok beszúrása a táblába ------------------------------------
                List<Tanulo> tanulok = new List<Tanulo>();
                tanulok.Add(new Tanulo("Dezső", 195));
                tanulok.Add(new Tanulo("Erzsébet", 168));
                tanulok.Add(new Tanulo("Antal", 173));
                foreach (Tanulo item in tanulok)
                {
                    MySqlCommand insert = new MySqlCommand("INSERT INTO `Tanulok` (`id`, `Nev`, `Magassag`) VALUES (NULL, @nev, @magassag);", con);
                    insert.Parameters.AddWithValue("@nev", item.Nev);
                    insert.Parameters.AddWithValue("@magassag", item.Magassag);
                    insert.ExecuteNonQuery();
                    //SQLcommand.
                }
                con.Close();
            }
            catch (MySqlException ex1)
            {
                switch (ex1.Number)
                {
                    case 1042:
                        //-- Unable to connect to any of the specified MySQL hosts. ------------
                        Console.WriteLine("Nem lehet csatlakozni a megadott MySQL-szolgáltatóhoz");
                        break;
                    case 0:
                        //-- Access denied for user --------------------------------------------
                        Console.WriteLine("Sikertelen felhasználó hitelesítés.");
                        break;
                    case 1044:
                        //--   Access denied for user --------------------------------------
                        Console.WriteLine("A felhasználó hozzáférése megtagadva!");
                        break;
                    default:
                        Console.WriteLine(ex1.Message);
                        break;
                }
            } catch(Exception ex2)
            {
                Console.WriteLine(ex2.Message);            
            }
 
            Console.WriteLine("Program vége!");
            Console.ReadKey();
        }
    }
}
