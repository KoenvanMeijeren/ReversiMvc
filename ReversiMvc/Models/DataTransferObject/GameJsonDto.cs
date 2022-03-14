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

    public string? Description { get; set; }

    public string? Token { get; set; }

    public PlayerJsonDto? PlayerOne { get; set; }

    public PlayerJsonDto? PlayerTwo { get; set; }

    public PlayerJsonDto? CurrentPlayer { get; set; }

    public string? Board { get; set; }
    
    public string? Status { get; set; }

}
