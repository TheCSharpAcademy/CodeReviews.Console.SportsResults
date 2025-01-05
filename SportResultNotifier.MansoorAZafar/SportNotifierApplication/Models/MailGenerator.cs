using System.Text;
using WebScraperLib.Models;

namespace SportNotifierApplication.Models;

public static class MailGenerator
{
    // We use in-line css because really old versions may not support the <style> tag
    public static string GenerateTableMail(List<Game> data)
        => @$"
            <!DOCTYPE html>
            <html>
                <body>
                    <div style='display: flex; flex-wrap: wrap; gap: 10px; padding: 10px; justify-content: flex-start;'>
                        {GenerateTables(data)}
                    </div>
                </body>
            </html>
        ";

    private static string GenerateTable(Game item)
        => $@"
                <div style='display: flex; justify-content: space-between; padding: 5px 0; border-bottom: 1px solid #eee;'>
                    <div style='flex: 1; text-align: center;'>{item.GetWinner()}</div>
                    <div style='flex: 1; text-align: center;'>{item.TeamLabels[4]}</div>
                    <div style='flex: 1; text-align: center;'>{item.TeamLabels[5]}</div>
                    <div style='flex: 1; text-align: center;'>{item.TeamLabels[6]}</div>
                    <div style='flex: 1; text-align: center;'>{item.TeamLabels[7]}</div>
                </div>
                <div style='display: flex; justify-content: space-between; padding: 5px 0; border-bottom: 1px solid #eee;'>
                    <div style='flex: 1; text-align: center;'>{item.GetLoser()}</div>
                    <div style='flex: 1; text-align: center;'>{item.TeamLabels[0]}</div>
                    <div style='flex: 1; text-align: center;'>{item.TeamLabels[1]}</div>
                    <div style='flex: 1; text-align: center;'>{item.TeamLabels[2]}</div>
                    <div style='flex: 1; text-align: center;'>{item.TeamLabels[3]}</div>
                </div>
            ";

    private static string GenerateTables(List<Game> data)
    {
        StringBuilder htmlContent = new();
        foreach (var item in data)
        {
            htmlContent.Append(@$"
                <div style='border: 1px solid #ccc; border-radius: 8px; width: 300px; padding: 10px; box-shadow: 0 4px 6px rgba(0,0,0,0.1);'>
                <div style='font-size: 1.2rem; font-weight: bold; margin-bottom: 8px;'>{item.GetWinner()} vs {item.GetLoser()} </div>
                <div style='color: #555; font-weight: bold; margin-bottom: 8px;'>{item.GetWinner()} Won </div>
                <div style='font-size: 0.8rem;'>
            ");
            htmlContent.Append(GenerateTable(item));
            htmlContent.Append("</div></div>");

        }
        return htmlContent.ToString();
    }
}
