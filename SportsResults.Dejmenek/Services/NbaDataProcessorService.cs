using HandlebarsDotNet;
using SportsResults.Dejmenek.Models;

namespace SportsResults.Dejmenek.Services;
public class NbaDataProcessorService : INbaDataProcessorService
{
    private static string mailTemplate = """
        <h1>Latest NBA Updates - {{ Date }}</h1>
        <h2>Today's Games</h2>
        {{#if Games}}
            <div>
            {{#each Games}}
            <div style="border: 1px solid black; margin-bottom: 10px">
                <div><p>{{ HomeTeam.Name }} - {{HomeTeamScore}}</p></div>
                <div><p>{{ AwayTeam.Name }} - {{AwayTeamScore}}</p></div>
            </div>
            {{/each}}
            </div>
        {{else}}
            <p>No games played yet</p>
        {{/if}}
        <h2>Eastern Conference Standings</h2>
        <table>
          <thead>
            <tr>
              <th>Team</th>
              <th>Wins</th>
              <th>Losses</th>
              <th>%</th>
            </tr>
          </thead>
          <tbody>
            {{#each EasternStandings}}
            <tr>
              <td>{{ Team.Name }}</td>
              <td>{{ Wins }}</td>
              <td>{{ Losses }}</td>
              <td>{{ WinPercentage }}</td>
            </tr>
            {{/each}}
          </tbody>
        </table>
        <h2>Western Conference Standings</h2>
        <table>
          <thead>
            <tr>
              <th>Team</th>
              <th>Wins</th>
              <th>Losses</th>
              <th>%</th>
            </tr>
          </thead>
          <tbody>
            {{#each WesternStandings}}
            <tr>
              <td>{{ Team.Name }}</td>
              <td>{{ Wins }}</td>
              <td>{{ Losses }}</td>
              <td>{{ WinPercentage }}</td>
            </tr>
            {{/each}}
          </tbody>
        </table>
        """;

    public string PrepareEmailBody(List<Game> games, List<TeamStanding> easternConferenceStandings, List<TeamStanding> westernConferenceStandings)
    {
        var data = new
        {
            Date = DateTime.Now.ToString("dd/MM/yyyy"),
            Games = games,
            EasternStandings = easternConferenceStandings,
            WesternStandings = westernConferenceStandings
        };

        var body = Handlebars.Compile(mailTemplate)(data);

        return body;
    }
}
