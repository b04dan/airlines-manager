using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Airlines.Models
{
    //TODO: переделать систему валидации
    // ValidatableModelBase я придумал сам и только через некоторое время нашел несколько
    // гораздо более хороших решений. Например: https://stackoverflow.com/questions/2079552/wpf-textbox-validation

    // ValidatableModelBase наследуется от ModelBase и пререопределяет метод SetProperty<T>,
    // добавляя поддержку интерфейса IDataErrorInfo
    public class ValidatableModelBase : ModelBase, IDataErrorInfo
    {
        protected new bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            return SetProperty(ref field, newValue, null, null, propertyName);
        }

        // кроме поля и нового значения передается предикат, который проверяет, является ли переданное значение корректным.
        // если значение не подходит, в словарь ошибок добавляется заданное описание ошибки
        protected bool SetProperty<T>(ref T field, T newValue, Func<T, bool> errorPredicate,
            string errorInfo = null, [CallerMemberName]string propertyName = null)
        {
            base.SetProperty(ref field, newValue, propertyName);

            if (errorPredicate != null && errorPredicate(newValue))
                _errors[propertyName] = errorInfo ?? $"Свойство \"{propertyName}\" не может иметь значение \"{newValue}\"";
            else
                _errors[propertyName] = null;
            return true;
        }

        // Реализация IDataErrorInfo
        // словарь ошибок
        // хранит пары "Свойство" - "Ошибка"
        private readonly Dictionary<string, string> _errors = new Dictionary<string, string>();

        public string this[string property] => _errors.ContainsKey(property) ? _errors[property] : null;

        [JsonIgnore]
        // если все записи в словаре ошибок равны null - объект валиден
        public bool IsValid => _errors.Values.All(x => x == null);

        [JsonIgnore]
        public string Error => null;
    }
}
