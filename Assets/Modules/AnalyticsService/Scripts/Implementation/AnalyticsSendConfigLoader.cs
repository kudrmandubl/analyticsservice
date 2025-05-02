using Modules.AnalyticsService.Interfaces;
using Modules.AnalyticsService.Models;
using UnityEngine;

namespace Modules.AnalyticsService.Implementation
{
    /// <summary>
    /// Сущность для загрузки конфига с настройками отправки сообщений аналитики из ресурсов.
    /// Один из возможных вариантов реализации <see cref="IAnalyticsSendConfigLoader" />
    /// </summary>
    public class AnalyticsSendConfigLoader : IAnalyticsSendConfigLoader
    {
        private const string ConfigPath = "Configs/SendConfig";

        ///  <inheritdoc />
        public SendConfig GetConfig()
        {
            var config = Resources.Load<SendConfig>(ConfigPath);
            return config;
        }


    }
}
