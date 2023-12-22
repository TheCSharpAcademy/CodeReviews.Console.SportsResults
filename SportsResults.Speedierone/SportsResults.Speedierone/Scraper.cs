using HtmlAgilityPack;

namespace SportsResults.Speedierone
{
    public class Scraper
    {
        public List<Results> GetResults(HtmlDocument doc)
        { 
            DateTime targetDate = DateTime.Now.AddDays(-1);
            List<Results> results = new List<Results>();

            var dateNode = doc.DocumentNode.SelectSingleNode("//span[@class='button2 index']");
            if (dateNode != null)
            {
                string dateString = dateNode.InnerText.Trim();
                DateTime dateOnPage;

                if (DateTime.TryParse(dateString, out dateOnPage))
                {
                    if (dateOnPage.Date == targetDate.Date)
                    {
                        var gameNodes = doc.DocumentNode.SelectNodes("//div[contains(@class,'game_summary expanded nohover')]");
                        try
                        {
                            if (gameNodes != null)
                            {
                                foreach (var gameNode in gameNodes)
                                {
                                    var teamsNode = gameNode.SelectSingleNode(".//table[@class='teams']");
                                    var winnerNode = teamsNode.SelectSingleNode(".//tr[@class='winner']");
                                    var loserNode = teamsNode.SelectSingleNode(".//tr[@class='loser']");

                                    string winnerTeam = winnerNode.SelectSingleNode(".//td[1]/a").InnerText.Trim();
                                    int winnerScore = int.Parse(winnerNode.SelectSingleNode(".//td[2]").InnerText.Trim());

                                    string loserTeam = loserNode.SelectSingleNode(".//td[1]/a").InnerText.Trim();
                                    int loserScore = int.Parse(loserNode.SelectSingleNode(".//td[2]").InnerText.Trim());

                                    results.Add(new Results
                                    {
                                        WinningTeam = winnerTeam,
                                        WinningScore = winnerScore,
                                        LosingTeam = loserTeam,
                                        LosingScore = loserScore
                                    });
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }
                }
            }
            return results;
        }
    }
}