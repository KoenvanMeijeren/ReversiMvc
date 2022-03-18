// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using System.Configuration;
using System.Net;
using BitArmory.ReCaptcha;
using Microsoft.Extensions.Options;

namespace ReversiMvc.Security;

public class Recaptcha : IRecaptcha
{
    private readonly RecaptchaConfiguration _configuration;
    public string SiteKey => this._configuration.SiteKey;

    public Recaptcha(RecaptchaConfiguration configuration)
    {
        this._configuration = configuration;
    }

    /// <inheritdoc />
    public async Task<bool> Validate(IFormCollection formCollection, IPAddress? clientIp)
    {
        if (clientIp == null)
        {
            return false;
        }

        string? captchaResponse = null;
        if (formCollection.TryGetValue(Constants.ClientResponseKey, out var formField))
        {
            captchaResponse = formField;
        }

        if (string.IsNullOrEmpty(captchaResponse))
        {
            return false;
        }

        var captchaApi = new ReCaptchaService();

        return await captchaApi.Verify2Async(captchaResponse, clientIp.ToString(), this._configuration.PrivateKey);
    }

}
