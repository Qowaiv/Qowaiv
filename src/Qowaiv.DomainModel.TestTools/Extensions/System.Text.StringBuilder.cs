using System.Reflection;

namespace System.Text;

internal static class QowaivDomainModelStringBuilderExtensions
{
    [Impure]
    public static bool AppendEvents(this StringBuilder sb, long index, object exp, object act)
        => sb.AppendDifferentTypes(index, exp, act)
        || sb.AppendDifferentEvents(index, exp, act)
        || sb.AppendIdenticalEvents(index, act);

    [Impure]
    public static bool AppendExtraEvents(this StringBuilder sb, IEnumerable<object> events, long offset, int skip, string prefix)
    {
        var index = offset + skip;

        var extra = false;

        foreach (var @event in events.Skip(skip))
        {
            sb.AppendLine($"[{index}] {prefix} {@event.GetType().Name}");
            index++;
            extra = true;
        }

        return extra;
    }

    [Impure]
    private static bool AppendDifferentTypes(this StringBuilder sb, long index, object exp, object act)
    {
        var actType = act.GetType();
        var expType = exp.GetType();

        return actType != expType &&
            sb.AppendExpectedActual(index, expType, actType);
    }

    [Impure]
    private static bool AppendDifferentEvents(this StringBuilder sb, long index, object exp, object act)
    {
        var failure = false;

        var sbExp = new StringBuilder().Append("{ ");
        var sbAct = new StringBuilder().Append("{ ");

        var properties = exp.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var prop in properties)
        {
            var e = prop.GetValue(exp, []);
            var a = prop.GetValue(act, []);

            if (prop.PropertyType.IsArray)
            {
                var arrayE = (Array)e!;
                var arrayA = (Array)a!;

                var e_ = new object[arrayE.Length];
                var a_ = new object[arrayA.Length];

                Array.Copy(arrayE, e_, e_.Length);
                Array.Copy(arrayA, a_, a_.Length);

                if (!Enumerable.SequenceEqual(e_, a_))
                {
                    failure = true;

                    sbExp.Append($"{prop.Name}: [ {string.Join(", ", e_)} ], ");
                    sbAct.Append($"{prop.Name}: [ {string.Join(", ", a_)} ], ");
                }
            }
            else
            {
                if (!Equals(e, a))
                {
                    failure = true;
                    sbExp.Append($"{prop.Name}: {e}, ");
                    sbAct.Append($"{prop.Name}: {a}, ");
                }
            }
        }

        sbExp.Remove(sbExp.Length - 2, 2);
        sbAct.Remove(sbAct.Length - 2, 2);

        sbExp.Append(" }");
        sbAct.Append(" }");

        return failure && sb.AppendExpectedActual(index, sbExp, sbAct);
    }

    [Impure]
    private static bool AppendIdenticalEvents(this StringBuilder sb, long index, object @event)
    {
        sb.AppendLine($"[{index}] {@event.GetType().Name}");
        return false;
    }

    [Impure]
    private static bool AppendExpectedActual(this StringBuilder sb, long index, object expected, object actual)
    {
        var prefix = $"[{index}] ";
        var empty = new string(' ', prefix.Length);

        sb.Append(prefix + "Expected: ");
        sb.AppendLine(expected.ToString());
        sb.AppendLine($"{empty}Actual:   {actual}");

        return true;
    }
}
