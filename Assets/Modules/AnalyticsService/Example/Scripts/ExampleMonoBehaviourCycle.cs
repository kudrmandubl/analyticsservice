using System;
using UnityEngine;

namespace Assets.Modules.AnalyticsService.Example.Scripts
{
    /// <summary>
    /// Вариант реализации класс для взаимодействия с циклом жизни MonoBehaviour
    /// </summary>
    public class ExampleMonoBehaviourCycle : MonoBehaviour
    {
        private static ExampleMonoBehaviourCycle _instance;

        public static Action OnUpdate;
        public static Action<bool> OnApplicationFocusChange;

        /// <summary>
        /// Создать экземпляр
        /// </summary>
        public static void Create()
        {
            var go = new GameObject();
            go.name = typeof(ExampleMonoBehaviourCycle).ToString();
            var component = go.AddComponent<ExampleMonoBehaviourCycle>();
            component.ProvideSingleton();
        }

        /// <summary>
        /// Каждый кадр
        /// </summary>
        private void Update()
        {
            OnUpdate?.Invoke();
        }

        /// <summary>
        /// Вызывается при смене фокуса приложения
        /// </summary>
        /// <param name="focus">Значение фокуса</param>
        private void OnApplicationFocus(bool focus)
        {
            OnApplicationFocusChange?.Invoke(focus);
        }

        /// <summary>
        /// Обеспечить один инстанс
        /// </summary>
        private void ProvideSingleton()
        {
            if (_instance && _instance != this) 
            {
                Destroy(gameObject);
            }
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
