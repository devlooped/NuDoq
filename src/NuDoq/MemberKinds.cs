using System;

namespace NuDoq
{
    /// <summary>
    /// Kind (or kinds for semantically-augmented readings)
    /// of member node.
    /// </summary>
    /// <remarks>
    /// See http://msdn.microsoft.com/en-us/library/fsbx0t7x(v=vs.80).aspx.
    /// </remarks>
    [Flags]
    public enum MemberKinds
    {
        /// <summary>
        /// The type of member could not be determined.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The member is a type.
        /// </summary>
        Type = 1,

        /// <summary>
        /// The member is a field.
        /// </summary>
        Field = 2,

        /// <summary>
        /// The member is a property.
        /// </summary>
        Property = 4,

        /// <summary>
        /// The member is a method.
        /// </summary>
        Method = 8,
        /// <summary>
        /// The member is an event.
        /// </summary>
        Event = 16,
        
        // Here start the extended member types 
        // available only when you pass in an 
        // assembly for the reading.
        /// <summary>
        /// The member is an extension method. This 
        /// kind is used in conjunction with <see cref="Method"/> 
        /// as a combined flag.
        /// </summary>
        ExtensionMethod = 32,

        /// <summary>
        /// The member is an enumeration. This 
        /// kind is used in conjunction with <see cref="Type"/> 
        /// as a combined flag.
        /// </summary>
        Enum = 128,

        /// <summary>
        /// The member is an interface. This 
        /// kind is used in conjunction with <see cref="Type"/> 
        /// as a combined flag.
        /// </summary>
        Interface = 256,

        /// <summary>
        /// The member is a class. This 
        /// kind is used in conjunction with <see cref="Type"/> 
        /// as a combined flag.
        /// </summary>
        Class = 512,

        /// <summary>
        /// The member is a struct. This 
        /// kind is used in conjunction with <see cref="Type"/> 
        /// as a combined flag.
        /// </summary>
        Struct = 1024,
    }
}