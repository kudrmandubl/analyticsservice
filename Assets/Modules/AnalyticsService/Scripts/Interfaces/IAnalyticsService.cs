using System.Threading.Tasks;

namespace Modules.AnalyticsService.Interfaces
{
    /// <summary>
    /// Сервис для отправки аналитики
    /// </summary>
    public interface IAnalyticsService
    {
        /// <summary>
        /// Отправить сообщение
        /// </summary>
        /// <param name="eventData">Отправляемые данные</param>
        /// <returns>void</returns>
        Task SendMessageAsync(string eventData);
    }
}
