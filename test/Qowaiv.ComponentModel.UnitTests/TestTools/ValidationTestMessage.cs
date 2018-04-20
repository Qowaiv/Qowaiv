using Qowaiv.ComponentModel.Messages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Qowaiv.ComponentModel.Tests.TestTools
{
    public sealed class ValidationTestMessage: IEquatable<ValidationTestMessage>
    {
        public ValidationTestMessage(ValidationSeverity severity, string message, params string[] memberNames)
        {
            Severity = severity;
            Message = message;
            MemberNames = new ReadOnlyCollection<string>(memberNames.ToList());
        }

        public string Message { get; }
        public ValidationSeverity Severity { get; }
        public ReadOnlyCollection<string> MemberNames { get; }

        public override bool Equals(object obj) => Equals(obj as ValidationTestMessage);
        public bool Equals(ValidationTestMessage other)=> other != null && ToString() == other.ToString();
        public override int GetHashCode() => ToString().GetHashCode();

        public override string ToString() => $"[{Severity}] {Message}, Members: {string.Join(", ", MemberNames)}";

        public static ValidationTestMessage Info(string message, params string[] memberNames) => new ValidationTestMessage(ValidationSeverity.Info, message, memberNames);
        public static ValidationTestMessage Warning(string message, params string[] memberNames) => new ValidationTestMessage(ValidationSeverity.Warning, message, memberNames);
        public static ValidationTestMessage Error(string message, params string[] memberNames) => new ValidationTestMessage(ValidationSeverity.Error, message, memberNames);
    }

    public static class ValidationTestMessageExtensions
    {
        public static ValidationTestMessage[] ForAssertion(this IEnumerable<ValidationResult> results)
        {
            var list = new List<ValidationTestMessage>();
            foreach(var result in results)
            {
                var severity = result.GetSeverity();
                if(severity != ValidationSeverity.None)
                {
                    list.Add(new ValidationTestMessage(severity, result.ErrorMessage, result.MemberNames.ToArray()));
                }
            }
            return list.ToArray();
        }
    }
}
