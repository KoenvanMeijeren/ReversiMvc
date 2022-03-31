using System.ComponentModel.DataAnnotations;

namespace ReversiMvc.Models.DataTransferObject;

public enum Color
{
    None,
    White,
    Black
}

public enum Status
{
    Created,
    Queued,
    Pending,
    Playing,
    Finished,
    Quit
}

public class GameJsonDto : IEntity
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Het beschrijvingsveld is verplicht!")]
    public string? Description { get; set; }

    public string? Token { get; set; }

    public PlayerJsonDto? PlayerOne { get; set; }

    public PlayerJsonDto? PlayerTwo { get; set; }

    public PlayerJsonDto? CurrentPlayer { get; set; }

    public string? PredominantColor { get; set; }
    public string? Board { get; set; }
    public string? PossibleMoves { get; set; }

    public string? Status { get; set; }

    public int ConqueredWhiteFiches { get; set; }
    public int ConqueredBlackFiches { get; set; }

}
