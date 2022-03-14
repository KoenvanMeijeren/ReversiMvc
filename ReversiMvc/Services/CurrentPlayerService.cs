// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using ReversiMvc.Services.Contracts;

namespace ReversiMvc.Services;

public class CurrentPlayerService : ICurrentPlayerService
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IPlayersRepository _playersRepository;

    private PlayerEntity? _currentPlayer;

    public CurrentPlayerService(ICurrentUserService currentUserService, IPlayersRepository playersRepository)
    {
        this._currentUserService = currentUserService;
        this._playersRepository = playersRepository;
    }

    /// <inheritdoc/>
    public PlayerEntity Get()
    {
        if (this._currentPlayer != null)
        {
            return this._currentPlayer;
        }

        // Todo: find out how to do this when a sign in event occurs.
        this.Set();

        return this._currentPlayer!;
    }

    /// <inheritdoc/>
    public void Set()
    {
        var user = this._currentUserService.Identity;
        var currentUserGuid = this._currentUserService.Guid;
        if (currentUserGuid != null && user != null)
        {
            this._currentPlayer = this._playersRepository.FirstOrCreate(new PlayerEntity { Guid = currentUserGuid, Name = user.Name });
            return;
        }

        this._currentPlayer = this._playersRepository.Get(currentUserGuid)!;
    }

}
