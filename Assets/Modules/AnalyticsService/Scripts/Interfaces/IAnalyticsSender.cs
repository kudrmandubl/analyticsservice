using Modules.AnalyticsService.Models;
using System.Threading.Tasks;

namespace Modules.AnalyticsService.Interfaces
{
    /// <summary>
    /// Сущность для отправки сообщений аналитики.
    /// Возможны разные реализации, например, отправка на урл, запись в локальное хранилище, вызов метода api или др.
    /// </summary>
    public interface IAnalyticsSender
    {
        /// <summary>
        /// Отправить сообщение
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <returns>Успешность отправки</returns>
        Task<bool> SendMessageAsync(Message message);
    }
}
