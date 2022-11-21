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
        foreach (var team in teams)
        {
            int BamaWinCount = 0;
            int BamaLossCount = 0;
            int BamaHighScore = 0;
            int BamaLowScore = 1000;
            int BamaTotalPoints = 0;
            int OppTotalPoints = 0;
            for (int i = 0; i < 50; i++)
            {
                Game currentGame = new Game(Bama, team);
                BamaTotalPoints += currentGame.HomeTeamScore;
                OppTotalPoints += currentGame.AwayTeamScore;
                if (currentGame.HomeTeamScore > BamaHighScore)
                {
                    BamaHighScore = currentGame.HomeTeamScore;
                }
                if (currentGame.HomeTeamScore < BamaLowScore) { BamaLowScore = currentGame.HomeTeamScore; }
                string winner = currentGame.Winner;
                if (winner == "Alabama")
                {
                    BamaWinCount++;
                }
                else { BamaLossCount++; }
            }
            Console.WriteLine($"In a simulation of 50 games between {Bama.uniName} and {team.uniName}, {Bama.uniName} won {BamaWinCount} times and {team.uniName} won {BamaLossCount} times. {Bama.uniName}'s highest point total was {BamaHighScore}, its lowest was {BamaLowScore}, and the average game score was {Bama.teamShorthand} {BamaTotalPoints / 50} {team.teamShorthand} {OppTotalPoints / 50}");
        }

        foreach (var team in teams)
        {
            int TAMUWinCount = 0;
            int TAMULossCount = 0;
            int TAMUHighScore = 0;
            int TAMULowScore = 1000;
            int TAMUTotalPoints = 0;
            int OppTotalPoints = 0;
            for (int i = 0; i < 50; i++)
            {
                Game currentGame = new Game(TexasAM, team);
                TAMUTotalPoints += currentGame.HomeTeamScore;
                OppTotalPoints+= currentGame.AwayTeamScore;
                if (currentGame.HomeTeamScore > TAMUHighScore)
                {
                    TAMUHighScore= currentGame.HomeTeamScore;
                }
                if (currentGame.HomeTeamScore < TAMULowScore) { TAMULowScore= currentGame.HomeTeamScore; }
                string winner = currentGame.Winner;
                if (winner == "Texas A&M")
                {
                    TAMUWinCount++;
                }
                else { TAMULossCount++; }
            }
            Console.WriteLine($"In a simulation of 50 games between {TexasAM.uniName} and {team.uniName}, {TexasAM.uniName} won {TAMUWinCount} times and {team.uniName} won {TAMULossCount} times. {TexasAM.uniName}'s highest point total was {TAMUHighScore}, its lowest was {TAMULowScore}, and the average game score was {TexasAM.teamShorthand} {TAMUTotalPoints / 50} {team.teamShorthand} {OppTotalPoints / 50}");
        }
        //Console.WriteLine(Game.GetFinalScore(Bama, UMass));
        //Console.WriteLine(Game.GetFinalScore(Bama, Georgia));
    }
}