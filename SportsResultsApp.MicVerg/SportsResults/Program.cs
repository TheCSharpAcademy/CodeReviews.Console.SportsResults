using SportsResults;

string mailBody = string.Join("\n", GetWinningTeams.WinningNbaTeams());
SendMail.SendEmail(mailBody);

