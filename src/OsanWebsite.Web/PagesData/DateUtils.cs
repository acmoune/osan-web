namespace OsanWebsite.Web.PagesData;

public static class DateUtils
{
    public static Dictionary<int, string> Months = new Dictionary<int, string>
    {
        { 1, "Janvier" },
        { 2, "Février" },
        { 3, "Mars" },
        { 4, "Avril" },
        { 5, "Mai" },
        { 6, "Juin" },
        { 7, "Juillet" },
        { 8, "Août" },
        { 9, "Septembre" },
        { 10, "Octobre" },
        { 11, "Novembre" },
        { 12, "Décembre" },
    };

    public static IEnumerable<int> Years = new[] { 2024, 2025, 2026, 2027, 2028 };
}

public class MonthData 
{
    public int Month { get; set; }
    public int Year { get; set; }
 }
