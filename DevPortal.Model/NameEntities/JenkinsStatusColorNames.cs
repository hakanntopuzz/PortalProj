namespace DevPortal.Data
{
    public static class JenkinsStatusColorNames
    {
        static string GenerateStatusColorName(string name)
        {
            return $"{name.ToUpperInvariant().ToLowerInvariant()}";
        }

        public static string Blue => GenerateStatusColorName(nameof(Blue));

        public static string Red => GenerateStatusColorName(nameof(Red));

        public static string Yellow => GenerateStatusColorName(nameof(Yellow));

        public static string Disabled => GenerateStatusColorName(nameof(Disabled));
    }
}