using Assets.Modules.AnalyticsService.Example.Scripts;
using Modules.AnalyticsService.Interfaces;
using System;
using System.Collections;
using UnityEngine;

namespace Modules.AnalyticsService.Example
{
    /// <summary>
    /// Депонстрационный вариант использования сервиса аналитики
    /// </summary>
    public class ExampleEntryPoint
    {
        private static IAnalyticsService _analyticsService;

        /// <summary>
        /// Инциализация точки входа после загрузки первой сцены
        /// </summary>
        [RuntimeInitializeOnLoadMethod]
        private static void Initialize()
        {
            _analyticsService = new AnalyticsService.Implementation.AnalyticsService();
            _analyticsService.SendMessageAsync("init message");

            ExampleMonoBehaviourCycle.Create();
        }
    }
}
