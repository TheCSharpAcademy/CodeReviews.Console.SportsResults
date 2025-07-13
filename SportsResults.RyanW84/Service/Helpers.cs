using Spectre.Console;
using WebScraper_RyanW84.Models;
using HtmlAgilityPack;  

namespace WebScraper_RyanW84.Service;

public class Helpers
{
    internal Table BuildTable(string[] headings, string[][] allRows)
    {
        var table = new Table();
        foreach (var heading in headings)
            table.AddColumn(heading);

        if (allRows != null)
        {
            foreach (var rowData in allRows)
            {
                if (rowData != null)
                {
                    table.AddRow(rowData);
                }
            }
        }
   
        else if (headings.Length == 0)
        {
            System.Console.WriteLine("Warning: No headings provided.");
        }
        else
        {
            System.Console.WriteLine("Warning: Table Details or All Rows is null.");
        }

        return table;
    }

    public void DisplayTable(Table table)
    {
        AnsiConsole.Write(table);
    }
}