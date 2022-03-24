// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

namespace ReversiMvc.Middleware;

/// <summary>
/// Ensures that the important headers are added to the response.
/// </summary>
public class ResponseHeaders
{
    private readonly RequestDelegate _next;

    public ResponseHeaders(RequestDelegate next)
    {
        this._next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var headers = context.Response.Headers;
        
        // Security settings.
        headers.XXSSProtection = "1; mode=block";
        headers.XFrameOptions = "DENY";
        headers.Referer = "no-referrer";
        
        // General settings.
        headers.CacheControl = "no-cache, no-store, must-revalidate";
        headers.XContentTypeOptions = "nosniff";
        if (context.Request.Method == "POST")
        {
            headers.XContentTypeOptions = "nosniff";
        }
        
        await this._next.Invoke(context);
    }
}
