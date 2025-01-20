namespace SportsResults.K_MYR;

public record class GameSummary
(
    string GuestTeam,
    string HomeTeam,
    string Winner,
    int GuestTeamPoints,
    int HomeTeamPoints,
    string Gamelink,
    int[] PointsPerQuarterGuest,
    int[] PointsPerQuarterHome,
    string PlayerWithMostPoints,
    int MostPoints,
    string PlayerWithMostTotalRebounds,
    int MostTotalRebounds,
    DateOnly Date
);
