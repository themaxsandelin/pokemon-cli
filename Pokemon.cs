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

	class Monsters
    {
        public static List<Pokemon> pokemons = new List<Pokemon>();
        public static List<Pokemon> randomList = new List<Pokemon>();
        public static List<Pokemon> areaPokemon;
        public static List<Pokemon> starters = new List<Pokemon>();
        public static int probability = 0;
        public static Random random = new Random();

        public static void LoadPokemon ()
        {
            var pokemonJson = File.ReadAllText("resources/pokemon/pokemon.json");
            pokemons = JsonConvert.DeserializeObject<List<Pokemon>>(pokemonJson);
            foreach (var pokemon in pokemons)
            {
                pokemon.LoadSprite();
                pokemon.LoadMoves();
            }
            starters = pokemons.Where(x => x.starter == true).ToList();
        }

        public static void LoadAreaPokemon (int group)
        {
            areaPokemon = pokemons.Where(x => x.group == group).ToList();

            foreach (var pokemon in areaPokemon)
            {
                for (int y = 1; y <= pokemon.rarity; y++)
                {
                    randomList.Add(pokemon);
                }
                probability += pokemon.rarity;
            }

            int emptyGrass = 100 - probability;

            for (int x = 1; x <= emptyGrass; x++)
            {
                var empty = new Pokemon();
                empty.name = "empty";
                randomList.Add(empty);
            }
        }

        public static void RandomizeEncounter()
        {
            var randomPokemon = randomList[random.Next(randomList.Count)];
            if (randomPokemon.name != "empty")
            {
                InitiateBattle(randomPokemon);
            }
        }

        public static void InitiateBattle (Pokemon wildPokemon)
        {
            Logic.inputType = 4;
            Console.SetCursorPosition(0,3);
            Console.WriteLine("|                                                                                |");
            Console.WriteLine("|--------------|-----|-------------|-----------|------------|                    |");
            Console.WriteLine("| Wild Pokemon | {0} | HP: {1}/{2} | Lv: {3}   | Type: {4}  |                    |");
            Console.WriteLine("|--------------|-----|-------------|-----------|------------|                    |");
            Console.WriteLine("|                                                                                |");
            wildPokemon.DrawSprite();
        }

    }

    class Pokemon
    {
        public string name;
        public string type;
        public string spriteFileName;
        public int rarity;
        public bool starter;

        public int hp;
        public int attack;
        public int defence;
        public int speed;
        public int level;
        
        public string attack1Name = "";
        public string attack2Name = "";
        public string attack3Name = "";
        public string attack4Name = "";

        public Move attack1;
        public Move attack2;
        public Move attack3;
        public Move attack4;
        
        public int group = 0;
        public List<string> sprite = new List<string>();

        public void LoadSprite ()
        {
            var line = "";
            System.IO.StreamReader file = new System.IO.StreamReader("resources/pokemon/"+spriteFileName);
            while ((line = file.ReadLine()) != null)
            {
                sprite.Add(line);
            }
            file.Close();
        }

        public void LoadMoves ()
        {
            if (attack1Name != "")
            {
                attack1 = Attacks.moves.FirstOrDefault(x => x.name == attack1Name);
            }
            if (attack2Name != "")
            {
                attack2 = Attacks.moves.FirstOrDefault(x => x.name == attack2Name);
            }
            if (attack3Name != "")
            {
                attack3 = Attacks.moves.FirstOrDefault(x => x.name == attack3Name);
            }
            if (attack4Name != "")
            {
                attack4 = Attacks.moves.FirstOrDefault(x => x.name == attack4Name);
            }
        }

        public void DrawSprite ()
        {
            foreach (string line in sprite)
            {
                Console.WriteLine(line);
            }
        }
    }

}