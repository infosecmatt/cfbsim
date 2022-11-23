using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFBSim
{
    public class Team
    {
        public string uniName;
        public string teamName;
        public string teamShorthand;
        public string city;
        public string state;
        public int enrollment;
        public string conference;
        public string confDiv;
        public int historicalWins;
        public double prestige;
        public string offPlayStyle;
        private double offAbility;
        public string defPlayStyle;
        private double defAbility;
        public double homeFieldAdvantage;
        static readonly double leagueAverageScore = 28.4;
        static readonly double leagueAveragePlays = 136.7;

        public Team(string aUniName, string aTeamName, string aTeamShorthand, string aConference, string aConfDiv, string aCity, string aState, int aEnrollment, double aPrestige, string aOffPlayStyle, double aOffAbility, string aDefPlayStyle, double aDefAbility, double aHomeFieldAdvantage)
        {
            uniName = aUniName;
            teamName = aTeamName;
            teamShorthand = aTeamShorthand;
            conference = aConference;
            confDiv = aConfDiv;
            city = aCity;
            state = aState;
            enrollment = aEnrollment;
            prestige = aPrestige;
            offPlayStyle = aOffPlayStyle;
            offAbility = aOffAbility;
            defPlayStyle = aDefPlayStyle;
            defAbility = aDefAbility;
            homeFieldAdvantage = aHomeFieldAdvantage;
        }

        public static Team FromCsv(string csvLine)
        {
            string[] values = csvLine.Split(',');

            string uniName = values[0];
            string teamName = values[1];
            string teamShorthand = values[2];
            string city = values[3];
            string state = values[4];
            int enrollment = Int32.Parse(values[5]);
            string conference = values[6];
            string confDiv = values[19];
            int historicalWins = Int32.Parse(values[7]);
            double ppgScored = Convert.ToDouble(values[8]);
            double ppgAllowed = Convert.ToDouble(values[9]);
            double homeATS = Convert.ToDouble(values[10]);
            double offPassYds = Convert.ToDouble(values[11]);
            double offRushYds = Convert.ToDouble(values[12]);
            double defPassYds = Convert.ToDouble(values[13]);
            double defRushYds = Convert.ToDouble(values[14]);
            double offPassAtt = Convert.ToDouble(values[15]);
            double offRushAtt = Convert.ToDouble(values[16]);
            double defPassAtt = Convert.ToDouble(values[17]);
            double defRushAtt = Convert.ToDouble(values[18]); 

            double teamPace = offPassAtt + offRushAtt + defPassAtt + defRushAtt;

            return new Team(uniName, teamName, teamShorthand, conference, confDiv, city, state, enrollment, (historicalWins / 10), Team.DetermineOffPlayStyle(offPassAtt, offRushAtt), Team.CalcOffAbilityScore(ppgScored, teamPace), Team.DetermineDefPlayStyle(defPassYds, defPassAtt, defRushYds, defRushAtt), Team.CalcDefAbilityScore(ppgAllowed, teamPace), Team.GetHomeFieldAdvantage(homeATS));

        }

        public double OffAbility
        {
            get { return offAbility; }
            set
            {
                if (value < 20)
                {
                    offAbility = 20;
                }
                else if (value > 100)
                {
                    offAbility = 100;
                }
                else
                {
                    offAbility = value;
                }
            }
        }
        public double DefAbility
        {
            get { return defAbility; }
            set
            {
                if (value < 20)
                {
                    defAbility = 20;
                }
                else if (value > 100)
                {
                    defAbility = 100;
                }
                else
                {
                    defAbility = value;
                }
            }
        }
        public string SummarizeTeam()
        {
            string summary = "";
            summary += $"The {uniName} {teamName}, also known as \"{teamShorthand}\", is a Division 1 football program and a part of the {conference}.";
            summary += " ";
            summary += $"{uniName} is located in {city}, {state}.";
            summary += " ";
            summary += $"Analysts have described the team as {DescribePrestige(prestige)}.";
            summary += " ";
            summary += $"Offensively, the team is known for its {offPlayStyle} play style and is considered to be {DescribeAbility(offAbility)}.";
            summary += " ";
            summary += $"Defensively, the team is known for its {defPlayStyle} play style and is considered to be {DescribeAbility(defAbility)}.";
            return summary;
        }
        public static string DescribePrestige(double prestige)
        {
            string strPrestige;
            if (prestige >= 90.0)
            {
                strPrestige = "very prestigious";
            }
            else if (prestige >= 75.0)
            {
                strPrestige = "prestigious";
            }
            else if (prestige >= 50.0)
            {
                strPrestige = "moderately prestigious";
            }
            else
            {
                strPrestige = "not prestigious";
            }

            return strPrestige;
        }
        private static string DescribeAbility(double ability)
        {
            string strAbility;
            if (ability >= 90.0)
            {
                strAbility = "legendary";
            }
            else if (ability >= 75.0)
            {
                strAbility = "strong";
            }
            else if (ability >= 50.0)
            {
                strAbility = "average";
            }
            else
            {
                strAbility = "below average";
            }
            return strAbility;

        }

        public static double CalcDefAbilityScore(double pointsPerGame, double playsPerGame)
        {
            // formula: ppg/plays per game = (50 / def ability) * (league-avg ppg/league-avg plays)
            double leagueAverageOffenseAbility = 50.0;
            double ability = (leagueAverageScore * leagueAverageOffenseAbility * playsPerGame) / (pointsPerGame * leagueAveragePlays);
            return ability;
        }
        public static double CalcOffAbilityScore(double pointsPerGame, double playsPerGame)
        {
            // formula: ppg/plays per game = (off ability / 50) * league-avg ppg/league-avg plays
            double leagueAverageDefenseAbility = 50.0;
            double ability = (pointsPerGame * leagueAverageDefenseAbility * leagueAveragePlays) / (playsPerGame * leagueAverageScore);
            return ability;
        }
        public static double GetHomeFieldAdvantage(double HomeATSSince16)
        {
            // if a team beats the spread 60% of the time at home, it is inferred that they have a home field advantage factor of 1.2
            // reference values: https://www.actionnetwork.com/ncaaf/home-field-advantage-every-college-football-team-betting-2022
            double homeFieldAdvantage;
            //technically if a team was so bad that they lost against the spread 99+% of the time they would pull a really boring nuclear ghandi and become league average at home. call it an easter egg because i ain't fixing it
            if (HomeATSSince16 < 1)
            {
                HomeATSSince16 *= 100;
            }
            homeFieldAdvantage = HomeATSSince16 / 50;
            return homeFieldAdvantage;
        }
        public static string DetermineOffPlayStyle(double passAtt, double rushAtt)
        {
            // based on 2022 data, between the 10th and 90th percentile of percent pass plays / total plays the relationship is more or less linear.
            // the equation for determining this is 4.44x - 1.5424 = y, where x is the percent passing plays and y is the percentile
            // teams in the 80th percentile and above are considered to be pass-heavy, between the 60th and 80th moderate pass, between 40 and 60, balanced, between 20 and 40 moderate run, and less than 20 are considered run heavy
            string offPlayStyle;
            double pctPassingPlays = passAtt / rushAtt;
            double passPercentile = (4.44 * pctPassingPlays) - 1.5424;
            if (passPercentile > .80)
            {
                offPlayStyle = "Pass-heavy";
            }
            else if (passPercentile > 0.6)
            {
                offPlayStyle = "Moderate pass";
            }
            else if (passPercentile > 0.4)
            {
                offPlayStyle = "Balanced";
            }
            else if (passPercentile > 0.2)
            {
                offPlayStyle = "Moderate run";
            }
            else
            {
                offPlayStyle = "Run-heavy";
            }
            return offPlayStyle;
        }
        public static string DetermineDefPlayStyle(double defPassYds, double defPassAtt, double defRushYds, double defRushAtt)
        {
            // based on 2022 data, between the 10th and 90th percentile of Pass YPA / Rush YPA the relationship is more or less linear.
            // the equation for determining this is 1.24x - 1.7 = y, where x is the ratio of Pass YPA to Rush YPA and y is the percentile
            // teams in the 80th percentile and above are considered to be heavy run stop, between the 60th and 80th moderate run stop, between 40 and 60, balanced, between 20 and 40 moderate pass defense, and less than 20 are considered heavy pass defense
            string defPlayStyle;
            double passYPAtoRunYPA = (defPassYds / defPassAtt) / (defRushYds / defRushAtt);
            double ypaPercentile = (1.24 * passYPAtoRunYPA) - 1.7;
            if (ypaPercentile > .80)
            {
                defPlayStyle = "Heavy run defense focus";
            }
            else if (ypaPercentile > 0.6)
            {
                defPlayStyle = "Moderate run defense focus";
            }
            else if (ypaPercentile > 0.4)
            {
                defPlayStyle = "Balanced";
            }
            else if (ypaPercentile > 0.2)
            {
                defPlayStyle = "Moderate pass defense focus";
            }
            else
            {
                defPlayStyle = "Heavy pass defense focus";
            }
            return defPlayStyle;
        }
    }
}
