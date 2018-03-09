/// <reference path="Jasmine.d.ts"/>
/// <reference path="../../src/Qowaiv.TypeScript/IEquatable.ts"/>
/// <reference path="../../src/Qowaiv.TypeScript/IFormattable.ts"/>
/// <reference path="../../src/Qowaiv.TypeScript/IJsonStringifyable.ts"/>
/// <reference path="../../src/Qowaiv.TypeScript/TimeSpan.ts" />

describe("TimeSpan: ", () => {

    it("getDays should be 1.", () => {

        var span = new Qowaiv.TimeSpan(1, 13, 15, 17, 19);
        expect(span.getDays()).toBe(1);
    });

    it("getHours should be 13.", () => {

        var span = new Qowaiv.TimeSpan(1, 13, 15, 17, 19);
        expect(span.getHours()).toBe(13);
    });

    it("getMinutes should be 15.", () => {

        var span = new Qowaiv.TimeSpan(1, 13, 15, 17, 19);
        expect(span.getMinutes()).toBe(15);
    });

    it("getSeconds should be 17.", () => {

        var span = new Qowaiv.TimeSpan(1, 13, 15, 17, 19);
        expect(span.getSeconds()).toBe(17);
    });

    it("getMilliseconds should be 19.", () => {

        var span = new Qowaiv.TimeSpan(1, 13, 15, 17, 19);
        expect(span.getMilliseconds()).toBe(19);
    });

    it("getTotalDays should be 1.5522803125", () => {

        var span = new Qowaiv.TimeSpan(1, 13, 15, 17, 19);
        expect(span.getTotalDays()).toBe(1.5522803125 );
    });

    it("getTotalHours should be 37.2547275", () => {

        var span = new Qowaiv.TimeSpan(1, 13, 15, 17, 19);
        expect(span.getTotalHours()).toBe(37.2547275);
    });

    it("getTotalMinutes should be 2235.28365", () => {

        var span = new Qowaiv.TimeSpan(1, 13, 15, 17, 19);
        expect(span.getTotalMinutes()).toBe(2235.28365 );
    });

    it("getTotalSeconds should be 134117.019", () => {

        var span = new Qowaiv.TimeSpan(1, 13, 15, 17, 19);
        expect(span.getTotalSeconds()).toBe(134117.019);
    });

    it("getTotalMilliseconds should be 134117019", () => {

        var span = new Qowaiv.TimeSpan(1, 13, 15, 17, 19);
        expect(span.getTotalMilliseconds()).toBe(134117019);
    });
});
