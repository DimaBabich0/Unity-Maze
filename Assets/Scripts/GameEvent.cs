public class GameEvent
{
    public string type { get; set; } = null;
    public object payload { get; set; } = null;
    public string toast { get; set; } = null;
    public float toastTimer { get; set; } = float.NaN;
    public string sound { get; set; } = null;
}
