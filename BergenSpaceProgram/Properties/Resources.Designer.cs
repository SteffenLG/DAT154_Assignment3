﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BergenSpaceProgram.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("BergenSpaceProgram.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;UTF-8&quot; standalone=&quot;yes&quot;?&gt;
        ///&lt;data-set xmlns:xsi=&quot;http://www.w3.org/2001/XMLSchema-instance&quot;&gt;
        ///	&lt;record&gt;
        ///		&lt;Name&gt;Sun&lt;/Name&gt;
        ///		&lt;Dad&gt;0&lt;/Dad&gt;
        ///		&lt;OrbitalRadius&gt;0&lt;/OrbitalRadius&gt;
        ///		&lt;OrbitalPeriod&gt;0&lt;/OrbitalPeriod&gt;
        ///	&lt;/record&gt;
        ///	&lt;record&gt;
        ///		&lt;Name&gt;Mercury&lt;/Name&gt;
        ///		&lt;Dad&gt;Sun&lt;/Dad&gt;
        ///		&lt;OrbitalRadius&gt;57910&lt;/OrbitalRadius&gt;
        ///		&lt;OrbitalPeriod&gt;87.97&lt;/OrbitalPeriod&gt;
        ///	&lt;/record&gt;
        ///	&lt;record&gt;
        ///		&lt;Name&gt;Venus&lt;/Name&gt;
        ///		&lt;Dad&gt;Sun&lt;/Dad&gt;
        ///		&lt;OrbitalRadius&gt;108200&lt;/OrbitalRadius&gt;
        ///		&lt;OrbitalPeriod&gt;224. [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Planets {
            get {
                return ResourceManager.GetString("Planets", resourceCulture);
            }
        }
    }
}
