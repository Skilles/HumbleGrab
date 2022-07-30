namespace HumbleGrab.Humble.Utilities.Extensions;

public static class HttpExtensions
{
    public static IEnumerable<string> ToSeparateEndpoints(this IEnumerable<string> parameters, string endpoint, int maxParameters)
    {
        var tempParams = new List<string>();
        var endpoints = new List<string>();
        foreach (var param in parameters)
        {
            tempParams.Add(param);
            if (tempParams.Count == 40)
            {
                endpoints.Add(tempParams.Aggregate(endpoint, (current, key) => current + $"&gamekeys={key}"));
                tempParams.Clear();
            }
        }

        return endpoints;
    }
}