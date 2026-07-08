using System;

namespace Rampastring.XNAUI;

/// <summary>
/// Clipboard access for UI controls. On desktop this forwards to the operating system
/// clipboard through TextCopy. In the browser (Blazor WebAssembly) there is no synchronous
/// clipboard API, so a cached value is used instead: the host page pushes the real clipboard
/// contents in ahead of paste operations (from the DOM paste event) and is notified through
/// <see cref="TextSet"/> when the user copies text, so it can forward it to the browser clipboard.
/// </summary>
public static class RClipboard
{
    /// <summary>
    /// Raised when text is copied from a UI control. The Blazor host subscribes to this to
    /// forward the text to the browser clipboard. No-op subscribers are fine on desktop.
    /// </summary>
    public static event Action<string> TextSet;

#if BLAZOR
    private static string cachedText = string.Empty;

    public static string GetText() => cachedText;

    public static void SetText(string text)
    {
        cachedText = text ?? string.Empty;
        TextSet?.Invoke(cachedText);
    }

    /// <summary>
    /// Updates the cached clipboard text from the host without raising <see cref="TextSet"/>.
    /// Called by the Blazor host when the browser delivers the real clipboard contents.
    /// </summary>
    public static void UpdateFromHost(string text) => cachedText = text ?? string.Empty;
#else
    public static string GetText() => TextCopy.ClipboardService.GetText();

    public static void SetText(string text)
    {
        TextCopy.ClipboardService.SetText(text ?? string.Empty);
        TextSet?.Invoke(text ?? string.Empty);
    }

    public static void UpdateFromHost(string text)
    {
        // Desktop reads the operating system clipboard directly; nothing to cache.
    }
#endif
}
