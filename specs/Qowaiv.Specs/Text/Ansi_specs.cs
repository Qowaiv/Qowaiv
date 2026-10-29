namespace Text.Ansi_specs;

public class SpanFormattable
{
    public class Formats
    {
        [Test]
        public void Default()
        {
            Span<char> buffer = stackalloc char[256];
            Ansi ansi = default;

            ansi.TryFormat(buffer, out var written, default, null).Should().BeTrue();
            new string(buffer[..written].ToArray()).Should().Be("\x1b[0m");
        }

        [Test]
        public void Single_code()
        {
            Span<char> buffer = stackalloc char[256];
            Ansi ansi = Ansi.Bright.BG.Cyan;

            ansi.TryFormat(buffer, out var written, default, null).Should().BeTrue();
            new string(buffer[..written].ToArray()).Should().Be("\x1b[106m");
        }

        [Test]
        public void Multiple_codes()
        {
            Span<char> buffer = stackalloc char[256];
            Ansi ansi = Ansi.Rgb(1, 12, 123);

            ansi.TryFormat(buffer, out var written, default, null).Should().BeTrue();
            new string(buffer[..written].ToArray()).Should().Be("\x1b[38;2;1;12;123m");
        }
    }

    [Test]
    public void Does_not_format([Range(0, 15)]int size)
    {
        Span<char> buffer = stackalloc char[size];
        Ansi ansi = Ansi.Rgb(34, 51, 68);
        ansi.TryFormat(buffer, out var written, default, null).Should().BeFalse();
        written.Should().Be(0);
        buffer.Length.Should().Be(size);
    }
}

public class RGB
{
    public class Foreground
    {
        [Test]
        public void Grayscale()
            => Ansi.Gray(42).Should().Be("\x1b[38;5;42m");

        [Test]
        public void Red_Green_Blue()
           => Ansi.Rgb(42, 17, 225).Should().Be("\x1b[38;2;42;17;225m");

        [Test]
        public void _24_bit()
           => Ansi.Rgb(0xCC_03_25).Should().Be("\x1b[38;2;204;3;37m");
    }

    public class Background
    {
        [Test]
        public void Grayscale()
            => Ansi.BG.Gray(42).Should().Be("\x1b[48;5;42m");

        [Test]
        public void Red_Green_Blue()
           => Ansi.BG.Rgb(42, 17, 225).Should().Be("\x1b[48;2;42;17;225m");

        [Test]
        public void _24_bit()
           => Ansi.BG.Rgb(0xCC_03_25).Should().Be("\x1b[48;2;204;3;37m");
    }
}

public class Merges
{
    public class Replaces
    {
        [Test]
        public void Dim_by_bold()
            => (Ansi.Dim | Ansi.Bold).Should().Be("\x1b[1m");

        [Test]
        public void Not_bold_by_bold()
            => (Ansi.Not.Bold | Ansi.Bold).Should().Be("\x1b[1m");

        [Test]
        public void Not_Iitalic_by_italic()
           => (Ansi.Not.Italic | Ansi.Italic).Should().Be("\x1b[3m");

        [Test]
        public void Bold_by_dim()
           => (Ansi.Bold | Ansi.Dim).Should().Be("\x1b[2m");

        [Test]
        public void Not_underline_by_underline()
           => (Ansi.Not.Underline | Ansi.Underline).Should().Be("\x1b[4m");

        [Test]
        public void Rapid_blink_by_slow_blink()
           => (Ansi.RapidBlink | Ansi.SlowBlink).Should().Be("\x1b[5m");

        [Test]
        public void Not_blink_by_slow_blink()
           => (Ansi.Not.Blink | Ansi.SlowBlink).Should().Be("\x1b[5m");

        [Test]
        public void Slow_blink_by_rapid_blink()
           => (Ansi.SlowBlink | Ansi.RapidBlink).Should().Be("\x1b[6m");

        [Test]
        public void Not_blink_by_rapid_blink()
           => (Ansi.Not.Blink | Ansi.RapidBlink).Should().Be("\x1b[6m");

        [Test]
        public void Not_invert_by_invert()
            => (Ansi.Not.Invert | Ansi.Invert).Should().Be("\x1b[7m");

        [Test]
        public void Not_hide_by_hide()
            => (Ansi.Not.Hide | Ansi.Hide).Should().Be("\x1b[8m");

        [Test]
        public void Not_strike_by_strike()
            => (Ansi.Not.Strike | Ansi.Strike).Should().Be("\x1b[9m");

        [Test]
        public void Bold_by_not_bold()
            => (Ansi.Bold | Ansi.Not.Bold).Should().Be("\x1b[21m");

        [Test]
        public void Bold_by_not_bold_or_dim()
            => (Ansi.Bold | Ansi.Not.BoldOrDim).Should().Be("\x1b[22m");
        
        [Test]
        public void Dim_by_not_bold_or_dim()
            => (Ansi.Dim | Ansi.Not.BoldOrDim).Should().Be("\x1b[22m");

        [Test]
        public void Italic_by_not_italic()
            => (Ansi.Italic | Ansi.Not.Italic).Should().Be("\x1b[23m");

        [Test]
        public void Underline_by_not_underline()
            => (Ansi.Underline | Ansi.Not.Underline).Should().Be("\x1b[24m");

        [Test]
        public void Slow_blink_by_not_blink()
            => (Ansi.SlowBlink | Ansi.Not.Blink).Should().Be("\x1b[25m");

        [Test]
        public void Rapid_blink_by_not_blink()
            => (Ansi.RapidBlink | Ansi.Not.Blink).Should().Be("\x1b[25m");

        [Test]
        public void Invert_by_not_invert()
            => (Ansi.Invert | Ansi.Not.Invert).Should().Be("\x1b[27m");

        [Test]
        public void Hide_by_not_hide()
            => (Ansi.Hide | Ansi.Not.Hide).Should().Be("\x1b[28m");

        [Test]
        public void Strike_by_not_strike()
            => (Ansi.Strike | Ansi.Not.Strike).Should().Be("\x1b[29m");
    }

    public class Styles
    {
        [Test]
        public void That_do_not_contadict()
            => (Ansi.Bold | Ansi.Italic).Should().Be("\x1b[1;3m");
    }

    [Test]
    public void Reset_skips_previous_formatting()
        => ((Ansi.Green | Ansi.BG.Cyan) | Ansi.Reset).Should().Be("\x1b[0m");

    public class Foreground_with
    {
        [Test]
        public void Foreground() =>
           (Ansi.Red | Ansi.Blue).Should().Be("\x1b[34m");

        [Test]
        public void Forground_RGB() =>
           (Ansi.Rgb(20, 17, 129) | Ansi.Rgb(200, 12, 16)).Should().Be("\x1b[38;2;200;12;16m");

        [Test]
        public void Background() =>
            (Ansi.Red | Ansi.BG.Blue).Should().Be("\x1b[31;44m");

        [Test]
        public void Background_RGB() =>
           (Ansi.Rgb(20, 17, 129) | Ansi.BG.Rgb(200, 12, 16)).Should().Be("\x1b[38;2;20;17;129;48;2;200;12;16m");
    }

    public class Background_with
    {
        [Test]
        public void Foreground() =>
           (Ansi.BG.Red | Ansi.Blue).Should().Be("\x1b[34;41m");

        [Test]
        public void Forground_RGB() =>
           (Ansi.BG.Rgb(20, 17, 129) | Ansi.Rgb(200, 12, 16)).Should().Be("\x1b[38;2;200;12;16;48;2;20;17;129m");

        [Test]
        public void Background() =>
            (Ansi.BG.Red | Ansi.BG.Blue).Should().Be("\x1b[44m");

        [Test]
        public void Background_RGB() =>
           (Ansi.BG.Rgb(20, 17, 129) | Ansi.BG.Rgb(200, 12, 16)).Should().Be("\x1b[48;2;200;12;16m");
    }
}
