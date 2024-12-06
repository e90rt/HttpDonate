using System.Collections.Concurrent;
using Lab6.Http.Common;

class TaskStorage : StorageBase<TaskItem>, ITaskApi
{
    private static readonly Dictionary<int, TaskItem> defaultData
        = new Dictionary<int, TaskItem>()
        {
            [1] = new TaskItem(1, "Killer228", 300,"Шахматист я пока не могу смотреть стрим, так что выключай"),
            [2] = new TaskItem(2, "Sasavot",500, "Зверя нет сильнее ламы, сасавот сын лучшей мамы"),
            [3] = new TaskItem(3, "Phimoz2005",200, "А можешь занять 200 рублей"),
        };

    private static ConcurrentDictionary<int, TaskItem> taskRepository
        = new ConcurrentDictionary<int, TaskItem>();

    private static int _lastId;

    public TaskStorage(IDataSerializer<TaskItem[]> dataSerializer)
        : base(Path.Combine(
            "Data",
            dataSerializer.SerializerType,
            $"{nameof(TaskItem)}.{dataSerializer.SerializerType}"
        ),
        dataSerializer)
    {
        var readData = ReadAsync().Result;

        taskRepository = readData?.Any() == true
            ? new ConcurrentDictionary<int, TaskItem>(
                readData.ToDictionary(r => r.Id, r => r))
            : new ConcurrentDictionary<int, TaskItem>(defaultData);

        _lastId = taskRepository.Count + 1;
    }

    public async Task<bool> AddAsync(TaskItem newTask)
    {
        newTask.Id = _lastId;

        if (taskRepository.ContainsKey(_lastId))
        {
            return false;
        }

        var result = taskRepository.TryAdd(_lastId, newTask);

        if (!result)
        {
            return false;
        }
        Interlocked.Increment(ref _lastId);

        result = await WriteAsync(taskRepository.Values.ToArray());

        return result;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        if (!taskRepository.ContainsKey(id))
        {
            return false;
        }

        var result = taskRepository.Remove(id, out var _);
        if (!result)
        {
            return false;
        }

        result = await WriteAsync(taskRepository.Values.ToArray());

        return result;
    }

    public Task<TaskItem[]> GetAllAsync()
    {
        return Task.FromResult(taskRepository.Values.ToArray());
    }

    public Task<TaskItem?> GetAsync(int id)
    {
        if (!taskRepository.ContainsKey(id))
        {
            return Task.FromResult(default(TaskItem));
        }

        return Task.FromResult<TaskItem?>(taskRepository[id]);
    }

    public async Task<bool> UpdateAsync(int id, TaskItem updateTask)
    {
        if (!taskRepository.ContainsKey(id))
        {
            return false;
        }

        var result = taskRepository.TryUpdate(id, updateTask, taskRepository[id]);
        if (!result)
        {
            return false;
        }

        result = await WriteAsync(taskRepository.Values.ToArray());

        return result;
    }
}