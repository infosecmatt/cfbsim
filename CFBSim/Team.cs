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
        public string division;
        public string conference;
        public string location;
        public double prestige;
        public string offPlayStyle;
        private double offAbility;
        public string defPlayStyle;
        private double defAbility;
        public double homeFieldAdvantage;

        public Team(string aUniName, string aTeamName, string aTeamShorthand, string aDivision, string aConference, string aLocation, double aPrestige, string aOffPlayStyle, double aOffAbility, string aDefPlayStyle, double aDefAbility, double aHomeFieldAdvantage)
        {
            uniName = aUniName;
            teamName = aTeamName;
            teamShorthand = aTeamShorthand;
            division = aDivision;
            conference = aConference;
            location = aLocation;
            prestige = aPrestige;
            offPlayStyle = aOffPlayStyle;
            offAbility = aOffAbility;
            defPlayStyle = aDefPlayStyle;
            defAbility = aDefAbility;
            homeFieldAdvantage = aHomeFieldAdvantage;
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
            summary += $"The {uniName} {teamName}, also known as \"{teamShorthand}\", is a {division} football program and a part of the {conference}.";
            summary += " ";
            summary += $"{uniName} is located in {location}.";
            summary += " ";
            summary += $"Analysts have described the team as {describePrestige(prestige)}.";
            summary += " ";
            summary += $"Offensively, the team is known for its {offPlayStyle} play style and is considered to be {describeAbility(offAbility)}.";
            summary += " ";
            summary += $"Defensively, the team is known for its {defPlayStyle} play style and is considered to be {describeAbility(defAbility)}.";
            return summary;
        }
        private string describePrestige(double prestige)
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
        private string describeAbility(double ability)
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

        public static double DefPPGToAbilityScore(double ppg)
        {
            // formula: ppg = (50 / def ability) * league average scoring
            double leagueAverageScore = 30.0;
            double leagueAverageOffenseAbility = 50.0;
            double ability = (leagueAverageScore/ppg) * leagueAverageOffenseAbility;
            return ability;
        }
        public static double OffPPGToAbilityScore(double ppg)
        {
            // formula: ppg = (off ability / 50) * league average scoring
            double leagueAverageScore = 30.0;
            double leagueAverageDefenseAbility = 50.0;
            double ability = (ppg / leagueAverageScore) * leagueAverageDefenseAbility;
            return ability;
        }
        public static double GetHomeFieldAdvantage(double HomeATSSince16)
        {
            // if a team beats the spread 60% of the time at home, it is inferred that they have a home field advantage factor of 1.2
            // reference values: https://www.actionnetwork.com/ncaaf/home-field-advantage-every-college-football-team-betting-2022
            double homeFieldAdvantage = 1;
            if (HomeATSSince16 < 1)
            {
                HomeATSSince16 = HomeATSSince16 * 100;
            }
            homeFieldAdvantage = HomeATSSince16 / 50;
            return homeFieldAdvantage;
        }
    }
}
