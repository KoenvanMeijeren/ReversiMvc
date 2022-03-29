// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

namespace ReversiMvc.Models;

public class GameOverviewViewModel
{
    public IEnumerable<GameEditViewModel>? Games { get; }

    public PlayerEntity LoggedInPlayer { get; }

    public GameOverviewViewModel(IEnumerable<GameEditViewModel>? games, PlayerEntity loggedInPlayer)
    {
        this.Games = games;
        this.LoggedInPlayer = loggedInPlayer;
    }
}
