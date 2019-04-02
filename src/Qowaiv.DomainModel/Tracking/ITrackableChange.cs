namespace Qowaiv.DomainModel.Tracking
{
    /// <summary>Represents a trackeable change.</summary>
    public interface ITrackableChange
    {
        /// <summary>Applies the change.</summary>
        void Apply();

        /// <summary>Rolls back the change.</summary>
        void Rollback();
    }
}
