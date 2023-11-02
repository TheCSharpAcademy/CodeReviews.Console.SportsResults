using HtmlAgilityPack;
using SportsResultNotifier.Model;
using System.Data;

namespace SportsResultNotifier
{
    public class Scrapper
    {
        public HtmlDocument Document { get; set; }
        public List<string> Columns { get; set; } = new();
        public List<List<string>> Rows { get; set; } = new();
        public List<TeamModel> EasternTeam { get; set; } = new();
        public List<TeamModel> WesternTeam { get; set; } = new();
        public Scrapper()
        {
            string url = "https://www.basketball-reference.com/boxscores/";
            HtmlWeb web = new HtmlWeb();
            Document = web.Load(url);
        }

        public void GetColumns(string seperator)
        {
            try
            {
                Columns = Document.DocumentNode
                        .SelectNodes($"//*[@id=\"confs_standings_{seperator}\"]/thead/tr/th")
                        .Select(x => x.InnerText)
                        .ToList();
            }
            catch
            {
                Columns.Clear();
            }
        }

        public void GetRows(string seperator)
        {
            try
            {
                Rows = Document.DocumentNode
                        .SelectNodes($"//*[@id=\"confs_standings_{seperator}\"]/tbody/tr")
                        .Select(row => row.ChildNodes
                            .Select(cell => cell.InnerText)
                            .ToList())
                        .ToList();
            }
            catch
            {
                Rows.Clear();
            }

        }
        public DataTable BuildTable(string seperator)
        {
            DataTable data = new();

            GetColumns(seperator);
            GetRows(seperator);

            Columns.ForEach(x => data.Columns.Add(x));
            foreach (var row in Rows)
            {
                var teamData = GetModelData(row);
                if (seperator == "E") 
                    EasternTeam.Add(teamData);
                else 
                    WesternTeam.Add(teamData);

                data.Rows.Add(row[0], row[1], row[2], row[3], row[4], row[5], row[6]);
            }

            return data;
        }
        public TeamModel GetModelData(List<string> data)
        {
            return new TeamModel()
            {
                Name = data[0],
                Win = data[1],
                Lose = data[2],
                WL = data[3],
                GB = data[4],
                PSG = data[5],
                PAG = data[6]
            };
        }
    }
}
