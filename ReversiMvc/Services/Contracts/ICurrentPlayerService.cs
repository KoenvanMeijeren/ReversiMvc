// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

namespace ReversiMvc.Services.Contracts;

public interface ICurrentPlayerService
{
    /// <summary>
    /// Gets the current player or creates it and returns if not present.
    /// </summary>
    /// <returns>The current player.</returns>
    public PlayerEntity Get();

    /// <summary>
    /// Sets the current player.
    /// </summary>
    public void Set();
}
