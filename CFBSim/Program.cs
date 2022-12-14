using CFBSim;
using System.Net;

internal class Program
{
    private static void Main(string[] args)
    {
        List<Team> teams = File.ReadAllLines("C:\\Users\\matt\\source\\repos\\CFBSim\\fbsTeams.csv").Skip(1).Select(v => Team.FromCsv(v)).ToList();

        List<Game> schedule = Season.MakeRegSeasonSECSchedule(teams);
        var secTeams = teams.Where(team => team.conference == "SEC").ToList();

        foreach (Team team in secTeams)
        {
            int gameCount = 0;
            foreach (Game game in schedule)
            {
                if (game.awayTeam.uniName== team.uniName || game.homeTeam.uniName == team.uniName)
                {
                    gameCount++;
                    if (game.awayTeam.uniName == "Georgia" || game.homeTeam.uniName == "Georgia")
                    {
                        Console.WriteLine($"{game.awayTeam.uniName} @ {game.homeTeam.uniName}");
                    }
                }
            }
            //Console.WriteLine($"{team.uniName} games: {gameCount}");
        }

/*        foreach (Game game in schedule) { Console.WriteLine($"{game.awayTeam.uniName} @ {game.homeTeam.uniName}"); }
*/
       /* //testing football scores
        Team mass = teams.SingleOrDefault(item => item.uniName.Equals("Massachusetts"));
        Team uga = teams.SingleOrDefault(item => item.uniName.Equals("Georgia"));

        for (int i = 0; i < 100; i++)
        {
            Game game = new Game(mass, uga);
            Console.WriteLine(game.GetFinalScore());
        }*/

        /*Console.WriteLine($"There are {teams.Count} teams in college football.");
        foreach (var team in teams)
        {
            Console.WriteLine(team.SummarizeTeam());
            Console.WriteLine();
        }*/


        /*Console.WriteLine("Playing some games...");
        foreach (var team1 in teams)
        {
            foreach (var team2 in teams)
            {
                if (team1.uniName != team2.uniName)
                {
                    int Team1WinCount = 0;
                    int Team1LossCount = 0;
                    int Team1HighScore = 0;
                    int Team1LowScore = 1000;
                    int Team1TotalPoints = 0;
                    int OppTotalPoints = 0;
                    for (int i = 0; i < 50; i++)
                    {
                        Game currentGame = new Game(team1, team2);
                        Team1TotalPoints += currentGame.HomeTeamScore;
                        OppTotalPoints += currentGame.AwayTeamScore;
                        if (currentGame.HomeTeamScore > Team1HighScore)
                        {
                            Team1HighScore = currentGame.HomeTeamScore;
                        }
                        if (currentGame.HomeTeamScore < Team1LowScore) { Team1LowScore = currentGame.HomeTeamScore; }
                        string winner = currentGame.Winner;
                        if (winner == team1.uniName)
                        {
                            Team1WinCount++;
                        }
                        else { Team1LossCount++; }
                    }
                    Console.WriteLine($"In a simulation of 50 games between {team1.uniName} and {team2.uniName}, {team1.uniName} won {Team1WinCount} times and {team2.uniName} won {Team1LossCount} times. {team1.uniName}'s highest point total was {Team1HighScore}, its lowest was {Team1LowScore}, and the average game score was {team1.teamShorthand} {Team1TotalPoints / 50} {team2.teamShorthand} {OppTotalPoints / 50}");
                }
            }
        }*/
    }
}