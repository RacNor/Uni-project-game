using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public class HighScoresJson
{
    public List<ScoreJson> data;
    [Serializable]
    public class ScoreJson
    {
        public int id;
        public string name;
        public int score;
        public string date;
    }
}


