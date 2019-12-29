using System.Collections.Generic;
using System.Linq;

namespace SeeSaySign.Controls
{
    public static class SessionScores
    {
        public static Score SeeStage2Score { get; set; }
        public static Score SeeStage3Score { get; set; }

        public static void ResetStage2()
        {
            SeeStage2Score = new Score()
            {
                AllGameWords = WordManager.GetNouns().ToList().GetRandom(5).Shuffle(),
                Correct = new List<SightWord>(),
                Incorrect = new List<SightWord>()
            };
            foreach (SightWord allGameWord in SessionScores.SeeStage2Score.AllGameWords)
            {
                allGameWord.Enabled = false;
            }
            SeeStage2Score.AllGameWords.First().Enabled = true;
        }

        public static void ResetStage3()
        {
            SeeStage3Score = new Score()
            {
                AllGameWords = WordManager.GetNouns().ToList().GetRandom(5).Shuffle(),
                Correct = new List<SightWord>(),
                Incorrect = new List<SightWord>()
            };
            foreach (SightWord allGameWord in SessionScores.SeeStage3Score.AllGameWords)
            {
                allGameWord.Enabled = false;
            }
            SeeStage3Score.AllGameWords.First().Enabled = true;
        }

    }
}