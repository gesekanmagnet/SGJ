using System;

public enum GameResult { Lose, Win }

public static class EventCallback
{
    public static Action<float> OnScore {  get; set; } = delegate { };
    public static Action<GameResult> OnGameOver { get; set; } = delegate { };
}