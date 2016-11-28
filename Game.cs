using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PokemonTextAdventure;

namespace PokemonTextAdventure
{
    class Game
    {
        static void Main(string[] args)
        {
            Logic.Start();
            
        }

    }

    // Huvudklassen för spelet som innehåller den mesta logiken i spelet
    class Logic
    { 
        // Minimum och max värden på spelarens position i kartan i X-led (horisontellt)
        public static int minX = 2;
        public static int maxX = 81;

        // Minimum och max värden på spelarens position i kartan i Y-led (vertikalt)
        public static int minY = 0;
        public static int maxY = 19;

        // Spelarens initiala position på kartan
        public static int px = 75;
        public static int py = 7;

        // Vilken karta som skall genereras
        public static int mapX = 1;
        public static int mapY = 1;

        // Ett objekt för att hålla reda på den karta som just nu är renderad i spelet
        public static Map loadedMap = new Map();

        // En vool variabel för att kunna stänga av input om så önskas
        public static bool getInput = true;

        // En int variabel som håller koll på vad för input som skall köras, 0 = start input (körs på start skärmen), 1 = standard input (körs i spelet)
        public static int inputType = 0;

        // En int variabel som håller koll på vilket menu item som är valt (0 = Continue, 1 = New game, 2 = Exit)
        public static int selectedMenuItem = 0;

        // En int variabel som håller koll på vilken starter använder väljer
        public static int chosenStarter = 0;

        // Ett Player objekt som är baserat på spelaren med dess pokemon
        public static Player player;

        // Start funktionen för att starta spelet
        public static void Start()
        {
            Console.SetWindowSize(82, 27);
            Console.CursorVisible = false;

            // Metod som laddar in alla attacker från en JSON fil
            Attacks.LoadMoves();

            // Metod som laddar in alla Pokemon från JSON fil och laddar in deras sprites samt deras attacker
            Monsters.LoadPokemon();

            // Hämtar alla kartor
            Field.LoadMaps();

            // Laddar in alla pokemon i den karta som spelaren är i
            Monsters.LoadAreaPokemon(loadedMap.pokemonGroup);

            // Ritar ut startskärmen
            Field.DrawHomeScreen();

            // En loop för att hålla igång spelet tills det att spelaren stänger ner spelet med ex. ESC tangenten
            while (true)
            {
                ReadUserActions();
            }
        }

        // Metod som lyssnar efter användarens tangenttryck (keystroke) och skickar vidare tangentens nyckel till en annan metod
        public static void ReadUserActions ()
        {
            // Plockar ut nyckeln från keystroke.
            var key = Console.ReadKey(true).KeyChar;

            // Kollar om man vill ta emot input i spelet.
            if (getInput)
            {
                if (key == 27)
                {
                    // Spelaren trycker på ESC tangenten för att stänga ner spelet
                    Console.SetCursorPosition(0,26);
                    Console.CursorVisible = true;
                    Environment.Exit(0);
                }
                else
                {
                    if (inputType == 0)
                    {
                        // Input som sker i start skärmen
                        SelectMenuItem(key);
                    }
                    else if (inputType == 1)
                    {
                        // Input som sker när man är inne i spelet
                        if (key == 101)
                        {
                            SaveGame();
                        }
                        else
                        {
                            SetPlayerPosition(key);
                        }
                    }
                    else if (inputType == 2)
                    {
                        // Input som sker när man har startat ett nytt spel
                        if (key == 13)
                        {
                            inputType = 3;
                            Field.ShowStarters();
                        }
                    }
                    else if (inputType == 3)
                    {
                        SelectStarter(key);
                    }
                    else if (inputType == 4)
                    {

                    }
                }
            }
        }

        // Metod som hanterar user input vid startskärmen
        public static void SelectMenuItem(int key)
        {
            if (key == 97)
            {
                // Spelaren trycker på A tangenten för att byta menu item åt vänster
                if (selectedMenuItem == 1)
                {
                    // Körs om man står på "New game" menu item i start skärmen
                    selectedMenuItem = 0;

                    Console.SetCursorPosition(38, 20);
                    Console.Write(" ");
                    Console.SetCursorPosition(47, 20);
                    Console.Write(" ");

                    Console.SetCursorPosition(20, 20);
                    Console.Write(">");
                    Console.SetCursorPosition(29, 20);
                    Console.Write("<");
                }
                else if (selectedMenuItem == 2)
                {
                    // Körs om man står på "Exit" menu item i start skärmen
                    selectedMenuItem = 1;

                    Console.SetCursorPosition(56, 20);
                    Console.Write(" ");
                    Console.SetCursorPosition(61, 20);
                    Console.Write(" ");

                    Console.SetCursorPosition(38, 20);
                    Console.Write(">");
                    Console.SetCursorPosition(47, 20);
                    Console.Write("<");
                }
            }
            else if (key == 100)
            {
                // Spelaren trycker på D tangenten för att byta menu item åt höger
                if (selectedMenuItem == 0)
                {
                    // Körs om man står på "Continue" menu item i start skärmen
                    selectedMenuItem = 1;
                    
                    Console.SetCursorPosition(20, 20);
                    Console.Write(" ");
                    Console.SetCursorPosition(29, 20);
                    Console.Write(" ");

                    Console.SetCursorPosition(38, 20);
                    Console.Write(">");
                    Console.SetCursorPosition(47, 20);
                    Console.Write("<");
                }
                else if (selectedMenuItem == 1) {
                    // Körs om man står på "New game" menu item i start skärmen

                    selectedMenuItem = 2;

                    Console.SetCursorPosition(38, 20);
                    Console.Write(" ");
                    Console.SetCursorPosition(47, 20);
                    Console.Write(" ");

                    Console.SetCursorPosition(56, 20);
                    Console.Write(">");
                    Console.SetCursorPosition(61, 20);
                    Console.Write("<");
                }
            }
            else if (key == 13)
            {
                // Spelaren trycker på ENTER tangenten för att välja ett utav de menu items på start skärmen
                if (selectedMenuItem == 0)
                {
                    // Körs om det valda menu item var "Continue"
                    if (File.Exists("resources/pokemon/save.json"))
                    {
                        LoadSave();
                        inputType = 1;
                        Field.DrawField(mapX, mapY, px, py);
                    }
                }
                else if (selectedMenuItem == 1)
                {
                    // Körs om det valda menu item var "New game"

                    // Här skall new game (intro liknande historia) köras
                    inputType = 2;
                    Field.RunIntro();
                }
                else if (selectedMenuItem == 2)
                {
                    // Körs om det valda menu item var "Exit"
                    Console.SetCursorPosition(0, 26);
                    Console.CursorVisible = true;
                    Environment.Exit(0);
                }
            }
        }

        public static void SelectStarter (int key)
        {
            Console.SetCursorPosition(0, 20);
            if (key == 13)
            {
                // Spelare trycker ner ENTER tangenten
                inputType = 1;
                player = new Player();
                player.starter = Monsters.starters[chosenStarter];
                Field.DrawField(mapX, mapY, px, py);
            }
            else if (key == 119) 
            {
                // Spelaren trycker ner W tangenten
                if (chosenStarter == 1) 
                {
                    // Spelaren går från starter nr.2 tll nr.1
                    chosenStarter = 0;
                    Console.SetCursorPosition(1, 13);
                    Console.Write(" ");
                    Console.SetCursorPosition(1, 6);
                    Console.Write(">");
                }
                else if (chosenStarter == 2)
                {
                    // Spelaren går från starter nr.3 till nr.2
                    chosenStarter = 1;
                    Console.SetCursorPosition(1, 20);
                    Console.Write(" ");
                    Console.SetCursorPosition(1, 13);
                    Console.Write(">");
                }
            }
            else if (key == 115)
            {
                // Spelaren trycker ner S tangenten
                if (chosenStarter == 0)
                {
                    // Spelaren går från starter nr.1 till nr.2
                    chosenStarter = 1;
                    Console.SetCursorPosition(1, 6);
                    Console.Write(" ");
                    Console.SetCursorPosition(1, 13);
                    Console.Write(">");
                }
                else if (chosenStarter == 1)
                {
                    // Spelaren går från starter nr.2 till nr.3
                    chosenStarter = 2;
                    Console.SetCursorPosition(1, 13);
                    Console.Write(" ");
                    Console.SetCursorPosition(1, 20);
                    Console.Write(">");
                }
            }
        }

        // Metod som tar emot nyckelvärdet på tangenten spelaren tryckte på för att utföra metoder beroende på tangentens nyckel
        public static void SetPlayerPosition(int key)
        {
            int playerX = 0;
            int playerY = 0;
            if (key == 119)
            {
                // Spelaren trycker på W tangenten för att röra sig uppåt
                playerX = px;
                playerY = py - 1;
                if (py == minY)
                {
                    mapY -= 1;
                    px = playerX;
                    py = maxY;
                    loadedMap = Field.maps.FirstOrDefault(x => x.Value.positionX == mapX && x.Value.positionY == mapY).Value;
                    loadedMap.DrawMap(px, py);
                }
                else
                {
                    MovePlayer(playerX, playerY);
                }
            }
            else if (key == 97)
            {
                // Spelaren trycker på A tangenten för att röra sig åt vänster
                playerX = px - 1;
                playerY = py;
                if (px == minX)
                {
                    mapX -= 1;
                    px = maxX;
                    py = playerY;
                    loadedMap = Field.maps.FirstOrDefault(x => x.Value.positionX == mapX && x.Value.positionY == mapY).Value;
                    loadedMap.DrawMap(px, py);
                }
                else
                {
                    MovePlayer(playerX, playerY);
                }
            }
            else if (key == 115)
            {
                // Spelaren trycker på S tangenten för att röra sig ner åt
                playerX = px;
                playerY = py + 1;
                if (py == maxY)
                {
                    mapY += 1;
                    px = playerX;
                    py = minY;
                    loadedMap = Field.maps.FirstOrDefault(x => x.Value.positionX == mapX && x.Value.positionY == mapY).Value;
                    loadedMap.DrawMap(px, py);
                }
                else
                {
                    MovePlayer(playerX, playerY);
                }
            }
            else if (key == 100)
            {
                // Spelaren trycker på D tangenten för att röra sig åt höger
                playerX = px + 1;
                playerY = py;
                if (px == maxX)
                {
                    mapX += 1;
                    px = minX;
                    py = playerY;
                    loadedMap = Field.maps.FirstOrDefault(x => x.Value.positionX == mapX && x.Value.positionY == mapY).Value;
                    loadedMap.DrawMap(px, py);
                }
                else
                {
                    MovePlayer(playerX, playerY);
                }
            }
        }

        
        // Metod för att flytta spelaren till sin nya position (om han inte går på ett "#" tecken) och även kolla om spelaren går i gräs för att då slumpa en ny match
        public static void MovePlayer (int playerX, int playerY)
        {
            // En variabel som hämtar vilken typ av karaktär som karaktären går mot
            var getChar = loadedMap.map[playerY][playerX];
            if (getChar != "#")
            {
                int cursorX = px - 1;
                int cursorY = py + 3;
                Console.SetCursorPosition(cursorX, cursorY);
                string oldChar = loadedMap.map[py][px];
                if (oldChar == "x")
                {
                    oldChar = " ";
                }
                Console.Write(oldChar);
                
                px = playerX;
                py = playerY;
                cursorX = px - 1;
                cursorY = py + 3;
                Console.SetCursorPosition(cursorX, cursorY);
                Console.Write("A");
                
                string newchar = loadedMap.map[py][px];
                if (newchar == "'")
                {
                    // Kollar om spelaren precis gick genom gräs
                    // Monsters.RandomizeEncounter();
                }
            }
        }

        public static void SaveGame ()
        {
            var save = new Save();
            save.mapX = mapX;
            save.mapY = mapY;
            save.px = px;
            save.py = py;
            save.player = player;

            string jsonSave = JsonConvert.SerializeObject(save);
            string path = "resources/pokemon/save.json";
            File.WriteAllText(path, jsonSave);
        }

        public static void LoadSave ()
        {
            if (File.Exists("resources/pokemon/save.json"))
            {
                var pokemonJson = File.ReadAllText("resources/pokemon/save.json");
                var save = JsonConvert.DeserializeObject<Save>(pokemonJson);
                mapX = save.mapX;
                mapY = save.mapY;
                px = save.px;
                py = save.py;
            }
        }
    }

    class Save
    {
        public int mapX;
        public int mapY;
        public int px;
        public int py;
        public Player player;
    }

    class Player
    {
        public Pokemon starter;
    }
}
