# WebScraper

<p>Application that checks daily for most recent NBA game scores and emails them to user.</p>

<h3>How the Application Works:</h3>

<p>
  Uses HTML Agility Pack as its web scraping library, and uses Basketball Reference's box score web page as its source.
  Using the XPath of the target HTML elements, it extracts the two teams from each game, as well as their scores.
  This information, along with the date the games were played, are included in an email object and sent to target email using SMTP.
  If no games, were played, this fact is sent to the target email instead.
</p>

<h4>Application is then configured using Windows Task Scheduler to run at 3:00 AM EST everyday.</h4>
