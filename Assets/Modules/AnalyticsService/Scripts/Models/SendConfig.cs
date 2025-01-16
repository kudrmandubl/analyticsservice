using System.Text;
using UnityEngine;

namespace Modules.AnalyticsService.Models
{
    /// <summary>
    /// Конфиг отправки
    /// </summary>
    [CreateAssetMenu(fileName = "SendConfig", menuName = "AnalyticsService/SendConfig")]
    public class SendConfig : ScriptableObject
    {
        [SerializeField] private string _url;
        [SerializeField] private string _contentType;
        [SerializeField] private float _resendTime;

        /// <summary>
        /// Урл куда оптравляют аналитику
        /// </summary>
        public string Url => _url;

        /// <summary>
        /// Тип содержимого
        /// </summary>
        public string ContentType => _contentType;

        /// <summary>
        /// Время через которое происходит попытка переотправки
        /// </summary>
        public float ResendTime => _resendTime;
    }
}
