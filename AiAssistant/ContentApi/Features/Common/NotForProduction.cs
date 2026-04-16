

/// <summary>
/// This is just a quick way for me to get away with making
/// crappy test methods without accidentally mapping them to production
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
public sealed class NotForProduction : Attribute
{}