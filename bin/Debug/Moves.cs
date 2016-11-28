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
    class Attacks
    {
        public static List<Move> moves = new List<Move>();

        public static void LoadMoves ()
        {
            var movesJson = File.ReadAllText("resources/pokemon/moves.json");
            moves = JsonConvert.DeserializeObject<List<Move>>(movesJson);
        }
    }

    class Move
    {
        public string name;
        public string type;
        public int effect;
        public int power;
        public int pp;
    }
}
