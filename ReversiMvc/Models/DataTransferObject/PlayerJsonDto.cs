// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

namespace ReversiMvc.Models.DataTransferObject;

public class PlayerJsonDto
{
    public string Token { get; set; }

    public string Color { get; set; }

    public string Name { get; set; }
}
