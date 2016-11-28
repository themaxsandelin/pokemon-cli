using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PokemonTextAdventure
{

    class Field
    {
        // En maps lista som håller i alla kartor
        public static Dictionary<string, Map> maps = new Dictionary<string, Map>();

        public static void LoadMaps ()
        {
            // Directoryt där alla kartor ligger
            string mapsDirectory = "resources/maps/";
            
            // En variabel som håller i varje rad i kartan.
            string line;

            int count = 1;

            DirectoryInfo mapsDirectoryInfo = new DirectoryInfo(mapsDirectory);
            FileInfo[] mapsFiles = mapsDirectoryInfo.GetFiles("map*.txt");
            
            foreach (var mapFile in mapsFiles)
            {
                int i = 0;
                int positionX = 0;
                int positionY = 0;
             
                // Detta plockar ut X och Y positionen av kartan från .txt filens namn
                while ((i = mapFile.Name.IndexOf('(', i)) != -1)
                {
                    int stringEnd = (mapFile.Name.IndexOf(")", i) - i) - 1;
                    if (i == 3)
                    {
                        Int32.TryParse(mapFile.Name.Substring(i + 1, stringEnd), out positionX);
                    }
                    else
                    {
                        Int32.TryParse(mapFile.Name.Substring(i + 1, stringEnd), out positionY);
                    }
                    i++;
                }

                int rowAmount = 0;
                string[][] rows = new string[20][];

                // Creates a textreader so that I can pull out the text line by line
                System.IO.StreamReader file = new System.IO.StreamReader(mapsDirectory + mapFile);
                while ((line = file.ReadLine()) != null)
                {
                    // Byter ut space mot ett x för att kunna ta med karaktären i arrayen med split
                    line = line.Replace(' ', 'x');
                    string[] row = Regex.Split(line, "");
                    rows[rowAmount] = row;
                    rowAmount++;
                }
                file.Close();

                var mapObject = new Map();
                mapObject.positionX = positionX;
                mapObject.positionY = positionY;
                mapObject.map = rows;

                string mapName = "map" + count;
                maps.Add(mapName, mapObject);
                count++;
            }

        }

        // Metod som ritar ut hem skärmen med logotyp samt menyn
        public static void DrawHomeScreen()
        {
            Console.WriteLine("                                                                                  ");
            Console.WriteLine("                                                                                  ");
            Console.WriteLine("                                                                                  ");
            Console.WriteLine("                                                                                  ");
            Console.WriteLine("                                          .::.                                    ");
            Console.WriteLine("                                        .;:**'                                    ");
            Console.WriteLine("                                        `                                         ");
            Console.WriteLine("            .:XHHHHk.              db.   .;;.     dH  MX                          ");
            Console.WriteLine("          oMMMMMMMMMMM       ~MM  dMMP :MMMMMR   MMM  MR      ~MRMN               ");
            Console.WriteLine("          QMMMMMb  'MMX       MMMMMMP !MX' :M~   MMM MMM  .oo. XMMM 'MMM          ");
            Console.WriteLine("            `MMMM.  )M> :X!Hk. MMMM   XMM.o'  .  MMMMMMM X?XMMM MMM>!MMP          ");
            Console.WriteLine("             'MMMb.dM! XM M'?M MMMMMX.`MMMMMMMM~ MM MMM XM `' MX MMXXMM           ");
            Console.WriteLine("              ~MMMMM~ XMM. .XM XM`'MMMb.~*?**~ .MMX M t MMbooMM XMMMMMP           ");
            Console.WriteLine("               ?MMM>  YMMMMMM! MM   `?MMRb.    `'''   !L'MMMMM XM IMMM            ");
            Console.WriteLine("                MMMX   'MMMM'  MM       ~%:           !Mh.''' dMI IMMP            ");
            Console.WriteLine("                'MMM.                                             IMX             ");
            Console.WriteLine("                 ~M!M                                             IMP             ");
            Console.WriteLine("                                                                                  ");
            Console.WriteLine("                                                                                  ");
            Console.WriteLine("                                                                                  ");
            Console.WriteLine("                    >Continue<         New game          Exit                     ");
            Console.WriteLine("                                                                                  ");
            Console.WriteLine("                                                                                  ");
            Console.WriteLine("                                                                                  ");
            Console.WriteLine("                                                                                  ");
            Console.WriteLine("                                                                                  ");
        }

        public static void RunIntro()
        {
            Console.Clear();
            Console.WriteLine("                                                                                  ");
            Console.WriteLine("                                                                                  ");
            Console.WriteLine("                     |||----------------------------------|||                     ");
            Console.WriteLine("                     |||       A new journey begins       |||                     ");
            Console.WriteLine("                     |||----------------------------------|||                     ");
            Console.WriteLine("                                                                                  ");
            Console.WriteLine("                                                                                  ");
            Console.WriteLine("Hello there! Welcome to the world of POKEMON!                                     ");
            Console.WriteLine("My name is OAK! People call me the POKEMON PROF!                                  ");
            Console.WriteLine("                                                                                  ");
            Console.WriteLine("This world is inhabited by creatures called POKEMON!                              ");
            Console.WriteLine("For some people POKEMON are pets. Others use them to fight.                       ");
            Console.WriteLine("Myself...I study POKEMON as a profession.                                         ");
            Console.WriteLine("                                                                                  ");
            Console.WriteLine("First you will choose your companion POKEMON to be at your side on your journey.  ");
            Console.WriteLine("Then you will head out into the world to be a POKEMON MASTER!                     ");
            Console.WriteLine("                                                                                  ");
            Console.WriteLine("Your very own POKEMON legend is about to unfold!                                  ");
            Console.WriteLine("A world of dreams and adventures with POKEMON awaits! Let's go!                   ");
            Console.WriteLine("                                                                                  ");
            Console.WriteLine("                                                                                  ");
            Console.WriteLine("                                                                                  ");
            Console.WriteLine("                        Press ENTER to start your journey.                        ");
        }

        public static void ShowStarters()
        {
            Console.Clear();
            List<Pokemon> starters = new List<Pokemon>();
            starters = Monsters.pokemons.Where(x => x.starter == true).ToList();

            Console.WriteLine("                                                                                  ");
            Console.WriteLine("Choose your POKEMON!");
            Console.WriteLine("                                                                                  ");
            Console.WriteLine("   |------------|-------------|-----------|-------------|------------------------|");
            Console.WriteLine("   |     1      | HP:  20/20  | Atk: 8    | Lv: 5       | Type: Grass            |");
            Console.WriteLine("   |------------|-------------|-----------|-------------|------------------------|");
            Console.WriteLine("   | BULBASAUR  | Def: 3      | Spd: 15   | Exp: 500    | High Speed             |");
            Console.WriteLine("   |------------|-------------|-----------|-------------|------------------------|");
            Console.WriteLine("                                                                                  ");
            Console.WriteLine("                                                                                  ");
            Console.WriteLine("   |------------|-------------|-----------|-------------|------------------------|");
            Console.WriteLine("   |     2      | HP:  20/20  | Atk: 10   | Lv: 5       | Type: Fire             |");
            Console.WriteLine("   |------------|-------------|-----------|-------------|------------------------|");
            Console.WriteLine("   | CHARMANDER | Def: 3      | Spd: 10   | Exp: 500    | High Attack            |");
            Console.WriteLine("   |------------|-------------|-----------|-------------|------------------------|");
            Console.WriteLine("                                                                                  ");
            Console.WriteLine("                                                                                  ");
            Console.WriteLine("   |------------|-------------|-----------|-------------|------------------------|");
            Console.WriteLine("   |     3      | HP:  20/20  | Atk: 8    | Lv: 5       | Type: Water            |");
            Console.WriteLine("   |------------|-------------|-----------|-------------|------------------------|");
            Console.WriteLine("   | SQUIRTLE   | Def: 4      | Spd: 10   | Exp: 500    | High Defence           |");
            Console.WriteLine("   |------------|-------------|-----------|-------------|------------------------|");
            Console.WriteLine("                                                                                  ");
            Console.WriteLine("                                                                                  ");
            Console.WriteLine("   Use W and S to choose between the starters and then press ENTER.");
            
            Console.SetCursorPosition(1, 6);
            Console.Write(">");
        }

        // Metod som ritar ut spelfältet både övre och undre menyn och även kartan i mitten
        public static void DrawField(int mapX, int mapY, int px, int py)
        {
            // Rensar ut konsollen för att kunna printa spelfältet.
            Console.Clear();

            // Sätter den aktuella kartan til den kartan som skall renderas 
            Logic.loadedMap = Field.maps.FirstOrDefault(x => x.Value.positionX == mapX && x.Value.positionY == mapY).Value;

            // Ritar ut den övre ramen med titel samt versionsnummer
            Console.WriteLine("|--------------------------------------------------------------------------------|");
            Console.WriteLine("| Pokémon Text Adventure | By Max Sandelin | Version 1.0.0                       |");
            Console.WriteLine("|--------------------------------------------------------------------------------|");

            // Ritar ut kartan (Start kartan som är satt i mapX + mapY)
            Logic.loadedMap.DrawMap(px, py);

            // Ritar ut den undre ramen med mindre instruktioner för styrning och hur man stänger av spelet
            Console.WriteLine("|---------------|---------|-----------|------------------------------------------|");
            Console.WriteLine("| Move: W,A,S,D | Save: E | Quit: ESC |                                          |");
            Console.WriteLine("|---------------|---------|-----------|------------------------------------------|");
        }

    }

    // En map class för att skapa objekt av varje karta med kordinater i världen och kartans layout och namn
    class Map
    {
        public int positionX;
        public int positionY;
        public int pokemonGroup = 1;
        public string[][] map = new string[20][];

        // Skriv en funktion som printar ut kartan
        public void DrawMap (int playerX, int playerY)
        {
            string line;
            int rowC = 0;
            Console.SetCursorPosition(0, 3);
            foreach (var rowArray in map)
            {
                line = "";
                int i = 0;
                string character;

                foreach (string row in rowArray)
                {
                    character = row;
                    if (rowC == playerY && i == playerX)
                    {
                        character = "A";
                    }
                    else
                    {
                        if (character == "x")
                        {
                            character = " ";
                        }
                    }
                    line += character;
                    i++;
                }
                Console.WriteLine(line);
                rowC++;
            }
        }

    }

}