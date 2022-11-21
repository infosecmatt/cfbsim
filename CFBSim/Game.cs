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
