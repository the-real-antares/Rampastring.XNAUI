#if BLAZOR
namespace Rampastring.XNAUI.Input
{
    /// <summary>
    /// Character-input channel for the browser (Blazor WebAssembly) build. The KNI game window's
    /// TextInput event suppresses characters while a control or alt key is down, which breaks
    /// AltGr combinations (for example the backslash on many European layouts) and macOS Option
    /// compositions. The Blazor host instead forwards fully composed characters from the DOM
    /// keydown event through <see cref="TriggerCharEntered"/>, and text controls subscribe to
    /// <see cref="CharEntered"/> exactly like the XNA build does with its WndProc hook.
    /// </summary>
    public static class KeyboardEventInput
    {
        public delegate void CharEnteredHandler(object sender, KeyboardEventArgs e);

        /// <summary>
        /// Raised when a composed character has been entered.
        /// </summary>
        public static event CharEnteredHandler CharEntered;

        /// <summary>
        /// Called by the Blazor host to deliver a composed character to subscribed controls.
        /// </summary>
        public static void TriggerCharEntered(char character)
        {
            CharEntered?.Invoke(null, new KeyboardEventArgs(character, 0));
        }
    }
}
#endif
