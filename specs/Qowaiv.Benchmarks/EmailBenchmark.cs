﻿using Qowaiv;

namespace Benchmarks;

public class EmailBenchmark
{
    private static readonly string[] Emails = [
        "info@qowaiv.org",
        "info@[192.0.2.1]",
        "info@[IPv6:2001:0db8:0000:0000:0000:ff00:0042:8329]",
        "Mixed    <local@あいうえお.com>",
        "\"very.(),:;<>[]\\\".VERY.\\\"very@\\\\ \\\"very\\\".unusual\"@qowaiv.org",
        "mailto:info(with comment)@qowaiv.org",
        "Terminated@qowaiv.org",
        "principles@qowaiv.org",
        "sentiments@qowaiv.org",
        "pianoforte@qowaiv.org",
        "if@qowaiv.org",
        "projection@qowaiv.org",
        "impossible.234@qowaiv.org",
        "Horses@qowaiv.org",
        "pulled@qowaiv.org",
        "nature@qowaiv.org",
        "favour@qowaiv.org",
        "number@qowaiv.org",
        "yet@qowaiv.org",
        "HIGHLY@qowaiv.org",
        "his@qowaiv.org",
        "has@qowaiv.org",
        "old.234@qowaiv.org",
        "Contrasted@qowaiv.org",
        "literature@qowaiv.org",
        "excellence@qowaiv.org",
        "he@qowaiv.org",
        "admiration@qowaiv.org",
        "impression@qowaiv.org",
        "insipidity@qowaiv.org",
        "so.234@qowaiv.org",
        "Scale@qowaiv.org",
        "ought@qowaiv.org",
        "who@qowaiv.org",
        "terms@qowaiv.org",
        "after@qowaiv.org",
        "own@qowaiv.org",
        "quick@qowaiv.org",
        "since.234@qowaiv.org",
        "Servants@qowaiv.org",
        "margaret@qowaiv.org",
        "husbands@qowaiv.org",
        "to@qowaiv.org",
        "screened@qowaiv.org",
        "in@qowaiv.org",
        "throwing.234@qowaiv.org",
        "Imprudence@qowaiv.org",
        "oh@qowaiv.org",
        "an@qowaiv.org",
        "collecting@qowaiv.org",
        "partiality.234@qowaiv.org",
        "Admiration@qowaiv.org",
        "gay@qowaiv.org",
        "difficulty@qowaiv.org",
        "unaffected@qowaiv.org",
        "how.234@qowaiv.org",
        "Terminated@qowaiv.org.net",
        "PRINCIPLES@qowaiv.org.net",
        "sentiments@qowaiv.org.net",
        "pianoforte@qowaiv.org.net",
        "if@qowaiv.org.net",
        "projection@qowaiv.org.net",
        "impossible.234@qowaiv.org.net",
        "Horses@qowaiv.org.net",
        "pulled@qowaiv.org.net",
        "nature@qowaiv.org.net",
        "favour@qowaiv.org.net",
        "number@qowaiv.org.net",
        "yet@qowaiv.org.net",
        "highly@qowaiv.org.net",
        "his@qowaiv.org.net",
        "has@qowaiv.org.net",
        "old.234@qowaiv.org.net",
        "Contrasted@qowaiv.org.net",
        "literature@qowaiv.org.net",
        "excellence@qowaiv.org.net",
        "he@qowaiv.org.net",
        "admiration@qowaiv.org.net",
        "impression@qowaiv.org.net",
        "insipidity@qowaiv.org.net",
        "so.234@qowaiv.org.net",
        "Scale@qowaiv.org.net",
        "ought@qowaiv.org.net",
        "who@qowaiv.org.net",
        "terms@qowaiv.org.net",
        "after@qowaiv.org.net",
        "own@qowaiv.org.net",
        "quick@qowaiv.org.net",
        "since.234@qowaiv.org.net",
        "Servants@qowaiv.org.net",
        "margaret@qowaiv.org.net",
        "husbands@qowaiv.org.net",
        "to@qowaiv.org.net",
        "screened@qowaiv.org.net",
        "in@qowaiv.org.net",
        "throwing.234@qowaiv.org.net",
        "Imprudence@qowaiv.org.net",
        "oh-saint-clauss@qowaiv.org.net",
        "an.extra.ordinary.day@qowaiv.org.net",
        "collecting@qowaiv.org.net"
    ];

    private readonly EmailAddress[] Parsed = new EmailAddress[Emails.Length];

    [Benchmark]
    public EmailAddress[] Parse()
    {
        for(var i = 0; i < Parsed.Length;i++)
        {
            Parsed[i] = EmailAddress.Parse(Emails[i]);
        }
        return Parsed;
    }
}
