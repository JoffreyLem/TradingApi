namespace XtbLibrairie.utils
{
    internal abstract class CustomTag
    {
        private static int lastTag;
        private static readonly int maxTag = 1000000;

        private static readonly object locker = new();

        /// <summary>
        ///     Return next custom tag.
        /// </summary>
        /// <returns>Next custom tag</returns>
        public static string Next()
        {
            lock (locker)
            {
                lastTag = ++lastTag % maxTag;
                return lastTag.ToString();
            }
        }
    }
}