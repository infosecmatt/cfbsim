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
        //public int homeTeamScore;
        //public int awayTeamScore;
        //public string winner;

        public Game(Team aHomeTeam, Team aAwayTeam)
        {
            homeTeam = aHomeTeam;
            awayTeam = aAwayTeam;
        }
        public int HomeTeamScore
        {
            get { return GetTeamScore(homeTeam, awayTeam); }
        }
        public int AwayTeamScore
        {
            get { return GetTeamScore(awayTeam, homeTeam); }
        }
        public string Winner
        {
            get
            {
                if (HomeTeamScore > AwayTeamScore)
                {
                    return homeTeam.uniName;
                }
                else if (AwayTeamScore > HomeTeamScore)
                {
                    return awayTeam.uniName;
                }
                else
                {
                    return "tie";
                }
            }
        }
        private int GetTeamScore(Team scoringTeam, Team opponent)
        {
            double points = (scoringTeam.OffAbility / opponent.DefAbility) * 30;

            Random rand = new Random(); //reuse this if you are generating many
            double u1 = 1.0 - rand.NextDouble(); //uniform(0,1] random doubles
            double u2 = 1.0 - rand.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                         Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
            double randNormal =
                         points + (points * .33) * randStdNormal; //random normal(mean,stdDev^2)

            return (int)Math.Round(randNormal);
        }
        public string GetFinalScore(Team homeTeam, Team awayTeam)
        {
            int homePoints = GetTeamScore(homeTeam, awayTeam);
            int awayPoints = GetTeamScore(awayTeam, homeTeam);
            string finalScore = $"{homeTeam.teamShorthand} {homePoints} - {awayTeam.teamShorthand} {awayPoints}";
            if (homePoints > awayPoints)
            {
                return homeTeam.uniName;
            }
            else
            {
                return awayTeam.uniName;
            }

        }
    }
}
