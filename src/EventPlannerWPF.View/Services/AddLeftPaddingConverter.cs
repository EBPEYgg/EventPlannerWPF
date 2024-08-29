using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace EventPlannerWPF.View.Services
{
    /// <summary>
    /// Класс, описывающий конвертер значения для свойства Visibility.
    /// </summary>
    public class AddLeftPaddingConverter : IValueConverter
    {
        /// <summary>
        /// Метод, представляющий собой конвертер, который добавляет 
        /// указанное количество пробелов к отступу Left свойства Thickness.
        /// </summary>
        /// <param name="value">Значение типа <see cref="Thickness"/>, подлежащее преобразованию.</param>
        /// <param name="targetType">Тип, в который будет преобразовано значение.</param>
        /// <param name="parameter">Значение, которое будет добавлено к отступу Left.</param>
        /// <param name="culture">Язык, который будет использоваться в конвертере.</param>
        /// <returns>Обновленное значение типа Thickness с измененным отступом Left,
        /// или исходное значение, если преобразование не удалось.</returns>
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if (value is not Thickness padding)
            {
                return value;
            }
            if (!double.TryParse(parameter.ToString(), out double amount))
            {
                return value;
            }

            padding.Left += amount;
            return padding;
        }

        /// <summary>
        /// ConverterBack не поддерживается.
        /// </summary>
        /// <param name="value">Значение для обратного преобразования.</param>
        /// <param name="targetType">Тип, к которому значение будет преобразовано обратно.</param>
        /// <param name="parameter">Необязательный параметр, используемый при преобразовании.</param>
        /// <param name="culture">Язык, который будет использоваться в конвертере.</param>
        /// <returns>Вызывает исключение NotSupportedException.</returns>
        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}