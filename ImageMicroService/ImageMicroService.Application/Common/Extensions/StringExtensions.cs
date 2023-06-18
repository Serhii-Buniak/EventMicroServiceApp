namespace ImageMicroService.Application.Common.Extensions;

public static class StringExtensions
{
    public static string IncreaseFileName(this string str, string suffix)
    {
        int index = str.LastIndexOf('.');

        string start = str[..index] + suffix;

        string end = str[index..];

        return start + end;
    }
}
