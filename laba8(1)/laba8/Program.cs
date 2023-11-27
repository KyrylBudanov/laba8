using System;

public class ConfigurationManager
{
    private static ConfigurationManager instance;
    private string loggingMode;
    private string databaseConnection;

    // Приватний конструктор для запобігання створення екземпляра через конструктор
    private ConfigurationManager() { }

    // Метод для отримання єдиного екземпляру ConfigurationManager
    public static ConfigurationManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ConfigurationManager();
            }
            return instance;
        }
    }

    // Метод для встановлення параметрів конфігурації
    public void SetConfiguration(string loggingMode, string databaseConnection)
    {
        this.loggingMode = loggingMode;
        this.databaseConnection = databaseConnection;
    }

    // Метод для виведення параметрів конфігурації
    public void DisplayConfiguration()
    {
        Console.WriteLine($"Logging Mode: {loggingMode}");
        Console.WriteLine($"Database Connection: {databaseConnection}");
    }
}

class Program
{
    static void Main()
    {
        // Отримання єдиного екземпляру ConfigurationManager
        ConfigurationManager configManager = ConfigurationManager.Instance;

        // Встановлення конфігурації через консольний інтерфейс
        Console.Write("Enter logging mode: ");
        string loggingMode = Console.ReadLine();

        Console.Write("Enter database connection: ");
        string databaseConnection = Console.ReadLine();

        // Встановлення конфігурації
        configManager.SetConfiguration(loggingMode, databaseConnection);

        // Виведення конфігурації
        Console.WriteLine("\nCurrent Configuration:");
        configManager.DisplayConfiguration();

        // Додаткові операції або використання конфігурації можуть бути додані за потребою
    }
}
