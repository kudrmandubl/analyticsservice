using System.Collections.Generic;

namespace Modules.AnalyticsService.Models
{
    /// <summary>
    /// Неотправленные сообщения
    /// </summary>
    [System.Serializable]
    public class UnsentMessages
    {
        public LinkedList<Message> Messages { get; set; }
    }
}
