using System.Globalization;

namespace SportsResultNotifierCarDioLogic.Helpers;

static public class DateConverterHelper
{
    static public (int year, int month, int day) ConvertDateForScrapper(string dateString)
    {
        DateTime date = DateTime.ParseExact(dateString, "yyyy/MM/dd", CultureInfo.InvariantCulture);

        int year = date.Year;
        int month = date.Month;
        int day = date.Day;

        return (year, month, day);
    }
}
