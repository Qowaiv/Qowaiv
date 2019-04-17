﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Qowaiv.DomainModel {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class QowaivDomainModelMessages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal QowaivDomainModelMessages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Qowaiv.DomainModel.QowaivDomainModelMessages", typeof(QowaivDomainModelMessages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The version ({0}) of the event is not successive to the current version ({1})..
        /// </summary>
        internal static string ArgumenException_VersionNotSuccessive {
            get {
                return ResourceManager.GetString("ArgumenException_VersionNotSuccessive", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The identifier of the event does not match the identifier of the aggregate root..
        /// </summary>
        internal static string ArgumentException_InvalidEventId {
            get {
                return ResourceManager.GetString("ArgumentException_InvalidEventId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The events belong to multiple aggregate roots..
        /// </summary>
        internal static string ArgumentException_MultipleAggregates {
            get {
                return ResourceManager.GetString("ArgumentException_MultipleAggregates", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Version {0} does not exist for the aggregate root ({1})..
        /// </summary>
        internal static string ArgumentOutOfRangeException_InvalidVersion {
            get {
                return ResourceManager.GetString("ArgumentOutOfRangeException_InvalidVersion", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The events are out of order as ther versions are not successive..
        /// </summary>
        internal static string EventsOutOfOrderException {
            get {
                return ResourceManager.GetString("EventsOutOfOrderException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The event of the type {0} is not supported for {1}..
        /// </summary>
        internal static string EventTypeNotSupportedException {
            get {
                return ResourceManager.GetString("EventTypeNotSupportedException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The {0} field is immutable and can not be changed..
        /// </summary>
        internal static string ImmutableAttribute_ErrorMessage {
            get {
                return ResourceManager.GetString("ImmutableAttribute_ErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The change tracker has already been initialized..
        /// </summary>
        internal static string InvalidOperationException_ChangeTrackerAlreadyInitialized {
            get {
                return ResourceManager.GetString("InvalidOperationException_ChangeTrackerAlreadyInitialized", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The change tracker has not been initialized..
        /// </summary>
        internal static string InvalidOperationException_ChangeTrackerNotInitialized {
            get {
                return ResourceManager.GetString("InvalidOperationException_ChangeTrackerNotInitialized", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The event stream has already been initialized..
        /// </summary>
        internal static string InvalidOperationException_InitializedEventStream {
            get {
                return ResourceManager.GetString("InvalidOperationException_InitializedEventStream", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Entities that are transient can not be stored in a hash table..
        /// </summary>
        internal static string NotSupported_GetHashCodeOnIsTransient {
            get {
                return ResourceManager.GetString("NotSupported_GetHashCodeOnIsTransient", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The identifier of an entity can not be updated once it is set..
        /// </summary>
        internal static string Validation_UpdateEntityId {
            get {
                return ResourceManager.GetString("Validation_UpdateEntityId", resourceCulture);
            }
        }
    }
}