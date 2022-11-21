using CFBSim;
using System.Net;

internal class Program
{
    private static void Main(string[] args)
    {
        List<Team> teams = new List<Team>();

        Team Bama = new Team("Alabama", "Crimson Tide", "BAMA", "Division 1", "SEC", "Tuscaloosa, Alabama", 99.9, "balanced pro-style", Team.OffPPGToAbilityScore(40.6), "balanced 3-4", Team.DefPPGToAbilityScore(18.9), Team.GetHomeFieldAdvantage(61.5));
        teams.Add(Bama);
        Console.WriteLine(Bama.OffAbility);
        Console.WriteLine(Bama.DefAbility);
        Console.WriteLine(Bama.homeFieldAdvantage);

        Team OhioState = new Team("Ohio State", "Buckeyes", "tOSU", "Division 1", "B1G", "Columbus, Ohio", 99.9, "spread", Team.OffPPGToAbilityScore(46.5), "trench beasts", Team.DefPPGToAbilityScore(16.9), Team.GetHomeFieldAdvantage(58.3));
        teams.Add(OhioState);
        Console.WriteLine(OhioState.OffAbility);
        Console.WriteLine(OhioState.DefAbility);
        Console.WriteLine(OhioState.homeFieldAdvantage);

        Team TexasAM = new Team("Texas A&M", "Aggies", "TAMU", "Division 1", "SEC", "College Station, Texas", 70.0, "run-first pro-style", Team.OffPPGToAbilityScore(20.4), "run-stuffing 3-4", Team.DefPPGToAbilityScore(23.1), Team.GetHomeFieldAdvantage(68.0));
        teams.Add(TexasAM);
        Console.WriteLine(TexasAM.OffAbility);
        Console.WriteLine(TexasAM.DefAbility);
        Console.WriteLine(TexasAM.homeFieldAdvantage);
        
        Team ArizonaSt = new Team("Arizona State", "Sundevils", "ASU", "Division 1", "PAC-12", "Tempe, Arizona", 50.0, "pass-first spread", Team.OffPPGToAbilityScore(23.8), "pass-rushing zone", Team.DefPPGToAbilityScore(33.6), Team.GetHomeFieldAdvantage(47.6));
        teams.Add(ArizonaSt);
        Console.WriteLine(ArizonaSt.OffAbility);
        Console.WriteLine(ArizonaSt.DefAbility);
        Console.WriteLine(ArizonaSt.homeFieldAdvantage);
        
        Team UMass = new Team("Massachusetts", "Minutemen", "MASS", "Division 1", "Independent", "Amherst, Massachusetts", 25.0, "pass-first spread", Team.OffPPGToAbilityScore(12.3), "balanced man", Team.DefPPGToAbilityScore(32.6), Team.GetHomeFieldAdvantage(44.4));
        teams.Add(UMass);
        Console.WriteLine(UMass.OffAbility);
        Console.WriteLine(UMass.DefAbility);
        Console.WriteLine(UMass.homeFieldAdvantage);

        Team Georgia = new Team("Georgia", "Bulldogs", "UGA", "Division 1", "SEC", "Athens, Georgia", 90.0, "run-first pro-style", Team.OffPPGToAbilityScore(38.9), "pass-rush zone", Team.DefPPGToAbilityScore(12.2), Team.GetHomeFieldAdvantage(43.5));
        teams.Add(Georgia);
        Console.WriteLine(Georgia.OffAbility);
        Console.WriteLine(Georgia.DefAbility);
        Console.WriteLine(Georgia.homeFieldAdvantage);

        Console.WriteLine($"There are {teams.Count} teams in college football.");
        foreach (var team in teams)
        {
            Console.WriteLine(team.SummarizeTeam());
        }
        

        Console.WriteLine("Playing some games...");
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
        }
    }
}