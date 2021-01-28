namespace NuDoq
{
    /// <summary>
    /// Specifies the type of <see cref="List" /> in use.
    /// </summary>
    public enum ListType
    {
        /// <summary>
        /// The bullet list type.
        /// </summary>
        Bullet,

        /// <summary>
        /// The number list type.
        /// </summary>
        Number,

        /// <summary>
        /// The table list type.
        /// </summary>
        Table,

        /// <summary>
        /// The list type could not be determined.
        /// </summary>
        Unknown,
    }
}