using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CFBSim
{
    internal class Season
    {
        public int year;
        public int currentWeek;
        public static string[] powerFiveConferences = { "ACC", "Big 12", "Big Ten", "Pac-12", "SEC"};
        public static string[] groupOfFiveConferences = { "American", "C-USA", "MAC", "Mountain West", "Sun Belt" };

        /*public Season(List<Team> teams)
        {

        }*/

        public static List<Game> MakeRegSeasonSECSchedule(List<Team> teams)
        {
            //passing in all teams is probably pretty inefficient, but for now it'll do. optimize later (insert joke here)
            var secTeams = teams.Where(team => team.conference == "SEC").ToList();
            var secWest = secTeams.Where(team => team.confDiv == "West").ToList();
            var secEast = secTeams.Where(team => team.confDiv == "East").ToList();

            List<Game> secSchedule = new List<Game>();

            //first SEC West. round robin applies within the division
            secSchedule.AddRange(Season.RoundRobinScheduling(secWest));
            //then SEC East
            secSchedule.AddRange(Season.RoundRobinScheduling(secEast));

            // two additional interdivision games per year
            
            var rivalries = new[]
            {
                new {west = "Alabama", east = "Tennessee"},
                new {west = "Auburn", east = "Georgia" },
                new {west = "LSU", east = "Florida" },
                new {west = "Mississippi State", east = "Kentucky" },
                new {west = "Ole Miss", east = "Vanderbilt" },
                new {west = "Texas A&M", east = "South Carolina" },
                new {west = "Arkansas", east = "Missouri" }
            };
            int currentYear = 2022;
            int eastIndex = currentYear % secEast.Count;
            List<Team> secEastInterDiv = new List<Team>(secEast);

            foreach (Team team in secWest)
            {
                int i = Array.FindIndex(rivalries, r => r.west.Equals(team.uniName));
                //add the rivalry game
                Team westTeam = secWest.SingleOrDefault(item => item.uniName.Equals(rivalries[i].west));
                Team eastTeam = secEast.SingleOrDefault(item => item.uniName.Equals(rivalries[i].east));
                secSchedule.Add(new Game(eastTeam,westTeam));

                //add random interdivision game. by the end of the process all teams should be matched up. so I begin by creating a carbon copy of the secEast teams list, and for each team i remove their rival from the list using a copy of the copy. you can't remove the rival from the main available pool because they would then not be assigned the correct number of games. In order to ensure that the matchups change every year, I take the year modulo'd by the count of available teams, which becomes the index. this ensures that the list is being rotated yearly. once two teams are matched, the SEC East team is removed from the available pool and the process repeats with the next SEC west team.
                List<Team> availableOpponents = new List<Team>(secEastInterDiv);
                availableOpponents.RemoveAll(x => x.uniName.Equals(eastTeam.uniName));
                int count = availableOpponents.Count();
                count = currentYear % count;
                secSchedule.Add(new Game(westTeam, availableOpponents[count]));
                secEastInterDiv.RemoveAll(x => x.uniName.Equals(availableOpponents[count].uniName));


            }
            List<Game> FinalSecSchedule = secSchedule.Distinct().ToList();

            return FinalSecSchedule;
        }

        // this method is for generating the 3-5-5 schedule. it will produce two rotations and assign them to teams in a conference given as input
        public void Generate355StyleConfScheduleRotation(List<Team> teams)
        {
            //available teams starts off as all teams, but after a team hits 5 scheduled games it is removed from this list
            List<Team> availableTeams = teams;
            // start off with rotationA. this is a 1d array of a list of teams. this contains all opponents of the team.
            List<Team>[] rotationA = new List<Team>[teams.Count];

            //first, add rivalries to the rotations
            for (int i = 0; i < teams.Count; i++)
            {
                int mainRivalIndex = teams.FindIndex(x => x.uniName == teams[i].mainRivalUniName);
                int secondRivalIndex = teams.FindIndex(x => x.uniName == teams[i].secondRivalUniName);
                int thirdRivalIndex = teams.FindIndex(x => x.uniName == teams[i].thirdRivalUniName);
                rotationA[i].Add(teams[mainRivalIndex]);
                rotationA[i].Add(teams[secondRivalIndex]);
                rotationA[i].Add(teams[thirdRivalIndex]);
            }

            // now, the rest
            while (availableTeams.Count > 0)
            {
                //iterate over each team, one at a time. pick a random team to play against. update their availability count. update available teams (if necessary)
                for (int i = 0; i < teams.Count; i++)
                {
                    // if there's no available teams then stop. probably silly that i check this condition twice
                    if (availableTeams.Count == 0) { break; }

                    Team schedulingTeam = teams[i];
                    if (! availableTeams.Any(x => x.uniName == schedulingTeam.uniName)) { continue; } //skip this if the team isn't available
                    //find corresponding index of team in availableTeams array
                    int availableTeamIndex = availableTeams.FindIndex(x => x.uniName == schedulingTeam.uniName);

                    //get a random team from those available
                    Random rnd = new Random();
                    int r = rnd.Next(availableTeams.Count);
                    Team opposingTeam = availableTeams[r];
                    int opposingTeamIndex = teams.FindIndex(x => x.uniName == opposingTeam.uniName);

                    bool alreadyScheduled = rotationA[i].Any(item => item.uniName == opposingTeam.uniName);

                    // if the game hasn't already been scheduled and the team isn't facing itself, add the team
                    if (! alreadyScheduled && opposingTeam.uniName != schedulingTeam.uniName)
                    {
                        //add to the rotation of both teams
                        rotationA[i].Add(opposingTeam);
                        rotationA[opposingTeamIndex].Add(schedulingTeam);

                        //check if either team has hit the limit, which in this case is 8 (3 fixed rivalries plus 5 rotational games)
                        if (rotationA[i].Count == 8)
                        {
                            availableTeams.RemoveAt(availableTeamIndex);
                        }
                        if (rotationA[opposingTeamIndex].Count == 8)
                        {
                            availableTeams.RemoveAt(r);
                        }
                    }
                }
            }
            //rotationB is taking the difference between rotationA and all teams in the conference to get the leftover teams
            List<Team>[] rotationB = new List<Team>[teams.Count];
            
            // for each team, take the list of all teams except those already in rotationA. this will give you the opponents not scheduled and the host team itself. it will not include rivals. for rotationB i'll fix those issues separately.
            for (int i = 0; i < rotationB.Length; i++)
            {
                //gets all teams not found in rotationA
                rotationB[i] = teams.Except(rotationA[i]).ToList();
                //make sure team doesn't play itself.
                int selfTeam = rotationB[i].FindIndex(x => x.uniName == teams[i].uniName);
                rotationB[i].RemoveAt(selfTeam);
                //copy paste because i'm lazy
                int mainRivalIndex = teams.FindIndex(x => x.uniName == teams[i].mainRivalUniName);
                int secondRivalIndex = teams.FindIndex(x => x.uniName == teams[i].secondRivalUniName);
                int thirdRivalIndex = teams.FindIndex(x => x.uniName == teams[i].thirdRivalUniName);
                rotationB[i].Add(teams[mainRivalIndex]);
                rotationB[i].Add(teams[secondRivalIndex]);
                rotationB[i].Add(teams[thirdRivalIndex]);
            }

            return;
        }

        public static List<Game> RoundRobinScheduling(List<Team> teams)
        {
            List<Game> schedule = new List<Game>();

            if (teams == null || teams.Count < 2)
            {
                return schedule;
            }

            List<Team> restTeams = new List<Team>(teams.Skip(1));

            int teamCount = teams.Count();
            int gameplayWeeks;

            if (teamCount % 2 != 0)
            {
                restTeams.Add(default);
                teamCount++; //add a team to count to account for "bye" team
            }

            for (int week = 0; week < teamCount - 1; week++)
            {
                //if there's a bye week, the team will be playing against itself. this check only adds the game to the schedule if the two objects don't match
                if (restTeams[week % restTeams.Count]?.Equals(default) == false)
                {
                    schedule.Add(new Game(teams[0], restTeams[week % restTeams.Count]));
                }
                // rest of the teams
                for (int index = 1; index < (teamCount / 2); index++)
                {
                    Team homeTeam = restTeams[(week + index) % restTeams.Count];
                    Team awayTeam = restTeams[(week + restTeams.Count - index) % restTeams.Count];
                    //check for bye weeks
                    if (homeTeam?.Equals(default) == false && awayTeam?.Equals(default) == false)
                    {
                        schedule.Add(new Game(homeTeam, awayTeam));
                    }
                }
                    
            }

            return schedule;
        }
    }
}
