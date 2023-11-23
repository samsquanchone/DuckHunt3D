
using System.Collections.Generic;
using System.Linq;
using System;
using Data.JSON;
using Data.Objects;

namespace Data.Persistent
{

    /// <summary>
    /// Will be used to load file and set up scores in a container on start of game (menu), so there is no need to worry about deserialization at a more time critical point!
    /// Will also be used as a means to pass game result data to the scoreboard scene
    /// </summary>
    public static class PersistentData
    {
        private static HighScores file = new();
        private static List<PlayerScore> highScoresList = new();
        private static int finalScore;
        private static int finalRound;


        static PersistentData()
        {
            if (JSONManager.CheckIfSaveExits())
            {
                InitialiseHighScores();
            }
        }
        public static void InitialiseHighScores()
        {

            file = JSONManager.LoadData();

            foreach (var score in file.highscores)
            {

                highScoresList.Add(score);

            }
        }

        public static void AddAndSortHighScores(PlayerScore newScore)
        {
            //Add new score to list, then sort and reverse (ascending to descending) remove any elements past our size of 10 
            highScoresList.Add(newScore);

            //Bit of linq to be able to sort a list of objects, i.e playerscore by scores. Will be sorted into descending
            IEnumerable<PlayerScore> query = highScoresList.OrderByDescending(player => player.score);


            //Revert query back to the list!
            highScoresList = query.ToList();

            if (highScoresList.Count > 10) //Theoretically should not need this as how I plan to only add if score above max element score, but better safe than accessing an out of bounds element!
            {
                highScoresList.RemoveAt(10);
            }

            //Save changes to file
            file.highscores = highScoresList;
            JSONManager.SaveData(file);
        }

        public static List<PlayerScore> GetHighScores()
        {
            return highScoresList;
        }

        public static HighScores GetFile()
        {
            return file;
        }

        public static void SetGameResults(int score, int round)
        {
            finalScore = score;
            finalRound = round;
        }
        public static Tuple<int, int> GetGameFinishedStats()
        {
            Tuple<int, int> results = new(finalScore, finalRound);

            return results;
        }

    }
}