using System.Diagnostics.CodeAnalysis;

namespace Specs_Generated;

[Id<Behavior, int>]
public readonly partial struct EvenOnlyId
{
    internal sealed class Behavior : Int32IdBehavior
    {
        public override bool TryTransform(int value, [NotNullWhen(true)] out int transformed)
            => base.TryTransform(value, out transformed) && value % 2 == 0;
    }
}
