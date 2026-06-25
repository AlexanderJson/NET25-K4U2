using System;
using System.Collections.Generic;

namespace ContentApi.Common;

public sealed class ExternalApiException : Exception
{
    public int StatusCode { get; }
    public string? ResponseBody { get; }
    public IDictionary<string, string?>? Headers { get; }

    public ExternalApiException(int statusCode, string? responseBody = null, IDictionary<string, string?>? headers = null)
        : base($"External API error: {statusCode}")
    {
        StatusCode = statusCode;
        ResponseBody = responseBody;
        Headers = headers;
    }
}
