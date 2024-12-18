

## 📜 Объяснение кода

### 1. Основной класс и метод `Main`

```csharp
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
}
```

- **Класс `Program`**: Это основной класс вашего приложения. Он содержит метод `Main`, который является точкой входа в программу. 🚀
- **`_locker`**: Объект для блокировки, который используется для обеспечения потокобезопасности при выводе в консоль. 🔒
- **`HttpClient`**: Создаётся экземпляр `HttpClient`, который будет использоваться для отправки HTTP-запросов к API. 🌐
- **`BaseAddress`**: Устанавливает базовый адрес API, к которому будет подключаться клиент. 🏠
- **`ManageTasks(taskApiClient)`**: Асинхронный вызов метода для управления задачами (донатами). 📋

### 2. Метод `ManageTasks`

```csharp
private static async Task ManageTasks(ITaskApi taskApi)
{
    PrintMenu();

    while (true)
    {
        var key = Console.ReadKey(true);

        PrintMenu();
```

- **`ManageTasks`**: Этот метод отвечает за взаимодействие с пользователем и управление донатами. 💬
- **Цикл `while (true)`**: Бесконечный цикл, который позволяет пользователю выполнять действия до тех пор, пока он не нажмёт клавишу `Escape`. 🕹️

### 3. Вывод меню

```csharp
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
```

- **`PrintMenu`**: Метод, который выводит доступные действия на экран. 🖥️
- **Блокировка с помощью `_locker`**: Обеспечивает, что вывод в консоль не будет конфликтовать между потоками. 🤝

### 4. Обработка пользовательских действий

Каждый блок `if` проверяет нажатую клавишу и выполняет соответствующее действие:

#### 4.1 Показать все донаты

```csharp
if (key.Key == ConsoleKey.D1)
{
    var tasks = await taskApi.GetAllAsync();
    // Вывод информации о каждом донате
}
```

- При нажатии `1`, приложение запрашивает все донаты и выводит их в таблице. 📊

#### 4.2 Показать донат по ID

```csharp
if (key.Key == ConsoleKey.D2)
{
    // Запрос ID и вывод информации о конкретном донате
}
```

- При нажатии `2`, пользователь может ввести ID и получить информацию о конкретном донате. 🔍

#### 4.3 Добавить новый донат

```csharp
if (key.Key == ConsoleKey.D3)
{
    // Запрос данных для нового доната и добавление его в систему
}
```

- При нажатии `3`, приложение запрашивает информацию для нового доната и добавляет его в систему. 🎉💰

#### 4.4 Обновить существующий донат

```csharp
if (key.Key == ConsoleKey.D4)
{
    // Запрос ID и новой суммы для обновления существующего доната
}
```

- При нажатии `4`, пользователь может обновить сумму существующего доната по его ID. ✏️🔄

#### 4.5 Удалить донат

```csharp
if (key.Key == ConsoleKey.D5)
{
    // Запрос ID и удаление соответствующего доната
}
```

- При нажатии `5`, приложение удаляет донат по указанному ID. ❌🗑️

### 5. Завершение работы программы

```csharp
if (key.Key == ConsoleKey.Escape)
{
    break;
}
```

- Если пользователь нажимает клавишу `Escape`, программа завершает свою работу, выходя из цикла. 🚪✨

## 🎉 Заключение

Этот код представляет собой простое консольное приложение для управления донатами с использованием асинхронных запросов к API. Он позволяет пользователям легко взаимодействовать с системой, добавлять, обновлять, удалять и просматривать информацию о донатах! Надеюсь, это объяснение было полезным и понятным! 🌈💖

## Работа программы
![](https://github.com/e90rt/HttpDonate/blob/master/comand.png)
