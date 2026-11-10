using Qowaiv.Validation.Messages;
using Must = Qowaiv.Validation.Guarding.Must<ConquerClub.Domain.Game>;

namespace ConquerClub.Domain;

internal static class Musts
{
    [Pure]
    public static Result<Game> BeInPhase(this Must must, GamePhase phase)
        => must.Be(must.Subject.Phase == phase, Messages.MustBeInPhase, phase, must.Subject.Phase);

    [Pure]
    public static Result<Game> BeActivePlayer(this Must must, Player player)
        => must.Be(player == must.Subject.ActivePlayer, Messages.MustBeActive, must.Subject.ActivePlayer, player);

    [Pure]
    public static Result<Game> Exist(this Must must, CountryId country)
        => must.Exist(country, (g, id) => g.Countries.TryById(id), new EntityNotFound(string.Format(Messages.CountryMustExist, country)));

    [Pure]
    public static Result<Game> BeOwned(this Must must, CountryId country, Player by)
       => must.Be(must.Subject.Countries.ById(country).Owner == by, Messages.MustBeOwnedBy, must.Subject.Countries.ById(country).Name, by);

    [Pure]
    public static Result<Game> BeReachable(this Must must, CountryId target, CountryId by)
    {
        var b = must.Subject.Countries.ById(by);
        var t = must.Subject.Countries.ById(target);
        return must.Be(b.Borders.Contains(t), Messages.MustBeReachable, b.Name, t.Name);
    }

    [Pure]
    public static Result<Game> HaveArmiesToAttack(this Must must, CountryId country)
    {
        var c = must.Subject.Countries.ById(country);
        return must.Be(c.Army > 1, Messages.MustHaveArmiesToAttack, c.Name);
    }

    [Pure]
    public static Result<Game> NotExceedArmyBuffer(this Must must, Army army)
      => must.Be(army <= must.Subject.ArmyBuffer, Messages.MustNotExeedArmyBuffer, must.Subject.ArmyBuffer, army);

    [Pure]
    public static Result<Game> BeOwnedBy(this Must must, CountryId country, Player player)
    {
        var c = must.Subject.Countries.ById(country);
        return must.Be(c.Owner == player, Messages.MustBeOwnedBy, c.Name, player);
    }

    [Pure]
    public static Result<Game> NotBeOwnedBy(this Must must, CountryId country, Player player)
    {
        var c = must.Subject.Countries.ById(country);
        return must.NotBe(c.Owner == player, Messages.MustNotBeOwnedBy, c.Name, player);
    }
}
