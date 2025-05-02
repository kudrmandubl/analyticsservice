using System;
using Modules.AnalyticsService.Interfaces;
using UnityEngine;
using Zenject;

namespace Modules.AnalyticsService.Example
{
    public class ExampleMessageSender : MonoBehaviour
    {
        [Inject]
        IAnalyticsService _analyticsService;

        [ContextMenu("Send Example Message")]
        private void SendExampleMessage()
        {
            _analyticsService.SendMessageAsync($"Example message {DateTime.Now.ToString()}");
        }
    }
}