using PROJ.Configuration;

namespace PROJ.Logging.Classes;

public sealed class Logger
{
    private static readonly Logger _instance = new Logger();
    private readonly LoggerBox _box;

    public static Logger GetInstance => _instance;

    private LogSettings? _settings { get; set; }

    public string filePath { get; set; } = string.Empty;

    private Logger()
    {
        LoggerMode = false;
        _box = new LoggerBox();
        _box.DisplayFrame();
        _settings = Configurator.Instance.ConfigureLog();
        if (_settings == null)
            throw new InvalidOperationException("Missing Logging configuration in appsettings.json.");

        filePath = _settings.OutPath.Replace("{date}", DateTime.Now.ToString("yyyy-MM-dd_HH-mm"));
        var directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(directory))
            Directory.CreateDirectory(directory);

        File.WriteAllText(filePath, $"Logs From Gameplay on day {DateTime.Now:yyyy-MM-dd_HH-mm}{Environment.NewLine}");
    }

    public void Log(string message)
    {
        _box.AddLog(message);
        File.AppendAllText(filePath, message + Environment.NewLine);
    }

    public void LogUp()
    {
        _box.LogUp();
    }

    public void LogDown()
    {
        _box.LogDown();
    }

    public bool LoggerMode { get; set; }
}
