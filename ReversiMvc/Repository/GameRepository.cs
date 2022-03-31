// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using System.Net.Http.Headers;
using ReversiMvc.Models.DataTransferObject;
using ReversiMvc.Services;

namespace ReversiMvc.Repository;

public class GameRepository : IGameRepository
{
    private static string s_apiUri = "";

    public GameRepository(ApiConfiguration apiConfiguration)
    {
        GameRepository.s_apiUri = apiConfiguration.Url;
    }

    /// <inheritdoc />
    Task<int> IAsyncRepository<GameJsonDto>.AddAsync(GameJsonDto entity)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public async Task<GameJsonDto?> AddAsync(GameJsonDto entity)
    {
        var client = GameRepository.CreateHttpClient("", "text/plain");
        var request = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress)
        {
            Content = JsonContent.Create(new
            {
                description = entity.Description,
            })
        };

        var response = await client.SendAsync(request);

        return !response.IsSuccessStatusCode ? null : response.Content.ReadFromJsonAsync<GameJsonDto>().Result;
    }

    /// <inheritdoc />
    public async Task<GameJsonDto?> AddPlayerOneAsync(string token, string playerGuid, string playerName)
    {
        var client = GameRepository.CreateHttpClient("/add/player-one", "text/plain");
        var request = new HttpRequestMessage(HttpMethod.Put, client.BaseAddress)
        {
            Content = JsonContent.Create(new
            {
                token,
                playerToken = playerGuid,
                name = playerName,
            })
        };
        var response = await client.SendAsync(request);

        return !response.IsSuccessStatusCode ? null : response.Content.ReadFromJsonAsync<GameJsonDto>().Result;
    }

    /// <inheritdoc />
    public async Task<GameJsonDto?> AddPlayerTwoAsync(string token, string playerGuid, string playerName)
    {
        var client = GameRepository.CreateHttpClient("/add/player-two", "text/plain");
        var request = new HttpRequestMessage(HttpMethod.Put, client.BaseAddress)
        {
            Content = JsonContent.Create(new
            {
                token,
                playerToken = playerGuid,
                name = playerName,
            })
        };
        var response = await client.SendAsync(request);

        return !response.IsSuccessStatusCode ? null : response.Content.ReadFromJsonAsync<GameJsonDto>().Result;
    }

    /// <inheritdoc />
    public async Task<GameJsonDto?> StartAsync(string token)
    {
        var client = GameRepository.CreateHttpClient($"/{token}/start", "text/plain");
        var request = new HttpRequestMessage(HttpMethod.Put, client.BaseAddress);
        var response = await client.SendAsync(request);

        return !response.IsSuccessStatusCode ? null : response.Content.ReadFromJsonAsync<GameJsonDto>().Result;
    }

    /// <inheritdoc />
    public async Task<GameJsonDto?> DoMoveAsync(GameDoMoveDto gameDoMoveDto)
    {
        var client = GameRepository.CreateHttpClient("/do-move", "text/plain");
        var request = new HttpRequestMessage(HttpMethod.Put, client.BaseAddress)
        {
            Content = JsonContent.Create(gameDoMoveDto)
        };
        var response = await client.SendAsync(request);

        return !response.IsSuccessStatusCode ? null : response.Content.ReadFromJsonAsync<GameJsonDto>().Result;
    }

    /// <inheritdoc />
    public async Task<GameJsonDto?> QuitAsync(string token)
    {
        var client = GameRepository.CreateHttpClient($"/{token}/quit", "text/plain");
        var request = new HttpRequestMessage(HttpMethod.Put, client.BaseAddress);
        var response = await client.SendAsync(request);

        return !response.IsSuccessStatusCode ? null : response.Content.ReadFromJsonAsync<GameJsonDto>().Result;
    }

    /// <inheritdoc />
    public async Task<List<GameJsonDto>?> AllAsync()
    {
        var client = GameRepository.CreateHttpClient("/all/all");
        var response = await client.GetAsync(client.BaseAddress);

        return !response.IsSuccessStatusCode ? null : response.Content.ReadFromJsonAsync<List<GameJsonDto>>().Result;
    }

    /// <inheritdoc />
    public Task<int> UpdateAsync(GameJsonDto entity)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task<int> DeleteAsync(GameJsonDto entity)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public async Task<bool> Exists(string? token)
    {
        return await this.Get(token) != null;
    }

    /// <inheritdoc />
    public async Task<GameJsonDto?> Get(string? token)
    {
        if (token == null)
        {
            return null;
        }

        var client = GameRepository.CreateHttpClient($"/{token}");
        var response = await client.GetAsync(client.BaseAddress);

        return !response.IsSuccessStatusCode ? null : response.Content.ReadFromJsonAsync<GameJsonDto>().Result;
    }

    /// <inheritdoc />
    public async Task<GameJsonDto?> GetByPlayerToken(string? token)
    {
        if (token == null)
        {
            return null;
        }

        var client = GameRepository.CreateHttpClient($"/player-one/{token}/active");
        var response = await client.GetAsync(client.BaseAddress);
        if (response.IsSuccessStatusCode)
        {
            return response.Content.ReadFromJsonAsync<GameJsonDto>().Result;
        }

        client = GameRepository.CreateHttpClient($"/player-two/{token}/active");
        response = await client.GetAsync(client.BaseAddress);

        return !response.IsSuccessStatusCode ? null : response.Content.ReadFromJsonAsync<GameJsonDto>().Result;
    }

    private static HttpClient CreateHttpClient(string uri, string accept = "application/json")
    {
        var client = new HttpClient();
        client.BaseAddress = new Uri($"{GameRepository.s_apiUri}{uri}");
        client.Timeout = new TimeSpan(0, 0, 90);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(accept));

        return client;
    }
}
