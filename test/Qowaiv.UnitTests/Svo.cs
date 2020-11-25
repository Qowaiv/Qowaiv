using System;

namespace Qowaiv.UnitTests
{
    public static class Svo
    {
        public static readonly PostalCode PostalCode = PostalCode.Parse("H0H0H0");

        public static readonly Guid Guid = Guid.Parse("8a1a8c42-d2ff-e254-e26e-b6abcbf19420");
        public static readonly Uuid Uuid = Uuid.Parse("Qowaiv_SVOLibrary_GUIA");

        public static readonly Year Year = 1979;
        public static readonly YesNo YesNo = YesNo.Yes;
    }
}
