using System;
using System.Threading.Tasks;

namespace HttpClientFactory_Directly.Helpers
{
    public static class RetryHelper
    {
        public static async Task RetryOnExceptionAsync(int times, int delay, Func<Task> operation)
        {
            var attempts = 0;

            do
            {
                try
                {
                    attempts++;
                    Console.WriteLine($"Attempt Times: {attempts}");
                    await operation();
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Request failed. More details: {ex.Message}.");

                    if (attempts == times)
                    {
                        Console.WriteLine($"Max Attempt excceded :( ");
                        throw;
                    }

                    Console.WriteLine($"Next Attempt in {delay / 1000} seconds.");
                    await Task.Delay(delay);
                }
            } while (true);
        }
    }
}
