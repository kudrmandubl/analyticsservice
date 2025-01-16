using System;

namespace Modules.AnalyticsService.Models
{
    /// <summary>
    /// Cообщение
    /// </summary>
    [System.Serializable]
    public struct Message
    {
        /// <summary>
        /// Дата и время отправки
        /// </summary>
        public DateTime SendDateTime { get; set; }

        /// <summary>
        /// Содержимое сообщения
        /// </summary>
        public string Content { get; set; }
    }
}
