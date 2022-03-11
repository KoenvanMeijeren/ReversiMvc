// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using NUnit.Framework;
using ReversiMvc.Models;

namespace Tests.Model;

[TestFixture]
public class ErrorViewModelTests
{
    [Test]
    public void Create_Notempty()
    {
        // Arrange
        var model = new ErrorViewModel { RequestId = "test" };

        // Act

        // Assert
        Assert.AreEqual("test", model.RequestId);
        Assert.IsTrue(model.ShowRequestId);
    }

    [Test]
    public void Create_Empty()
    {
        // Arrange
        var model = new ErrorViewModel();

        // Act

        // Assert
        Assert.IsNull(model.RequestId);
        Assert.IsFalse(model.ShowRequestId);
    }
}
