# **SportsResultsNotifier** 🏀📩  

This project scrapes basketball game results from [Basketball Reference](https://www.basketball-reference.com) for **yesterday's date** and sends an email with the results using SMTP.  

---

## **🚀 Features**
- Scrapes **yesterday's** game results from [Basketball Reference](https://www.basketball-reference.com/boxscores/).
- Formats the results and sends them via email.
- Uses **`HtmlAgilityPack`** for web scraping.
- Reads email configuration from `appsettings.json`.

---

## **🛠 Setup & Usage**
### **1️⃣ Clone the Repository**
```sh
git clone https://github.com/Eyad-Mostafa/SportsResultsNotifier.git
cd SportsResultsNotifier
```

### **2️⃣ Install Dependencies**
Make sure you have .NET 8 installed, then restore dependencies:
```sh
dotnet restore
```

### **3️⃣ Configure Email Settings**  
Before running the project, update `appsettings.json` with **your actual email credentials**:
```json
{
  "EmailSettings": {
    "SmtpAddress": "smtp.gmail.com",
    "PortNumber": 587,
    "EnableSSL": true,
    "EmailFromAddress": "your-email@gmail.com",
    "Password": "your-email-password",
    "EmailToAddress": "receiver-email@gmail.com"
  }
}
```
🚨 **Never commit your real email and password!**  

---

## **📌 Running the Project**
Run the application using:
```sh
dotnet run
```
The program will:
1. Scrape **yesterday's** basketball game results.
2. Send an email with the results.

---

## **🗒 Notes**
- If you fork or clone this project, **update `appsettings.json`** with valid email credentials before running.  
- You may need to enable **Less Secure Apps** in your Google account or use **App Passwords** if using Gmail.

