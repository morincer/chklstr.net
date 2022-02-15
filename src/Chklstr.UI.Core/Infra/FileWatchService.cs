using Chklstr.Core.Utils;
using Microsoft.Extensions.Logging;

namespace Chklstr.UI.Core.Infra;

public class FileWatchService
{
    private readonly ILogger<FileWatchService> _logger;

    private readonly object _lock = new();
    private readonly Dictionary<string, FileSystemWatcher> _watchers = new();
    private readonly Dictionary<string, HashSet<string>> _watchedPaths = new();

    public TimeSpan Delay { get; set; } = TimeSpan.FromMilliseconds(100);

    public FileWatchService(ILogger<FileWatchService> logger)
    {
        _logger = logger;
    }

    public void Register(string path)
    {
        _logger.LogDebug($"Registering file watch for {path}");
        path = Path.GetFullPath(path);

        lock (_lock)
        {
            var folder = Path.GetDirectoryName(path);
            if (folder == null) return;

            if (!_watchers.ContainsKey(folder))
            {
                var watcher = new FileSystemWatcher(folder)
                {
                    Path = folder,
                    Filter = "*.*",
                    NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite,
                    EnableRaisingEvents = true
                };

                watcher.Changed += OnChanged;
                _watchers[folder] = watcher;
                _watchedPaths[folder] = new HashSet<string>();
            }

            if (!_watchedPaths[folder].Contains(path))
            {
                _watchedPaths[folder].Add(path);
            }
        }
    }

    public void Unregister(string path)
    {
        _logger.LogDebug($"Unregistering file watch {path}");
        path = Path.GetFullPath(path);
        var folder = Path.GetDirectoryName(path);

        lock (_lock)
        {
            if (string.IsNullOrEmpty(folder)) return;

            if (!_watchers.ContainsKey(folder)) return;

            _watchedPaths[folder].Remove(path);
            
            if (!_watchedPaths[folder].IsEmpty()) return;
            
            _watchedPaths.Remove(folder);
            var watcher = _watchers[folder];
            watcher.EnableRaisingEvents = false;

            _watchers.Remove(folder);
            watcher.Dispose();
        }
    }

    public event Action<FileSystemEventArgs> FileChanged;

    private Queue<FileSystemEventArgs> _notificationQueue = new();
    private void OnChanged(object sender, FileSystemEventArgs e)
    {
        var folder = Path.GetDirectoryName(e.FullPath);
        if (folder == null) return;

        lock (_lock)
        {
            if (!_watchedPaths.ContainsKey(folder) || !_watchedPaths[folder].Contains(e.FullPath))
            {
                _logger.LogDebug($"No match for {folder} and {e.FullPath}");
                return;
            }
            
            _logger.LogDebug($"Enqueue FS change: {e.FullPath}, {e.ChangeType}");
            _notificationQueue.Enqueue(e);
        }

        Task.Delay(Delay).ContinueWith(_ => FlushQueue());
    }

    private void FlushQueue()
    {
        lock (_lock)
        {
            if (_notificationQueue.Count == 0) return;
            
            _logger.LogDebug($"Flushing notification queue ({_notificationQueue.Count})");
            var events = _notificationQueue
                .GroupBy(e => e.FullPath)
                .Select(g => g.Last());

            foreach (var e in events)
            {
                _logger.LogDebug($"Notify change {e.FullPath}");
                OnFileChanged(e);
            }
            
            _notificationQueue.Clear();
        }
    }

    protected virtual void OnFileChanged(FileSystemEventArgs obj)
    {
        FileChanged?.Invoke(obj);
    }
}