using System.Reflection;
using System.Reflection.Emit;

namespace Qowaiv.Validation.DataAnnotations.Reflection;

internal static class PropertyInfoExtensions
{
    extension(PropertyInfo prop)
    {
        public bool IsSimpleFieldAccessor
        {
            get
            {
                // Fast Path: Check if it's a standard compiler-generated auto-property name
                // (This catches 90% of cases instantly without reading IL)
                if (prop.DeclaringType?.GetField($"<{prop.Name}>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance) is not null)
                {
                    return true;
                }

                // Get the read method (getter)
                if (prop.GetMethod is not { } getMethod) return false;

                // An ideal, optimized instance property getter contains 5 or 6 bytes:
                // ldarg.0 (1 byte) + ldfld (1 byte) + Field Token (4 bytes) + ret (1 byte) = 7 bytes maximum.
                // If it contains local variables or a massive instruction payload, it's not a pure accessor.
                if (getMethod.GetMethodBody()?.GetILAsByteArray() is not { Length: < 8 } ilBytes) return false;

                // Static properties don't pass an instance argument (ldarg.0), so their IL pattern differs.
                bool isStatic = getMethod.IsStatic;

                int index = 0;

                // 1. Check for instance argument if it's an instance property
                if (!isStatic)
                {
                    if (ilBytes[index] == OpCodes.Ldarg_0.Value)
                    {
                        index++;
                    }
                    else return false;
                }

                // 2. Look for the load-field instruction
                // OpCodes.Ldfld is 0x7B (instance field), OpCodes.Ldsfld is 0x7E (static field)
                byte expectedOpCode = isStatic ? (byte)0x7E : (byte)0x7B;

                if (index >= ilBytes.Length || ilBytes[index] != expectedOpCode)
                {
                    return false;
                }
                index++;

                // 3. Skip the 4-byte Metadata Token of the field
                index += 4;

                // 4. Ensure the last instruction is a return statement
                if (index >= ilBytes.Length || ilBytes[index] != OpCodes.Ret.Value)
                {
                    return false;
                }
                return true;
            }
        }
    }
}
