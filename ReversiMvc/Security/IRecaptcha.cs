// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using System.Net;

namespace ReversiMvc.Security;

public interface IRecaptcha
{
    public const string InvalidMessage = "De reCAPTCHA is ongeldig!";

    public string SiteKey { get; }

    /// <summary>
    /// Validates te reCAPTCHA request.
    /// </summary>
    /// <param name="formCollection">The form collection.</param>
    /// <param name="clientIp">The IP of the client.</param>
    /// <returns>Whether the client passed the recaptcha check or not.</returns>
    Task<bool> Validate(IFormCollection formCollection, IPAddress? clientIp);
}
