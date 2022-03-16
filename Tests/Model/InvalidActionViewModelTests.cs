// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using NUnit.Framework;
using ReversiMvc.Models;

namespace Tests.Model;

[TestFixture]
public class InvalidViewModelTests
{
    [Test]
    public void Create_Notempty()
    {
        // Arrange
        var model = new InvalidActionViewModel { Message = "test" };

        // Act

        // Assert
        Assert.AreEqual("test", model.Message);
    }

    [Test]
    public void Create_Empty()
    {
        // Arrange
        var model = new InvalidActionViewModel();

        // Act

        // Assert
        Assert.IsNull(model.Message);
    }
}
