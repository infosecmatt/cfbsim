using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFBSim
{
    public class Game
    {
        public Team homeTeam;
        public Team awayTeam;
        private int homeTeamScore;
        private int awayTeamScore;
        private string winner;

        public Game(Team aHomeTeam, Team aAwayTeam)
        {
            homeTeam = aHomeTeam;
            awayTeam = aAwayTeam;
            homeTeamScore = GetTeamScore(homeTeam, awayTeam);
            awayTeamScore = GetTeamScore(awayTeam, homeTeam);
            winner = GetWinner();
        }
        public int HomeTeamScore
        {
            get { return homeTeamScore; }
        }
        public int AwayTeamScore
        {
            get { return awayTeamScore; }
        }
        public string Winner
        { get { return winner; } }
        private string GetWinner()
        {
            string winner = string.Empty;
            if (HomeTeamScore > AwayTeamScore)
            {
                winner = homeTeam.uniName;
            }
            else if (AwayTeamScore > HomeTeamScore)
            {
                winner = awayTeam.uniName;
            }
            else
            {
                winner = "tie";
            }
            return winner;
        }
        private int GetTeamScore(Team scoringTeam, Team opponent)
        {
            double points = (scoringTeam.OffAbility / opponent.DefAbility) * 30;
            double randPoints = 0;
            do
            {
                Random rand = new Random(); //reuse this if you are generating many
                double u1 = 1.0 - rand.NextDouble(); //uniform(0,1] random doubles
                double u2 = 1.0 - rand.NextDouble();
                double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
                randPoints = points + (points * .33) * randStdNormal; //random normal(mean,stdDev^2)
            }
            while (randPoints < 0);
            

            int rawScore = (int)Math.Round(randPoints);
            return FootballizeScore(rawScore);
        }
        public string GetFinalScore()
        {
            int homePoints = GetTeamScore(homeTeam, awayTeam);
            int awayPoints = GetTeamScore(awayTeam, homeTeam);
            //no ties allowed
            while (homePoints == awayPoints)
            {
                homePoints = GetTeamScore(homeTeam, awayTeam);
            }
            string finalScore = $"{homeTeam.teamShorthand} {homePoints} - {awayTeam.teamShorthand} {awayPoints}";
            return finalScore;

        }

        private int FootballizeScore(int rawScore)
        {
            int preScore = rawScore;
            int realScore = 0;
            if (preScore < 7)
            {
                //in testing the original method (found in the else statement below) wasn't really producing shutouts or 58-3 type showings. this basically says "if the randomly generated score is less than a touchdown, give it 50% odds of being a shutout and otherwise make it likely that they just got a field goal or two". not very scientific but it'll do the job
                Random rand = new Random();
                double odds = rand.NextDouble();
                if (odds > 0.5)
                {
                    while (preScore > 2)
                    {
                        odds = rand.NextDouble();
                        if (odds < .95)
                        {
                            realScore += 3;
                            preScore -= 3;
                        }
                        //5% chance of a safety
                        else
                        {
                            realScore += 2;
                            preScore -= 2;
                        }
                    }
                }

            }
            else
            {
                while (preScore > 2)
                {
                    //in general seems like the TD-FG ratio in college is 2.5-3. rounding up to 3 for simplicity's sake
                    //though i couldn't find any pre-calculated data for safeties for college football, in the NFL they occur once every 14 games on average. So I used the following formula to determine frequency of safeties relative to TDs and FGs: 1/((TDs/Game + FGs/Game ) * 14) = Share of scores that are safeties. It's about 1-2%.
                    Random rand = new Random();
                    double odds = rand.NextDouble();
                    //3 to 1 odds of a TD to a FG
                    if (odds < 0.73)
                    {
                        realScore += 7;
                        preScore -= 7;

                    }
                    else if (odds < .98)
                    {
                        realScore += 3;
                        preScore -= 3;
                    }
                    //2% chance of a safety
                    else
                    {
                        realScore += 2;
                        preScore -= 2;
                    }
                }
            }
            return realScore;
        }
    }
}
