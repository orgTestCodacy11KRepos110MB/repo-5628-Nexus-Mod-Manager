﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.4216
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Nexus.Client.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "9.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://where/to/get/the/nexus/client")]
        public string ModManagerUrl {
            get {
                return ((string)(this["ModManagerUrl"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool SettingsUpgraded {
            get {
                return ((bool)(this["SettingsUpgraded"]));
            }
            set {
                this["SettingsUpgraded"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Nexus.Client.Settings.SettingsList InstalledGames {
            get {
                return ((global::Nexus.Client.Settings.SettingsList)(this["InstalledGames"]));
            }
            set {
                this["InstalledGames"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool InstalledGamesDetected {
            get {
                return ((bool)(this["InstalledGamesDetected"]));
            }
            set {
                this["InstalledGamesDetected"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string RememberedGameMode {
            get {
                return ((string)(this["RememberedGameMode"]));
            }
            set {
                this["RememberedGameMode"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Ultra")]
        public global::SevenZip.CompressionLevel ModCompressionLevel {
            get {
                return ((global::SevenZip.CompressionLevel)(this["ModCompressionLevel"]));
            }
            set {
                this["ModCompressionLevel"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("SevenZip")]
        public global::SevenZip.OutArchiveFormat ModCompressionFormat {
            get {
                return ((global::SevenZip.OutArchiveFormat)(this["ModCompressionFormat"]));
            }
            set {
                this["ModCompressionFormat"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int SelectedAddModCommandIndex {
            get {
                return ((int)(this["SelectedAddModCommandIndex"]));
            }
            set {
                this["SelectedAddModCommandIndex"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool CheckForNewModVersions {
            get {
                return ((bool)(this["CheckForNewModVersions"]));
            }
            set {
                this["CheckForNewModVersions"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool AddMissingInfoToMods {
            get {
                return ((bool)(this["AddMissingInfoToMods"]));
            }
            set {
                this["AddMissingInfoToMods"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool CloseModManagerAfterGameLaunch {
            get {
                return ((bool)(this["CloseModManagerAfterGameLaunch"]));
            }
            set {
                this["CloseModManagerAfterGameLaunch"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool CloseModManagerAfterGameLaunchIsRemembered {
            get {
                return ((bool)(this["CloseModManagerAfterGameLaunchIsRemembered"]));
            }
            set {
                this["CloseModManagerAfterGameLaunchIsRemembered"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool RememberGameMode {
            get {
                return ((bool)(this["RememberGameMode"]));
            }
            set {
                this["RememberGameMode"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool ScanSubfoldersForMods {
            get {
                return ((bool)(this["ScanSubfoldersForMods"]));
            }
            set {
                this["ScanSubfoldersForMods"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\r\n\t\t\t\t<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n\t\t\t\t<KeyedSettingsOfKeyedSettingsO" +
            "fString />\r\n\t\t\t")]
        public global::Nexus.Client.Settings.KeyedSettings<Nexus.Client.Settings.KeyedSettings<string>> RepositoryAuthenticationTokens {
            get {
                return ((global::Nexus.Client.Settings.KeyedSettings<Nexus.Client.Settings.KeyedSettings<string>>)(this["RepositoryAuthenticationTokens"]));
            }
            set {
                this["RepositoryAuthenticationTokens"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool CheckForUpdatesOnStartup {
            get {
                return ((bool)(this["CheckForUpdatesOnStartup"]));
            }
            set {
                this["CheckForUpdatesOnStartup"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\r\n\t\t\t\t<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n\t\t\t\t<PerGameModeSettingsOfKeyedSet" +
            "tingsOfAddModDescriptor />\r\n\t\t\t")]
        public global::Nexus.Client.Settings.PerGameModeSettings<Nexus.Client.Settings.KeyedSettings<Nexus.Client.ModManagement.AddModDescriptor>> QueuedModsToAdd {
            get {
                return ((global::Nexus.Client.Settings.PerGameModeSettings<Nexus.Client.Settings.KeyedSettings<Nexus.Client.ModManagement.AddModDescriptor>>)(this["QueuedModsToAdd"]));
            }
            set {
                this["QueuedModsToAdd"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\r\n\t\t\t\t<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n\t\t\t\t<PerGameModeSettingsOfKeyedSet" +
            "tingsOfString />\r\n\t\t\t")]
        public global::Nexus.Client.Settings.PerGameModeSettings<Nexus.Client.Settings.KeyedSettings<string>> DelayedSettings {
            get {
                return ((global::Nexus.Client.Settings.PerGameModeSettings<Nexus.Client.Settings.KeyedSettings<string>>)(this["DelayedSettings"]));
            }
            set {
                this["DelayedSettings"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\r\n\t\t\t\t<PerGameModeSettingsOfString />\r\n\t\t\t")]
        public global::Nexus.Client.Settings.PerGameModeSettings<string> InstallationPaths {
            get {
                return ((global::Nexus.Client.Settings.PerGameModeSettings<string>)(this["InstallationPaths"]));
            }
            set {
                this["InstallationPaths"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\r\n\t\t\t\t<PerGameModeSettingsOfBoolean />\r\n\t\t\t")]
        public global::Nexus.Client.Settings.PerGameModeSettings<bool> CompletedSetup {
            get {
                return ((global::Nexus.Client.Settings.PerGameModeSettings<bool>)(this["CompletedSetup"]));
            }
            set {
                this["CompletedSetup"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\r\n\t\t\t\t<PerGameModeSettingsOfString />\r\n\t\t\t")]
        public global::Nexus.Client.Settings.PerGameModeSettings<string> ModFolder {
            get {
                return ((global::Nexus.Client.Settings.PerGameModeSettings<string>)(this["ModFolder"]));
            }
            set {
                this["ModFolder"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\r\n\t\t\t\t<PerGameModeSettingsOfPerGameModeSettingsOfObject />\r\n\t\t\t")]
        public global::Nexus.Client.Settings.PerGameModeSettings<Nexus.Client.Settings.PerGameModeSettings<object>> CustomGameModeSettings {
            get {
                return ((global::Nexus.Client.Settings.PerGameModeSettings<Nexus.Client.Settings.PerGameModeSettings<object>>)(this["CustomGameModeSettings"]));
            }
            set {
                this["CustomGameModeSettings"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\r\n\t\t\t\t<PerGameModeSettingsOfString />\r\n\t\t\t")]
        public global::Nexus.Client.Settings.PerGameModeSettings<string> InstallInfoFolder {
            get {
                return ((global::Nexus.Client.Settings.PerGameModeSettings<string>)(this["InstallInfoFolder"]));
            }
            set {
                this["InstallInfoFolder"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\r\n\t\t\t\t<WindowPositions />\r\n\t\t\t")]
        public global::Nexus.Client.Settings.WindowPositions WindowPositions {
            get {
                return ((global::Nexus.Client.Settings.WindowPositions)(this["WindowPositions"]));
            }
            set {
                this["WindowPositions"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\r\n\t\t\t\t<KeyedSettingsOfString />\r\n\t\t\t")]
        public global::Nexus.Client.Settings.KeyedSettings<string> DockPanelLayouts {
            get {
                return ((global::Nexus.Client.Settings.KeyedSettings<string>)(this["DockPanelLayouts"]));
            }
            set {
                this["DockPanelLayouts"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\r\n\t\t\t\t<ColumnWidths />\r\n\t\t\t")]
        public global::Nexus.Client.Settings.ColumnWidths ColumnWidths {
            get {
                return ((global::Nexus.Client.Settings.ColumnWidths)(this["ColumnWidths"]));
            }
            set {
                this["ColumnWidths"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\r\n\t\t\t\t<SplitterSizes />\r\n\t\t\t")]
        public global::Nexus.Client.Settings.SplitterSizes SplitterSizes {
            get {
                return ((global::Nexus.Client.Settings.SplitterSizes)(this["SplitterSizes"]));
            }
            set {
                this["SplitterSizes"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\r\n\t\t\t\t<PerGameModeSettingsOfString />\r\n\t\t\t")]
        public global::Nexus.Client.Settings.PerGameModeSettings<string> CustomLaunchCommands {
            get {
                return ((global::Nexus.Client.Settings.PerGameModeSettings<string>)(this["CustomLaunchCommands"]));
            }
            set {
                this["CustomLaunchCommands"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\r\n\t\t\t\t<PerGameModeSettingsOfString />\r\n\t\t\t")]
        public global::Nexus.Client.Settings.PerGameModeSettings<string> CustomLaunchCommandArguments {
            get {
                return ((global::Nexus.Client.Settings.PerGameModeSettings<string>)(this["CustomLaunchCommandArguments"]));
            }
            set {
                this["CustomLaunchCommandArguments"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\r\n\t\t\t\t<PerGameModeSettingsOfString />\r\n\t\t\t")]
        public global::Nexus.Client.Settings.PerGameModeSettings<string> SelectedLaunchCommands {
            get {
                return ((global::Nexus.Client.Settings.PerGameModeSettings<string>)(this["SelectedLaunchCommands"]));
            }
            set {
                this["SelectedLaunchCommands"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\r\n\t\t\t\t<KeyedSettingsOfString />\r\n\t\t\t")]
        public global::Nexus.Client.Settings.KeyedSettings<string> RepositoryUsernames {
            get {
                return ((global::Nexus.Client.Settings.KeyedSettings<string>)(this["RepositoryUsernames"]));
            }
            set {
                this["RepositoryUsernames"] = value;
            }
        }
    }
}
