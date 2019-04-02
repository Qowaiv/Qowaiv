namespace Qowaiv.DomainModel.Tracking
{
    /// <summary>Implements <see cref="ITrackableChange"/> for changing a property.</summary>
    public class PropertyChanged : ITrackableChange
    {
        /// <summary>Creates a new instance of a <see cref="PropertyChanged"/>.</summary>
        /// <param name="properties">
        /// The property collection that contains the property.
        /// </param>
        /// <param name="propertyName">
        /// The name of the property.
        /// </param>
        /// <param name="value">
        /// The new value.
        /// </param>
        public PropertyChanged(PropertyCollection properties, string propertyName, object value)
        {
            _properties = Guard.NotNull(properties, nameof(properties));
            _propertyName = Guard.NotNullOrEmpty(propertyName, nameof(propertyName));
            _originalValue = _properties[_propertyName];
            _value = value;
        }

        private readonly PropertyCollection _properties;
        private readonly string _propertyName;
        private readonly object _originalValue;
        private readonly object _value;

        /// <inheritdoc />
        public void Apply() => _properties[_propertyName] = _value;

        /// <inheritdoc />
        public void Rollback() => _properties[_propertyName] = _originalValue;
    }
}
