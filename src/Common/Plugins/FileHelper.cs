using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GoldBoxExplorer.Lib.Plugins
{
    public static class FileHelper
    {
        public enum GameList
        {
            PoolOfRadiance,
            CurseOfTheAzureBonds,
            SecretOfTheSilverBlades,
            PoolsOfDarkness,
            ForgottenRealmsUnlimitedAdventures,
            GatewayToTheSavageFrontier,
            TreasuresOfTheSavageFrontier,
            NeverwinterNights,
            ChampionsOfKrynn,
            DeathKnightsOfKrynn,
            DarkQueenOfKyrnn,
            CountdownToDoomsday,
            MatrixCubed,
            Unknown,
        }
        /// <summary>
        /// Poor mans implementation to figure out what game the
        /// user is looking at. Uses the directory to "hunt" for specific
        /// files. Can't use file sizes because users might have modfied exes
        /// </summary>
        /// <param name="blockFileName"></param>
        /// <returns></returns>
        public static GameList DetermineGameFrom(string blockFileName)
        {
            var currentGame = GameList.Unknown;

            // establish the directory from the file
            var dir = Path.GetDirectoryName(blockFileName);
            if (dir == null) return currentGame;

            // find a key file in the directory to identify the game
            if (File.Exists(Path.Combine(dir, "POOL.CFG")))
            {
                currentGame = GameList.PoolOfRadiance;
            }
            else if (File.Exists(Path.Combine(dir, "POOL4.CFG")))
            {
                currentGame = GameList.PoolsOfDarkness;
            }
            else if (File.Exists(Path.Combine(dir, "BLADES.CFG")))
            {
                currentGame = GameList.SecretOfTheSilverBlades;
            }
            else if (File.Exists(Path.Combine(dir, "CURSE.CFG")))
            {
                currentGame = GameList.CurseOfTheAzureBonds;
            }
            else if (File.Exists(Path.Combine(dir, "BUCK.CFG")))
            {
                currentGame = GameList.CountdownToDoomsday;
            }
            else if (File.Exists(Path.Combine(dir, "MATRIX.CFG")))
            {
                currentGame = GameList.MatrixCubed;
            }
            else if (File.Exists(Path.Combine(dir, "KRYNN.CFG")))
            {
                currentGame = GameList.ChampionsOfKrynn;
            }
            else if (File.Exists(Path.Combine(dir, "TREASURE.CFG")))
            {
                currentGame = GameList.TreasuresOfTheSavageFrontier;
            }
            else if (File.Exists(Path.Combine(dir, "DKK.CFG")))
            {
                currentGame = GameList.DeathKnightsOfKrynn;
            }
            else if (File.Exists(Path.Combine(dir, "GAME.CFG")) &&
                File.Exists(Path.Combine(dir, "8X8D6.DAX")))
            {
                currentGame = GameList.GatewayToTheSavageFrontier;
            }
            else if (File.Exists(Path.Combine(dir, "GAME.CFG")) &&
                File.Exists(Path.Combine(dir, "CPIC.DAX")))
            {
                currentGame = GameList.NeverwinterNights;
            }

            return currentGame;
        }
    }
}
