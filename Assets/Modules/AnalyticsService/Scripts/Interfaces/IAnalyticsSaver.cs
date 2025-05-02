using Modules.AnalyticsService.Models;

namespace Modules.AnalyticsService.Interfaces
{
    /// <summary>
    /// Сущность для сохранения списка неотправленных сообщений
    /// </summary>
    public interface IAnalyticsSaver
    {
        /// <summary>
        /// Добавить неотправленное сообщение
        /// </summary>
        /// <param name="message">Сообщение</param>
        void AddUnsentMessage(Message message);

        /// <summary>
        /// Убрать неотправленное сообщение из списка
        /// </summary>
        /// <param name="message"></param>
        void RemoveUnsentMessage(Message message);

        /// <summary>
        /// Получить первое неотправленное сообщение
        /// </summary>
        /// <returns></returns>
        Message? GetFirstUnsentMessage();

        /// <summary>
        /// Проверить содержится ли сообщение в списке неотправленных сообщений
        /// </summary>
        bool Contains(Message message);

        /// <summary>
        /// Сохранить неотправленные сообщения
        /// </summary>
        void Save();

        /// <summary>
        /// Загрузить неотправленные сообщения
        /// </summary>
        void Load();
    }
}
