using Lab6.Http.Common;

internal class Program
{
    private static object _locker = new object();

    public static async Task Main(string[] args)
    {
        var httpClient = new HttpClient()
        {
            BaseAddress = new Uri("http://localhost:5214/api/")
        };

        var taskApiClient = new TaskApiClient(httpClient);

        await ManageTasks(taskApiClient);
    }

    private static async Task ManageTasks(ITaskApi taskApi)
    {
        PrintMenu();

        while (true)
        {
            var key = Console.ReadKey(true);

            PrintMenu();

            if (key.Key == ConsoleKey.D1)
            {
                var tasks = await taskApi.GetAllAsync();
                Console.WriteLine($"| Id    |   Nickname        | Donate | Description                      | ");
                foreach (var task in tasks)
                {
                    Console.WriteLine($"| {task.Id,5} | {task.Nickname,15} | {task.Donate,8} | {task.Description,20} |");
                }
            }

            if (key.Key == ConsoleKey.D2)
            {
                Console.Write("Введите id доната: ");
                var taskIdString = Console.ReadLine();
                int.TryParse(taskIdString, out var taskId);
                var task = await taskApi.GetAsync(taskId);
                if (task == null)
                {
                    Console.WriteLine("донат не найден.");
                }
                else
                {
                    Console.WriteLine($"| {task.Id,5} | {task.Nickname,15} | {task.Donate,8} | {task.Description,20} |");
                }
            }

            if (key.Key == ConsoleKey.D3)
            {
                Console.Write("Введите ник: ");
                var nick = Console.ReadLine() ?? "Без названия";
                Console.Write("Введите описание доната: ");
                var description = Console.ReadLine() ?? "Без описания";
                Console.Write("Сумма доната: ");
                var input = Console.ReadLine();
                int donate = int.Parse(input);
                var newTask = new TaskItem(
                    id: 0,
                    nickname: nick,
                    donate: donate,
                    desc:description
                );

                var addResult = await taskApi.AddAsync(newTask);
                Console.WriteLine(addResult ? "Донат добавлен." : "Ошибка добавления доната.");
            }

            if (key.Key == ConsoleKey.D4)
            {
                Console.Write("Введите id доната: ");
                var taskIdString = Console.ReadLine();
                int.TryParse(taskIdString, out var taskId);
                var task = await taskApi.GetAsync(taskId);
                if (task == null)
                {
                    Console.WriteLine("Донат не найден.");
                    continue;
                }

                Console.Write("Введите новую суммму доната: ");
                var newStatus = Console.ReadLine() ?? "1000";
                int don = int.Parse(newStatus);
                task.Donate = don;

                var updateResult = await taskApi.UpdateAsync(taskId, task);
                Console.WriteLine(updateResult ? "Донат обновлен." : "Ошибка обновления доната.");
            }

            if (key.Key == ConsoleKey.D5)
            {
                Console.Write("Введите id доната: ");
                var taskIdString = Console.ReadLine();
                int.TryParse(taskIdString, out var taskId);

                var deleteResult = await taskApi.DeleteAsync(taskId);
                Console.WriteLine(deleteResult ? "Донат удален." : "Ошибка удаления доната.");
            }

            if (key.Key == ConsoleKey.Escape)
            {
                break;
            }
        }
    }

    private static void PrintMenu()
    {
        lock (_locker)
        {
            Console.WriteLine("1 - Показать все донаты");
            Console.WriteLine("2 - Показать донат по id");
            Console.WriteLine("3 - Добавить донат");
            Console.WriteLine("4 - Обновить донат");
            Console.WriteLine("5 - Удалить донат");
            Console.WriteLine("-------");
        }
    }
}
