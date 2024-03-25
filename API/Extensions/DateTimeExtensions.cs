namespace ProjectP.Extensions;

public static class DateTimeExtensions
{
    public static int NumberOfYears(this DateTime date)
    {
        var today = DateTime.Today;
        var years = today.Year - date.Year;

        if (date.Date > today.AddYears(-years)) years--;
        return years;
    }
}