// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using NUnit.Framework;
using ReversiMvc.Models;
using ReversiMvc.Models.DataTransferObject;
using ReversiMvc.Models.Entities;

namespace Tests.Model;

[TestFixture]
public class GameEditViewModelTests
{

    [Test]
    public void GameEdit_NotEmpty()
    {
        // Arrange
        var viewModel = new GameEditViewModel(new GameJsonDto()
        {
            Id = 1,
            Description = "test",
            Board = "[[0,0,0,0,0,0,0,0],[0,0,0,0,0,0,0,0],[0,0,0,0,0,0,0,0],[0,0,0,1,2,0,0,0],[0,0,0,2,1,0,0,0],[0,0,0,0,0,0,0,0],[0,0,0,0,0,0,0,0],[0,0,0,0,0,0,0,0]]",
            Token = "abcdeff",
            PlayerOne = new PlayerJsonDto() { Color = "White", Name = "Teddy", Token = "qwerty" },
            PlayerTwo = new PlayerJsonDto() { Color = "Black", Name = "Jessica", Token = "zxcvb" },
            CurrentPlayer = new PlayerJsonDto() { Color = "Black", Name = "Jessica", Token = "zxcvb" },
            Status = "playing"
        });

        // Act

        // Assert
        Assert.AreEqual(1, viewModel.Id);
        Assert.AreEqual("test", viewModel.Description);
        Assert.AreEqual("abcdeff", viewModel.Token);
        Assert.IsInstanceOf<object>(viewModel.Board);
        Assert.AreEqual("qwerty", viewModel.PlayerOne.Token);
        Assert.AreEqual("zxcvb", viewModel.PlayerTwo.Token);
        Assert.AreEqual("zxcvb", viewModel.CurrentPlayer.Token);
        Assert.AreEqual(Status.Playing, viewModel.Status);
    }

    [Test]
    public void GameEdit_Empty()
    {
        // Arrange
        var game = new GameEditViewModel();
        // Act

        // Assert
        Assert.IsNull(game.Id);
        Assert.IsNull(game.Description);
        Assert.IsNull(game.Token);
        Assert.IsInstanceOf<PlayerDto>(game.PlayerOne);
        Assert.IsInstanceOf<PlayerDto>(game.PlayerTwo);
        Assert.IsInstanceOf<PlayerDto>(game.CurrentPlayer);
        Assert.AreEqual(new Color[1, 1], game.Board);
        Assert.AreEqual(Status.Created, game.Status);
    }

    [Test]
    public void Status_Convert()
    {
        // Arrange

        // Act

        // Assert
        Assert.AreEqual(Status.Created, new GameEditViewModel(new GameJsonDto() { Status = "created" }).Status);
        Assert.AreEqual(Status.Created, new GameEditViewModel(new GameJsonDto() { Status = "Created" }).Status);
        Assert.AreEqual(Status.Queued, new GameEditViewModel(new GameJsonDto() { Status = "queued" }).Status);
        Assert.AreEqual(Status.Queued, new GameEditViewModel(new GameJsonDto() { Status = "Queued" }).Status);
        Assert.AreEqual(Status.Pending, new GameEditViewModel(new GameJsonDto() { Status = "pending" }).Status);
        Assert.AreEqual(Status.Pending, new GameEditViewModel(new GameJsonDto() { Status = "Pending" }).Status);
        Assert.AreEqual(Status.Playing, new GameEditViewModel(new GameJsonDto() { Status = "playing" }).Status);
        Assert.AreEqual(Status.Playing, new GameEditViewModel(new GameJsonDto() { Status = "Playing" }).Status);
        Assert.AreEqual(Status.Finished, new GameEditViewModel(new GameJsonDto() { Status = "finished" }).Status);
        Assert.AreEqual(Status.Finished, new GameEditViewModel(new GameJsonDto() { Status = "Finished" }).Status);
        Assert.AreEqual(Status.Quit, new GameEditViewModel(new GameJsonDto() { Status = "quit" }).Status);
        Assert.AreEqual(Status.Quit, new GameEditViewModel(new GameJsonDto() { Status = "Quit" }).Status);
    }

    [Test]
    public void Can_AddPlayerOne()
    {
        // Arrange
        var viewModel = new GameEditViewModel(new GameJsonDto(), new PlayerEntity("test"));

        // Act

        // Assert
        Assert.IsTrue(viewModel.CanAddPlayerOne());
    }

    [Test]
    public void Cannot_AddPlayerOne()
    {
        // Arrange
        var viewModel = new GameEditViewModel(new GameJsonDto()
        {
            PlayerOne = new PlayerJsonDto() { Token = "qwert" }
        }, new PlayerEntity("test"));
        var viewModel1 = new GameEditViewModel();

        // Act

        // Assert
        Assert.IsFalse(viewModel.CanAddPlayerOne());
        Assert.IsFalse(viewModel1.CanAddPlayerOne());
    }

    [Test]
    public void Can_AddPlayerTwo()
    {
        // Arrange
        var viewModel = new GameEditViewModel(new GameJsonDto(), new PlayerEntity("test"));

        // Act

        // Assert
        Assert.IsTrue(viewModel.CanAddPlayerTwo());
    }

    [Test]
    public void Cannot_AddPlayerTwo()
    {
        // Arrange
        var viewModel = new GameEditViewModel(new GameJsonDto()
        {
            PlayerTwo = new PlayerJsonDto() { Token = "qwert" }
        }, new PlayerEntity("test"));

        var viewModel1 = new GameEditViewModel();

        // Act

        // Assert
        Assert.IsFalse(viewModel.CanAddPlayerTwo());
        Assert.IsFalse(viewModel1.CanAddPlayerTwo());
    }

    [Test]
    public void Can_Start()
    {
        // Arrange
        var viewModel = new GameEditViewModel(new GameJsonDto()
        {
            PlayerOne = new PlayerJsonDto() { Token = "vadfasd" },
            PlayerTwo = new PlayerJsonDto() { Token = "qwert" },
            Status = "pending",
        }, new PlayerEntity("vadfasd"));

        // Act

        // Assert
        Assert.IsTrue(viewModel.CanStart());
        Assert.IsFalse(viewModel.IsPlaying());
    }

    [Test]
    public void Cannot_Start()
    {
        // Arrange
        var viewModel = new GameEditViewModel(new GameJsonDto());
        var viewModel1 = new GameEditViewModel(new GameJsonDto()
        {
            PlayerOne = new PlayerJsonDto() { Token = "test" }
        });
        var viewModel2 = new GameEditViewModel(new GameJsonDto()
        {
            PlayerTwo = new PlayerJsonDto() { Token = "test" }
        });
        var viewModel3 = new GameEditViewModel(new GameJsonDto()
        {
            PlayerOne = new PlayerJsonDto() { Token = "test" },
            PlayerTwo = new PlayerJsonDto() { Token = "test" },
        });

        // Act

        // Assert
        Assert.IsFalse(viewModel.CanStart());
        Assert.IsFalse(viewModel.IsPlaying());
        Assert.IsFalse(viewModel1.CanStart());
        Assert.IsFalse(viewModel1.IsPlaying());
        Assert.IsFalse(viewModel2.CanStart());
        Assert.IsFalse(viewModel2.IsPlaying());
        Assert.IsFalse(viewModel3.CanStart());
        Assert.IsFalse(viewModel3.IsPlaying());
    }

    [Test]
    public void Can_Quit()
    {
        // Arrange
        var viewModel = new GameEditViewModel(new GameJsonDto()
        {
            PlayerOne = new PlayerJsonDto() { Token = "vadfasd" },
            PlayerTwo = new PlayerJsonDto() { Token = "qwert" },
            Status = "playing",
        }, new PlayerEntity("vadfasd"));

        // Act

        // Assert
        Assert.IsTrue(viewModel.CanQuit());
        Assert.IsFalse(viewModel.IsQuit());
    }

    [Test]
    public void Cannot_Quit()
    {
        // Arrange
        var viewModel = new GameEditViewModel(new GameJsonDto());
        var viewModel1 = new GameEditViewModel(new GameJsonDto()
        {
            PlayerOne = new PlayerJsonDto() { Token = "test" },
            Status = "quit"
        }, new PlayerEntity("test"));
        var viewModel2 = new GameEditViewModel(new GameJsonDto()
        {
            PlayerTwo = new PlayerJsonDto() { Token = "test" },
            Status = "pending"
        }, new PlayerEntity("test"));
        var viewModel3 = new GameEditViewModel(new GameJsonDto()
        {
            PlayerOne = new PlayerJsonDto() { Token = "test" },
            PlayerTwo = new PlayerJsonDto() { Token = "test" },
            Status = "playing"
        });

        // Act

        // Assert
        Assert.IsFalse(viewModel.CanQuit());
        Assert.IsFalse(viewModel.IsQuit());
        Assert.IsFalse(viewModel1.CanQuit());
        Assert.IsTrue(viewModel1.IsQuit());
        Assert.IsFalse(viewModel2.CanQuit());
        Assert.IsFalse(viewModel2.IsQuit());
        Assert.IsFalse(viewModel3.CanQuit());
        Assert.IsFalse(viewModel3.IsQuit());
    }
    
    [Test]
    public void IsPending()
    {
        // Arrange
        var viewModel = new GameEditViewModel(new GameJsonDto()
        {
            Status = "pending",
        });
        var viewModel1 = new GameEditViewModel(new GameJsonDto()
        {
            Status = "playing",
        });

        // Act

        // Assert
        Assert.IsTrue(viewModel.IsPending());
        Assert.IsFalse(viewModel1.IsPending());
    }
    
    [Test]
    public void IsFinished()
    {
        // Arrange
        var viewModel = new GameEditViewModel(new GameJsonDto()
        {
            Status = "finished",
        });
        var viewModel1 = new GameEditViewModel(new GameJsonDto()
        {
            Status = "playing",
        });

        // Act

        // Assert
        Assert.IsTrue(viewModel.IsFinished());
        Assert.IsFalse(viewModel1.IsFinished());
    }
    
    [Test]
    public void IsPlayingOrEnded()
    {
        // Arrange
        var viewModel = new GameEditViewModel(new GameJsonDto()
        {
            Status = "finished",
        });
        var viewModel1 = new GameEditViewModel(new GameJsonDto()
        {
            Status = "playing",
        });
        var viewModel2 = new GameEditViewModel(new GameJsonDto()
        {
            Status = "quit",
        });
        var viewModel3 = new GameEditViewModel(new GameJsonDto()
        {
            Status = "created",
        });

        // Act

        // Assert
        Assert.IsTrue(viewModel.IsPlayingOrEnded());
        Assert.IsTrue(viewModel1.IsPlayingOrEnded());
        Assert.IsTrue(viewModel2.IsPlayingOrEnded());
        Assert.IsFalse(viewModel3.IsPlayingOrEnded());
    }
    
    [Test]
    public void IsLoggedInPlayerOwner()
    {
        // Arrange
        var playerOne = new PlayerJsonDto() {Color = "White", Name = "Teddy", Token = "qwerty"};
        var playerTwo = new PlayerJsonDto() {Color = "Black", Name = "Jessica", Token = "zxcvb"};
        
        var viewModel = new GameEditViewModel(new GameJsonDto()
        {
            PlayerOne = playerOne,
            PlayerTwo = playerTwo
        }, new PlayerEntity(playerOne.Token));
        var viewModel1 = new GameEditViewModel(new GameJsonDto()
        {
            PlayerOne = playerOne,
            PlayerTwo = playerTwo
        }, new PlayerEntity(playerTwo.Token));

        // Act

        // Assert
        Assert.IsTrue(viewModel.IsLoggedInPlayerOwner());
        Assert.IsFalse(viewModel1.IsLoggedInPlayerOwner());
    }
    
    [Test]
    public void PlayerOne_LoggedIn_PlayerTwoIsOpponent()
    {
        // Arrange
        var playerOne = new PlayerJsonDto() {Color = "White", Name = "Teddy", Token = "qwerty"};
        var playerTwo = new PlayerJsonDto() {Color = "Black", Name = "Jessica", Token = "zxcvb"};
        
        var viewModel = new GameEditViewModel(new GameJsonDto()
        {
            PlayerOne = playerOne,
            PlayerTwo = playerTwo
        }, new PlayerEntity(playerOne.Token, playerOne.Name));
        
        // Act

        // Assert
        Assert.AreEqual(playerTwo.Name, viewModel.Opponent);
    }
    
    [Test]
    public void PlayerTwo_LoggedIn_PlayerOneIsOpponent()
    {
        // Arrange
        var playerOne = new PlayerJsonDto() {Color = "White", Name = "Teddy", Token = "qwerty"};
        var playerTwo = new PlayerJsonDto() {Color = "Black", Name = "Jessica", Token = "zxcvb"};
        
        var viewModel = new GameEditViewModel(new GameJsonDto()
        {
            PlayerOne = playerOne,
            PlayerTwo = playerTwo
        }, new PlayerEntity(playerTwo.Token, playerTwo.Name));
        
        // Act

        // Assert
        Assert.AreEqual(playerOne.Name, viewModel.Opponent);
    }
    
    [Test]
    public void PlayerOne_Dominant_LoggedIn()
    {
        // Arrange
        var playerOne = new PlayerJsonDto() {Color = "White", Name = "Teddy", Token = "qwerty"};
        var playerTwo = new PlayerJsonDto() {Color = "Black", Name = "Jessica", Token = "zxcvb"};
        
        var viewModel = new GameEditViewModel(new GameJsonDto()
        {
            PlayerOne = playerOne,
            PlayerTwo = playerTwo,
            Status = "finished",
            PredominantColor = "White"
        }, new PlayerEntity(playerOne.Token, playerOne.Name));
        
        // Act

        // Assert
        Assert.AreEqual("White", viewModel.PredominantColor);
        Assert.AreEqual("Ik", viewModel.PredominantPlayer);
        Assert.AreEqual("Teddy", viewModel.DominantPlayerDto.Name);
        Assert.AreEqual("Jessica", viewModel.LoserPlayerDto.Name);
    }
    
    [Test]
    public void PlayerOne_Dominant_NotLoggedIn()
    {
        // Arrange
        var playerOne = new PlayerJsonDto() {Color = "White", Name = "Teddy", Token = "qwerty"};
        var playerTwo = new PlayerJsonDto() {Color = "Black", Name = "Jessica", Token = "zxcvb"};
        
        var viewModel = new GameEditViewModel(new GameJsonDto()
        {
            PlayerOne = playerOne,
            PlayerTwo = playerTwo,
            Status = "finished",
            PredominantColor = "White"
        }, new PlayerEntity(playerTwo.Token, playerTwo.Name));
        
        // Act

        // Assert
        Assert.AreEqual("White", viewModel.PredominantColor);
        Assert.AreEqual("Tegenstander", viewModel.PredominantPlayer);
        Assert.AreEqual("Teddy", viewModel.DominantPlayerDto.Name);
        Assert.AreEqual("Jessica", viewModel.LoserPlayerDto.Name);
    }
    
    [Test]
    public void PlayerTwo_Dominant_LoggedIn()
    {
        // Arrange
        var playerOne = new PlayerJsonDto() {Color = "White", Name = "Teddy", Token = "qwerty"};
        var playerTwo = new PlayerJsonDto() {Color = "Black", Name = "Jessica", Token = "zxcvb"};
        
        var viewModel = new GameEditViewModel(new GameJsonDto()
        {
            PlayerOne = playerOne,
            PlayerTwo = playerTwo,
            Status = "finished",
            PredominantColor = "Black"
        }, new PlayerEntity(playerOne.Token, playerOne.Name));
        
        // Act

        // Assert
        Assert.AreEqual("Black", viewModel.PredominantColor);
        Assert.AreEqual("Tegenstander", viewModel.PredominantPlayer);
        Assert.AreEqual("Jessica", viewModel.DominantPlayerDto.Name);
        Assert.AreEqual("Teddy", viewModel.LoserPlayerDto.Name);
    }
    
    [Test]
    public void PlayerTwo_Dominant_NotLoggedIn()
    {
        // Arrange
        var playerOne = new PlayerJsonDto() {Color = "White", Name = "Teddy", Token = "qwerty"};
        var playerTwo = new PlayerJsonDto() {Color = "Black", Name = "Jessica", Token = "zxcvb"};
        
        var viewModel = new GameEditViewModel(new GameJsonDto()
        {
            PlayerOne = playerOne,
            PlayerTwo = playerTwo,
            Status = "finished",
            PredominantColor = "Black"
        }, new PlayerEntity(playerTwo.Token, playerTwo.Name));
        
        // Act

        // Assert
        Assert.AreEqual("Black", viewModel.PredominantColor);
        Assert.AreEqual("Ik", viewModel.PredominantPlayer);
        Assert.AreEqual("Jessica", viewModel.DominantPlayerDto.Name);
        Assert.AreEqual("Teddy", viewModel.LoserPlayerDto.Name);
    }
}
