using SportsScraper.Models;

namespace SportsScraper;

public class EmailTemplate
{
    public static string Generate(List<GameSummaryDto> games)
    {
        var summaryMarkups = games.Select(GameSummaryHtml).ToList();
        var markup = string.Join("\n", summaryMarkups);
        var contents = summaryMarkups.Count == 0 ? "<h1>No games played</h1>" : markup;

        return $@"

        <!doctype html>
            <head>
                <title>NBA Scores today</title>
                <style>
                    table 
                    {{
                        font-size: 1.5rem;
                    }}
                    table {{
                        margin: 1rem;
                        border: 1px solid black;
                    }}

                    td {{
                        padding: 1.25rem;
                    }}

                    thead th:nth-child(1)
                    {{
                        width: 50%;
                        font-style: italic;
                    }}
                </style>
            </head>
            <body>
            <h1> Your daily NBA box scores update</h1>
                <h2><i>{games.Count}</i> NBA Games Played on {DateTime.Today.ToShortDateString()}</h1>
                {contents}
            </body>
        </html>
        ";
    }

    private static string GameSummaryHtml(GameSummaryDto game)
    {
        var (teamOne, teamTwo) = game;

        var cols = new List<string>(["Teams", "1", "2", "3", "4", "Final"]);

        string tableData = string.Join("", cols.Select((text) => $"<th>{text}</th>"));

        return "" +
        $@"
        <table>
            <thead>
                {tableData}
            </thead>
            <tbody>
                {TeamRow(teamOne)}
                {TeamRow(teamTwo)}
            </tbody>
        </table>
        ";
    }

    private static string TeamRow(Team team)
    {
        var (quarterScores, teamName, teamScore) = team;
        var quarterScoresMarkup = string.Join("", quarterScores.Select(s => $"<td>{s}</td>"));

        return $@"
            <tr>
                <td>{teamName}</td>
                {quarterScoresMarkup}
                <td><b>{teamScore}</b></td>
            </tr>
        ";
    }
}