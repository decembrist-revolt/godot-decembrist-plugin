using System;
using Decembrist.Utils.Callback;
using Godot;
using VoidFunc = System.Action;

namespace Decembrist.Utils
{
    public static class Popups
    {
        public const string AboutToShowSignal = "about_to_show";
        public const string PopupHideSignal = "popup_hide";
        
        /// <summary>
        /// Emitted when a popup is about to be shown. This is often used in PopupMenu to clear the list of options
        /// then create a new one according to the current context.
        /// </summary>
        /// <returns>Unsubscribe callback</returns>
        public static VoidFunc OnAboutToShow(this Popup popup, VoidFunc action)
        {
            var callback = popup.Subscribe(AboutToShowSignal, action);
            return () => popup.Unsubscribe(AboutToShowSignal, callback);
        }
        
        /// <summary>
        /// Emitted when a popup is hidden.
        /// </summary>
        /// <returns>Unsubscribe callback</returns>
        public static VoidFunc OnPopupHide(this Popup popup, VoidFunc action)
        {
            var callback = popup.Subscribe(PopupHideSignal, action);
            return () => popup.Unsubscribe(PopupHideSignal, callback);
        }
    }

    public static class ScriptCreateDialogs
    {
        public const string ScriptCreatedSignal = "script_created";
        
        /// <summary>
        /// Emitted when the user clicks the OK button.
        /// </summary>
        /// <param name="dialog">This dialog</param>
        /// <param name="action">action ( Script script )</param>
        /// <returns>Unsubscribe callback</returns>
        public static VoidFunc OnScriptCreated(this ScriptCreateDialog dialog, Action<Script> action)
        {
            var callback = dialog.Subscribe(ScriptCreatedSignal, action);
            return () => dialog.Unsubscribe(ScriptCreatedSignal, callback);
        }
    }
    
    public static class FileDialogs
    {
        public const string DirSelectedSignal = "dir_selected";
        public const string FileSelectedSignal = "file_selected";
        public const string FilesSelectedSignal = "files_selected";
        
        /// <summary>
        /// Emitted when the user selects a directory.
        /// </summary>
        /// <param name="dialog">This dialog</param>
        /// <param name="action">action ( String dir )</param>
        /// <returns>Unsubscribe callback</returns>
        public static VoidFunc OnDirSelected(this FileDialog dialog, Action<string> action)
        {
            var callback = dialog.Subscribe(DirSelectedSignal, action);
            return () => dialog.Unsubscribe(DirSelectedSignal, callback);
        }
        
        /// <summary>
        /// Emitted when the user selects a file by double-clicking it or pressing the OK button.
        /// </summary>
        /// <param name="dialog">This dialog</param>
        /// <param name="action">action ( String path )</param>
        /// <returns>Unsubscribe callback</returns>
        public static VoidFunc OnFileSelected(this FileDialog dialog, Action<string> action)
        {
            var callback = dialog.Subscribe(FileSelectedSignal, action);
            return () => dialog.Unsubscribe(FileSelectedSignal, callback);
        }
        
        /// <summary>
        /// Emitted when the user selects multiple files.
        /// </summary>
        /// <param name="dialog">This dialog</param>
        /// <param name="action">action ( string[] paths )</param>
        /// <returns>Unsubscribe callback</returns>
        public static VoidFunc OnFilesSelected(this FileDialog dialog, Action<string[]> action)
        {
            var callback = dialog.Subscribe(FilesSelectedSignal, action);
            return () => dialog.Unsubscribe(FilesSelectedSignal, callback);
        }
    }
}