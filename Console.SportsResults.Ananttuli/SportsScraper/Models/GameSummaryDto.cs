namespace SportsScraper.Models;

public record class GameSummaryDto(
    Team TeamOne,
    Team TeamTwo
);

public record class Team(
    List<string> QuarterScores,
    string? Name = "",
    string? FinalScore = ""
);