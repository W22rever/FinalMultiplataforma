using System;

public static class GameEvents
{
    public static Action<int> OnScoreAdded;       // recibe puntos
    public static Action<int> OnLifeChanged;      // vida actual
    public static Action<int> OnTimerChanged;      // temporizador
    public static Action OnGameStop;              // paramos el gameplay
    public static Action<string> OnEndGame; 
}
