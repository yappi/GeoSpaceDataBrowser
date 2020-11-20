namespace GeospaceDataBrowser.Web.Data
{
    using System;
    using System.Web.UI;

    /// <summary>
    /// Represents a helper class to work with user controls.
    /// </summary>
    public static class ControlHelper
    {
        /// <summary>
        /// Gets a first parent object in the in inheritance chain of the specific type.
        /// </summary>
        /// <typeparam name="T">The type of the parent to return.</typeparam>
        /// <param name="control">The control, a parent of which to return.</param>
        /// <returns>The first parent object of the specific type met in the inheritance chain.</returns>
        public static T GetParent<T>(this Control control) 
            where T: class
        {
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }

            Control parent = control.Parent;
            while (parent != null && !(parent is T))
            {
                parent = parent.Parent;
            }

            return parent as T;
        }
    }
}