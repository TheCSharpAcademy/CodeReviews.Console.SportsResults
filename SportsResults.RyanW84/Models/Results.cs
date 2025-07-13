namespace WebScraper_RyanW84.Models;

/// <summary>
///     Represents basketball game results data for email reporting
/// </summary>
public class Results
{
	/// <summary>
	///     Gets or sets the title for the email
	/// </summary>
	public string EmailTitle { get; set; } = string.Empty;

	/// <summary>
	///     Gets or sets the table column headings
	/// </summary>
	public string[] EmailTableHeadings { get; set; } = Array.Empty<string>();

	/// <summary>
	///     Gets or sets the table row data
	/// </summary>
	public string[][] EmailTableRows { get; set; } = Array.Empty<string[]>();
}