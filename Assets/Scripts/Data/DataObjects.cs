using System.Collections.Generic;

namespace Data.Objects
{
    /// <summary>
    /// This script will be used as the contianers to seralize and deserilize score data
    /// </summary>

    //Will be used as the data container that will and instance will be created from if player get's a high score, it is not directly to be saved but a list of these objects will be serialized as the high score
    [System.Serializable]
    public class PlayerScore
    {
        public string playeName;
        public int score;
        public int round;
    }

    [System.Serializable]
    public class HighScores
    {
        public List<PlayerScore> highscores;
    }

}