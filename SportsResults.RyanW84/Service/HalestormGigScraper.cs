using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using Spectre.Console;
using WebScraper_RyanW84.Models;

namespace WebScraper_RyanW84.Service;

public class HalestormScraper() : IScraper
{
    private const string Url = "https://www.halestormrocks.com/#tour";
    private const string TableClassName = "seated-events-table";
    private readonly Helpers _helpers;

    public HalestormScraper(Helpers helpers)
        : this()
    {
        _helpers = helpers;
    }

    public async Task<Results> Run()
    {
        var document = await LoadDocument(Url);
        var tableHeadings = GetTableHeadings(document);
        var allRows = GetAllTableRows(document);

        var title = GetTitle(document);

        DisplayScraperInfo(title);

        var results = new Results
        {
            EmailTitle = title,
            EmailTableHeadings = tableHeadings,
            EmailTableRows = allRows,
        };

        _helpers.DisplayTable(_helpers.BuildTable(tableHeadings, allRows));
        return results;
    }

    private async Task<HtmlDocument> LoadDocument(string url)
    {
        var options = new ChromeOptions();
        options.AddArgument("--headless=new");
        options.AddArgument("--disable-gpu");
        options.AddArgument("--no-sandbox");
        options.AddArgument("--window-size=1920,1080");

        using var driver = new ChromeDriver(options);
        try
        {
            driver.Navigate().GoToUrl(url);

            // Wait for the tour section to be visible and have content
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            wait.Until(driver => driver.FindElements(By.ClassName("seated-event-row")).Count > 0);

            // Add a small delay to ensure all dynamic content is loaded
            await Task.Delay(2000);

            var html = driver.PageSource;
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            return doc;
        }
        catch (WebDriverTimeoutException ex)
        {
            Console.WriteLine(ex);
            AnsiConsole.MarkupLine("[red]Timeout waiting for tour dates to load. Retrying...[/]");
            throw;
        }
        finally
        {
            driver?.Quit();
        }
    }

    private string[] GetTableHeadings(HtmlDocument document)
    {
        // Try to find headings in the tour section, fallback to defaults if not found
        var headingsNode = document.DocumentNode.SelectSingleNode(
            "//div[contains(@class, 'tour-date-container')]//div[contains(@class, 'tour-date')]"
        );
        if (headingsNode != null)
        {
            // Example: headings as child divs with specific classes
            var headingDivs = headingsNode.SelectNodes(
                ".//div[contains(@class, 'date') or contains(@class, 'location') or contains(@class, 'venue') or contains(@class, 'ticket-link')]"
            );
            if (headingDivs != null)
            {
                return headingDivs.Select(node => node.InnerText.Trim()).ToArray();
            }
        }
        // Fallback to hardcoded headings
        return new[] { "Date", "Location", "Venue", "More Info" };
    }

    private string[][] GetAllTableRows(HtmlDocument document)
    {
        var gigNodes = document.DocumentNode.SelectNodes(
            $"//div[contains(@class, \"seated-event-row\")]"
        );
        var rows = new List<string[]>();

        if (gigNodes != null)
        {
            foreach (var node in gigNodes)
            {
                var date = ExtractNodeText(node, "seated-event-date");
                var location = ExtractNodeText(node, "location");
                var venue = ExtractNodeText(node, "seated-event-venue-name");
				var ticketUrl = ExtractLinkHref(node , "seated-event-link seated-event-link1");
				var moreInfo = !string.IsNullOrEmpty(ticketUrl) ? $"{ticketUrl}" : "N/A";



				rows.Add(new[] { date, location, venue, moreInfo });
            }
        }

        return [.. rows];
    }

    private string ExtractNodeText(HtmlNode parentNode, string className) =>
        parentNode.SelectSingleNode($".//div[contains(@class, '{className}')]")?.InnerText.Trim()
        ?? "N/A";

    private string ExtractLinkHref(HtmlNode parentNode, string v) =>
        parentNode
            .SelectSingleNode(".//a[contains(@class, 'seated-event-link1')]")
            ?.GetAttributeValue("href", "") ?? "";

    private string GetTitle(HtmlDocument document) =>
        document.DocumentNode.SelectSingleNode("//title")?.InnerText.Trim()
        ?? "Halestorm Upcoming Gigs";

    private void DisplayScraperInfo(string title)
    {
        Console.Clear();
        AnsiConsole.Write(
            new Rule("[bold italic yellow]Halestorm Gigs Scraper[/]").RuleStyle("yellow").Centered()
        );

        Console.WriteLine(title);
        Console.WriteLine();
        AnsiConsole.MarkupLine("[Yellow]Passing data for Email to be sent[/]");
    }
}
