# Sports Results Notifier


<table>
  <tr>
    <td>
      <img src="https://github.com/user-attachments/assets/95499d31-77fa-4a5a-9f4f-b222ea6089f6" width="300">
    </td>
    <td>
      <h1>ExerciseTracker</h1>
      <p>This is an application where it reads sports data from a website once a day and sends it to a specific e-mail address.<br>
The data will be scraped from this 
<a href="https://www.basketball-reference.com/boxscores/" target="_blank">Basketball Reference Website</a>.
</p>
    </td>
  </tr>
</table>

---

## 📚 Resources I Used
> A list of tutorials, courses, blogs, or documentation that helped me build this project.

- [Learnt how to use the Html Agility Pack by reading directly from their documentation.](https://html-agility-pack.net/)
- [I had a revision on sending an email through Gmail SMTP.](https://www.c-sharpcorner.com/blogs/send-email-using-gmail-smtp)

---

## 🛠️ Tech Stack

| Category        | Technology Used     |
|----------------|---------------------|
| Backend        | `ASP.NET Core`      |
| Dependency Injection | `Built-in .NET Core DI` |
| Logging        | `Serilog`           |
| Web Scraping   | `Html Agiliy Pack`|

---


## 🚀 How to Run This App

> Simple and clear instructions on how to get the app running locally.

1. Clone the repository
   ```bash
   git clone https://github.com/your-username/ExerciseTracker.git
2. Navigate to the project directory
   ```bash
   cd ExerciseTracker
3. Update to the settings marked in `appsettings.json`
   
4. Update the reciever Email in `SportsResultsWorker.cs` class 
   
5. Run the project
   ```bash
   dotnet run

---

> The main features of the app.

- [ ] It scrapes the website of NBA results.
- [ ] Formats the results in a clean way.
- [ ] Switch between Emailing papercut localhost or Gmail SMTP seamlessly
- [ ] Logging with Serilog

---

