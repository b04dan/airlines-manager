using System;
using System.Linq;
using System.Text;

namespace Airlines.Utils
{
    // методы для генерации случайных значений
    static class Generators
    {
        public static readonly Random Random = new Random(); // объект для генерации случайных чисел

        public static int GetRand(int lo, int hi) => Random.Next(lo, hi + 1); // генерация int
        public static int GetRand(int hi) => Random.Next(hi + 1); // генерация int

        public static double GetRand(double lo, double hi)
        {
            double result = lo + (hi - lo) * Random.NextDouble();
            return Math.Abs(result) < 0.7 ? 0 : result; // для генерации нулей
        } // генерация double

        // сгенерировать фамилию
        public static string GenerateSurname()
        {
            string[] surnames = {
                "Андрусейко", "Гущин", "Корнейчук", "Князев", "Кононов",
                "Кабанов", "Лапин", "Кондратьев", "Кудрявцев", "Пахомов",
                "Палий", "Щукин", "Овчинников", "Мамонтов", "Кузьмин",
                "Смирнов", "Иванов", "Кузнецов", "Соколов", "Попов", "Лебедев",
                "Козлов", "Новиков", "Морозов", "Петров", "Волков", "Соловьёв",
                "Васильев", "Зайцев", "Павлов", "Семёнов", "Голубев", "Виноградов",
                "Богданов", "Воробьёв", "Фёдоров", "Михайлов", "Беляев", "Тарасов", "Белов"
            };

            return surnames[GetRand(0, surnames.Length - 1)];
        }

        // сгенерировать имя
        public static string GenerateName()
        {
            // имена, фамилии и отчества для генератора ФИО
            string[] names = {
                "Жерар", "Никодим", "Казбек", "Осип", "Назар",
                "Спартак", "Донат", "Харитон", "Лука", "Гавриил",
                "Елисей", "Жигер", "Милан", "Геннадий", "Яромир",
                "Алан", "Александр", "Алексей", "Альберт", "Анатолий",
                "Андрей", "Антон", "Арсен", "Арсений", "Артем", "Артемий", "Артур", "Богдан", "Борис", "Вадим",
                "Валентин", "Валерий", "Василий", "Виктор", "Виталий", "Владимир", "Владислав", "Всеволод", "Вячеслав",
                "Геннадий", "Георгий", "Герман", "Глеб", "Гордей", "Григорий", "Давид", "Дамир", "Даниил", "Демид",
                "Демьян", "Денис", "Дмитрий", "Евгений", "Егор", "Елисей", "Захар", "Иван", "Игнат", "Игорь", "Илья",
                "Ильяс", "Камиль", "Карим", "Кирилл", "Клим", "Константин", "Лев", "Леонид", "Макар", "Максим", "Марат",
                "Марк", "Марсель", "Матвей", "Мирон", "Мирослав", "Михаил", "Назар", "Никита", "Николай", "Олег",
                "Павел", "Петр", "Платон", "Прохор", "Рамиль", "Ратмир", "Ринат", "Роберт", "Родион", "Роман",
                "Ростислав", "Руслан", "Рустам", "Савва", "Савелий", "Святослав", "Семен", "Сергей", "Станислав",
                "Степан", "Тамерлан", "Тимофей", "Тимур", "Тихон", "Федор", "Филипп", "Шамиль", "Эдуард", "Эльдар",
                "Эмиль", "Эрик", "Юрий", "Ян", "Ярослав"
            };

            return names[GetRand(0, names.Length - 1)];
        }

        // сгенерировать город
        public static string GenerateCity()
        {
            // города
            string[] cities = {
                "Донецк", "Москва", "Киев", "Сочи", "Харьков",
                "Львов", "Лондон", "Анапа", "Томск", "Липецк",
                "Глазов", "Владимир", "Курган", "Шахты", "Котлас"
            };

            return cities[GetRand(0, cities.Length - 1)];
        }

        // сгенерировать отчество
        public static string GeneratePatronymic()
        {
            string[] patronymics = {
                "Владимирович", "Фёдорович", "Максимович", "Богданович", "Васильевич",
                "Борисович", "Максимович", "Станиславович", "Романович", "Петрович",
                "Алексеевич", "Григорьевич", "Данилович", "Брониславович", "Юхимович"
            };

            return patronymics[GetRand(0, patronymics.Length - 1)];
        }

        // сгенерировать ФИО
        public static string GenerateSNP() => $"{GenerateSurname()} {GenerateName()} {GeneratePatronymic()}";
        public static string GenerateSurnameNP() => $"{GenerateSurname()} {GenerateName().First()}. {GeneratePatronymic().First()}.";

        // сгенерировать согласную букву
        public static char GenerateConsonant()
        {
            // согласные
            char[] consonants = {
                'б', 'в', 'г', 'д', 'ж',
                'з', 'й', 'к', 'л', 'м',
                'н', 'п', 'р', 'с', 'т',
                'ф', 'х', 'ц', 'ч', 'ш', 'щ'
            };

            return consonants[GetRand(0, consonants.Length - 1)];
        }

        // генерация bool
        public static bool GenerateBool() => GetRand(0, 1) == 0;


        // сгенерировать номер машины
        public static string GenerateNumber()
        {
            // буквы для номеров
            // используются те, которые присутствуют и в кириллице, и в латинице
            char[] numberLetters = "АВЕКМНОРСТУХ".ToCharArray();

            StringBuilder builder = new StringBuilder(8);
            for (int i = 0; i < 5; i++)
                builder.Append(
                    i == 2
                    ? $"{GetRand(1000, 9999)}"
                    : $"{numberLetters[GetRand(numberLetters.Length - 1)]}");

            return builder.ToString();
        }

        public static string GeneratePassportID()
        {
            // используются те, которые присутствуют и в кириллице, и в латинице
            char[] letters = "АВЕКМНОРСТУХ".ToCharArray();

            StringBuilder builder = new StringBuilder(8);
            builder.Append(letters[GetRand(letters.Length - 1)]);
            builder.Append(letters[GetRand(letters.Length - 1)]);
            builder.Append('-');
            builder.Append(GetRand(100000, 999999));

            return builder.ToString();
        }

        // сгенерировать номер телефона
        public static string GeneratePhoneNumber(string countryCode, string mobileOperator) => $"+{countryCode}{mobileOperator}{GetRand(1000000, 9999999)}";
        public static string GeneratePhoneNumber() => $"+{GetRand(11, 99)}{GetRand(100, 999)}{GetRand(1000000, 9999999)}";

    }
}
