﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AccountingPC.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.6.0.0")]
    internal sealed partial class SecuritySettings : global::System.Configuration.ApplicationSettingsBase {
        
        private static SecuritySettings defaultInstance = ((SecuritySettings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new SecuritySettings())));
        
        public static SecuritySettings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string LOGIN_HASH {
            get {
                return ((string)(this["LOGIN_HASH"]));
            }
            set {
                this["LOGIN_HASH"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string PASSWORD_HASH {
            get {
                return ((string)(this["PASSWORD_HASH"]));
            }
            set {
                this["PASSWORD_HASH"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string LOGIN {
            get {
                return ((string)(this["LOGIN"]));
            }
            set {
                this["LOGIN"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string PASSWORD {
            get {
                return ((string)(this["PASSWORD"]));
            }
            set {
                this["PASSWORD"] = value;
            }
        }
    }
}
