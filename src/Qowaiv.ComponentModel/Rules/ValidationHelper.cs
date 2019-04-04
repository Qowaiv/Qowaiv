using System;

namespace Qowaiv.ComponentModel.Rules
{
    public static class ValidationHelper
    {
        public static bool NotDefaultOrUnknown(this object value)
        {
            return value.NotDefault() && value != Unknown.Value(value.GetType());
        }

        public static bool NotDefault(this object value)
        {
            if (value is null)
            {
                return false;
            }
            if (value is string str)
            {
                return !string.IsNullOrEmpty(str);
            }
            var tp = value.GetType();
            if (tp.IsValueType)
            {
                return !Activator.CreateInstance(tp).Equals(value);
            }
            return true;
        }
    }
}
