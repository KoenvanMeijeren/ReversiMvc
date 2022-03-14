// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using System.Net;
using System.Net.Http.Headers;
using ReversiMvc.Models.DataTransferObject;

namespace ReversiMvc.Repository;

public class GameRepository : IGameRepository
{
    private const string ApiUri = "https://localhost:7042/api/Game";

    /// <inheritdoc />
    Task<int> IAsyncRepository<GameJsonDto>.AddAsync(GameJsonDto entity)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public async Task<GameJsonDto?> AddAsync(GameJsonDto entity)
    {
        var client = GameRepository.CreateHttpClient($"{GameRepository.ApiUri}", "text/plain");
        var request = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress) { 
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
        var client = GameRepository.CreateHttpClient($"{GameRepository.ApiUri}/add/player-one", "text/plain");
        var request = new HttpRequestMessage(HttpMethod.Put, client.BaseAddress) { 
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
        var client = GameRepository.CreateHttpClient($"{GameRepository.ApiUri}/add/player-two", "text/plain");
        var request = new HttpRequestMessage(HttpMethod.Put, client.BaseAddress) { 
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
    public async Task<List<GameJsonDto>?> AllAsync()
    {
        var client = GameRepository.CreateHttpClient($"{GameRepository.ApiUri}/queue");
        var response = await client.GetAsync($"{GameRepository.ApiUri}/queue");
        
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
        
        var client = GameRepository.CreateHttpClient($"{GameRepository.ApiUri}/{token}");
        var response = await client.GetAsync($"{GameRepository.ApiUri}/{token}");
        
        return response.Content.ReadFromJsonAsync<GameJsonDto>().Result;
    }

    /// <inheritdoc />
    public async Task<GameJsonDto?> GetByPlayerToken(string? token)
    {
        var entities = await this.AllAsync();

        return entities?.SingleOrDefault(
            game => (game.PlayerOne is {Token: { }} && game.PlayerOne.Token.Equals(token))
                 || (game.PlayerTwo is {Token: { }} && game.PlayerTwo.Token.Equals(token))
        );
    }

    private static HttpClient CreateHttpClient(string uri, string accept = "application/json")
    {
        var client = new HttpClient();
        client.BaseAddress = new Uri(uri);
        client.Timeout = new TimeSpan(0, 0, 90);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(accept));

        return client;
    }
}
