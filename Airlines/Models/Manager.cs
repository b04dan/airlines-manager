using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Airlines.Utils;
using static Airlines.Utils.Generators;

namespace Airlines.Models
{
    public class Manager : ValidatableModelBase, ICloneable
    {
        #region Поля
        private string _surname; // фамилия
        private string _name; // имя
        private string _patronymic; // отчество
        private string _photoFileName; // название файла с фото
        private string _phoneNumber; // номер телефона
        private string _email; // электронная почта
        #endregion

        #region Свойства
        public string Surname
        {
            get => _surname;
            set => SetProperty(ref _surname, value, string.IsNullOrWhiteSpace,
                "Фамилия не может быть пустым");
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value, string.IsNullOrWhiteSpace,
                "Имя не может быть пустым");
        }

        public string Patronymic
        {
            get => _patronymic;
            set => SetProperty(ref _patronymic, value, string.IsNullOrWhiteSpace,
                "Отчество не может быть пустым");
        }

        public string Email
        {
            get => _email;
            // https://docs.microsoft.com/en-us/dotnet/standard/base-types/how-to-verify-that-strings-are-in-valid-email-format
            set => SetProperty(ref _email, value, email => !Regex.IsMatch(email,
                @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)),
                "E-mail адрес невалиден");
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set => SetProperty(ref _phoneNumber, value, string.IsNullOrWhiteSpace,
                "Номер телефона не может быть пустым");
        }

        public string PhotoFileName
        {
            get => _photoFileName;
            set => SetProperty(ref _photoFileName, value);
        }
        #endregion

        #region Генерация персон
        public static Manager Generate()
        {
            return new Manager()
            {
                Surname = GenerateSurname(),
                Name =  GenerateName(),
                Patronymic = GeneratePatronymic(),
                PhotoFileName = $"{GetRand(1, 15)}.png",
                PhoneNumber = GeneratePhoneNumber(),
                Email = Emails[GetRand(Emails.Length-1)]
            };
        }

        // массив эмейлов для генерации
        private static readonly string[] Emails =
        {
            "fiyipo2661@gmail.com", "sofyec@gmail.com", "ominousis@gmail.com", "urimp@gmail.com", "orerenge@gmail.com",
            "horounton@gmail.com", "owila@gmail.com", "astaler@gmail.com", "kufive@gmail.com", "ontoro@gmail.com",
            "rathenon@gmail.com", "lillilime@gmail.com", "aywemay@gmail.com", "gusonga@gmail.com", "feyli@gmail.com",
            "agaisugus@gmail.com", "irindort@gmail.com", "gofte@gmail.com", "acimmoflu@gmail.com", "rinesho@gmail.com"
        };

        #endregion

        public object Clone()
        {
            // в данном случае можно использовать метод поверхностного копирования,
            // т.к. используются только примитивные типы
            return MemberwiseClone();
        }
    }
}
