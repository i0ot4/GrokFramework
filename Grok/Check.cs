using Grok.System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Grok
{
    [DebuggerStepThrough]
    public static class Check
    {
        [JetBrains.Annotations.ContractAnnotation("value:null => halt")]
        public static T NotNull<T>(
            [NotNull] T? value,
            [JetBrains.Annotations.InvokerParameterName][JetBrains.Annotations.NotNull] string parameterName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }

            return value;
        }

        [JetBrains.Annotations.ContractAnnotation("value:null => halt")]
        public static T NotNull<T>(
            [NotNull] T? value,
            [JetBrains.Annotations.InvokerParameterName][JetBrains.Annotations.NotNull] string parameterName,
            string message)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName, message);
            }

            return value;
        }

        [JetBrains.Annotations.ContractAnnotation("value:null => halt")]
        public static string NotNull(
            [NotNull] string? value,
            [JetBrains.Annotations.InvokerParameterName][JetBrains.Annotations.NotNull] string parameterName,
            int maxLength = int.MaxValue,
            int minLength = 0)
        {
            if (value == null)
            {
                throw new ArgumentException($"{parameterName} can not be null!", parameterName);
            }

            if (value.Length > maxLength)
            {
                throw new ArgumentException($"{parameterName} length must be equal to or lower than {maxLength}!", parameterName);
            }

            if (minLength > 0 && value.Length < minLength)
            {
                throw new ArgumentException($"{parameterName} length must be equal to or bigger than {minLength}!", parameterName);
            }

            return value;
        }

        [JetBrains.Annotations.ContractAnnotation("value:null => halt")]
        public static string NotNullOrWhiteSpace(
            [NotNull] string? value,
            [JetBrains.Annotations.InvokerParameterName][JetBrains.Annotations.NotNull] string parameterName,
            int maxLength = int.MaxValue,
            int minLength = 0)
        {
            if (value.IsNullOrWhiteSpace())
            {
                throw new ArgumentException($"{parameterName} can not be null, empty or white space!", parameterName);
            }

            if (value!.Length > maxLength)
            {
                throw new ArgumentException($"{parameterName} length must be equal to or lower than {maxLength}!", parameterName);
            }

            if (minLength > 0 && value!.Length < minLength)
            {
                throw new ArgumentException($"{parameterName} length must be equal to or bigger than {minLength}!", parameterName);
            }

            return value;
        }

        [JetBrains.Annotations.ContractAnnotation("value:null => halt")]
        public static string NotNullOrEmpty(
            [NotNull] string? value,
            [JetBrains.Annotations.InvokerParameterName][JetBrains.Annotations.NotNull] string parameterName,
            int maxLength = int.MaxValue,
            int minLength = 0)
        {
            if (value.IsNullOrEmpty())
            {
                throw new ArgumentException($"{parameterName} can not be null or empty!", parameterName);
            }

            if (value!.Length > maxLength)
            {
                throw new ArgumentException($"{parameterName} length must be equal to or lower than {maxLength}!", parameterName);
            }

            if (minLength > 0 && value!.Length < minLength)
            {
                throw new ArgumentException($"{parameterName} length must be equal to or bigger than {minLength}!", parameterName);
            }

            return value;
        }

        [JetBrains.Annotations.ContractAnnotation("value:null => halt")]
        public static ICollection<T> NotNullOrEmpty<T>(
            [NotNull] ICollection<T>? value,
            [JetBrains.Annotations.InvokerParameterName][JetBrains.Annotations.NotNull] string parameterName)
        {
            if (value == null || value.Count <= 0)
            {
                throw new ArgumentException(parameterName + " can not be null or empty!", parameterName);
            }

            return value;
        }

        [JetBrains.Annotations.ContractAnnotation("type:null => halt")]
        public static Type AssignableTo<TBaseType>(
            Type type,
            [JetBrains.Annotations.InvokerParameterName][JetBrains.Annotations.NotNull] string parameterName)
        {
            NotNull(type, parameterName);

            if (!type.IsAssignableTo<TBaseType>())
            {
                throw new ArgumentException($"{parameterName} (type of {type.AssemblyQualifiedName}) should be assignable to the {typeof(TBaseType).GetFullNameWithAssemblyName()}!");
            }

            return type;
        }

        public static string? Length(
            string? value,
            [JetBrains.Annotations.InvokerParameterName][JetBrains.Annotations.NotNull] string parameterName,
            int maxLength,
            int minLength = 0)
        {
            if (minLength > 0)
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException(parameterName + " can not be null or empty!", parameterName);
                }

                if (value!.Length < minLength)
                {
                    throw new ArgumentException($"{parameterName} length must be equal to or bigger than {minLength}!", parameterName);
                }
            }

            if (value != null && value.Length > maxLength)
            {
                throw new ArgumentException($"{parameterName} length must be equal to or lower than {maxLength}!", parameterName);
            }

            return value;
        }

        public static Int16 Positive(
            Int16 value,
            [JetBrains.Annotations.InvokerParameterName][JetBrains.Annotations.NotNull] string parameterName)
        {
            if (value == 0)
            {
                throw new ArgumentException($"{parameterName} is equal to zero");
            }
            else if (value < 0)
            {
                throw new ArgumentException($"{parameterName} is less than zero");
            }
            return value;
        }

        public static Int32 Positive(
            Int32 value,
            [JetBrains.Annotations.InvokerParameterName][JetBrains.Annotations.NotNull] string parameterName)
        {
            if (value == 0)
            {
                throw new ArgumentException($"{parameterName} is equal to zero");
            }
            else if (value < 0)
            {
                throw new ArgumentException($"{parameterName} is less than zero");
            }
            return value;
        }

        public static Int64 Positive(
            Int64 value,
            [JetBrains.Annotations.InvokerParameterName][JetBrains.Annotations.NotNull] string parameterName)
        {
            if (value == 0)
            {
                throw new ArgumentException($"{parameterName} is equal to zero");
            }
            else if (value < 0)
            {
                throw new ArgumentException($"{parameterName} is less than zero");
            }
            return value;
        }

        public static float Positive(
            float value,
            [JetBrains.Annotations.InvokerParameterName][JetBrains.Annotations.NotNull] string parameterName)
        {
            if (value == 0)
            {
                throw new ArgumentException($"{parameterName} is equal to zero");
            }
            else if (value < 0)
            {
                throw new ArgumentException($"{parameterName} is less than zero");
            }
            return value;
        }

        public static double Positive(
            double value,
            [JetBrains.Annotations.InvokerParameterName][JetBrains.Annotations.NotNull] string parameterName)
        {
            if (value == 0)
            {
                throw new ArgumentException($"{parameterName} is equal to zero");
            }
            else if (value < 0)
            {
                throw new ArgumentException($"{parameterName} is less than zero");
            }
            return value;
        }

        public static decimal Positive(
            decimal value,
            [JetBrains.Annotations.InvokerParameterName][JetBrains.Annotations.NotNull] string parameterName)
        {
            if (value == 0)
            {
                throw new ArgumentException($"{parameterName} is equal to zero");
            }
            else if (value < 0)
            {
                throw new ArgumentException($"{parameterName} is less than zero");
            }
            return value;
        }

        public static Int16 Range(
            Int16 value,
            [JetBrains.Annotations.InvokerParameterName][JetBrains.Annotations.NotNull] string parameterName,
            Int16 minimumValue,
            Int16 maximumValue = Int16.MaxValue)
        {
            if (value < minimumValue || value > maximumValue)
            {
                throw new ArgumentException($"{parameterName} is out of range min: {minimumValue} - max: {maximumValue}");
            }

            return value;
        }
        public static Int32 Range(
            Int32 value,
            [JetBrains.Annotations.InvokerParameterName][JetBrains.Annotations.NotNull] string parameterName,
            Int32 minimumValue,
            Int32 maximumValue = Int32.MaxValue)
        {
            if (value < minimumValue || value > maximumValue)
            {
                throw new ArgumentException($"{parameterName} is out of range min: {minimumValue} - max: {maximumValue}");
            }

            return value;
        }

        public static Int64 Range(
            Int64 value,
            [JetBrains.Annotations.InvokerParameterName][JetBrains.Annotations.NotNull] string parameterName,
            Int64 minimumValue,
            Int64 maximumValue = Int64.MaxValue)
        {
            if (value < minimumValue || value > maximumValue)
            {
                throw new ArgumentException($"{parameterName} is out of range min: {minimumValue} - max: {maximumValue}");
            }

            return value;
        }


        public static float Range(
            float value,
            [JetBrains.Annotations.InvokerParameterName][JetBrains.Annotations.NotNull] string parameterName,
            float minimumValue,
            float maximumValue = float.MaxValue)
        {
            if (value < minimumValue || value > maximumValue)
            {
                throw new ArgumentException($"{parameterName} is out of range min: {minimumValue} - max: {maximumValue}");
            }
            return value;
        }


        public static double Range(
            double value,
            [JetBrains.Annotations.InvokerParameterName][JetBrains.Annotations.NotNull] string parameterName,
            double minimumValue,
            double maximumValue = double.MaxValue)
        {
            if (value < minimumValue || value > maximumValue)
            {
                throw new ArgumentException($"{parameterName} is out of range min: {minimumValue} - max: {maximumValue}");
            }

            return value;
        }


        public static decimal Range(
            decimal value,
            [JetBrains.Annotations.InvokerParameterName][JetBrains.Annotations.NotNull] string parameterName,
            decimal minimumValue,
            decimal maximumValue = decimal.MaxValue)
        {
            if (value < minimumValue || value > maximumValue)
            {
                throw new ArgumentException($"{parameterName} is out of range min: {minimumValue} - max: {maximumValue}");
            }

            return value;
        }

        public static T NotDefaultOrNull<T>(
            [NotNull] T? value,
            [JetBrains.Annotations.InvokerParameterName][JetBrains.Annotations.NotNull] string parameterName)
            where T : struct
        {
            if (value == null)
            {
                throw new ArgumentException($"{parameterName} is null!", parameterName);
            }

            if (value.Value.Equals(default(T)))
            {
                throw new ArgumentException($"{parameterName} has a default value!", parameterName);
            }

            return value.Value;
        }
    }
}