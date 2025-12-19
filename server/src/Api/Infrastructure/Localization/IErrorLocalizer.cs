using System.Globalization;

namespace Api.Infrastructure.Localization
{
    public interface IErrorLocalizer
    {
        string GetMessage(string key, CultureInfo? culture = null);
        string GetMessage(string key, object arg, CultureInfo? culture = null);
    }
}