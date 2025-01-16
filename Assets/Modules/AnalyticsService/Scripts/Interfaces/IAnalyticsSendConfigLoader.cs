
using Modules.AnalyticsService.Models;
using System.Threading.Tasks;

namespace Modules.AnalyticsService.Interfaces
{
    /// <summary>
    /// Сущность для загрузки конфига с настройками отправки сообщений аналитики.
    /// Возможны разные реализации, например, загрузку из ресурсов, получение конфига с сервера или др.
    /// </summary>
    public interface IAnalyticsSendConfigLoader
    {
        /// <summary>
        /// Получить конфиг
        /// </summary>
        /// <returns>Загруженный конфиг</returns>
        SendConfig GetConfig();
    }
}
