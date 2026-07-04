using System;
using System.Runtime.CompilerServices;
using BhModule.Community.Pathing.Utility;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework.Input;

namespace BhModule.Community.Pathing {

    public enum MarkerClipboardConsentLevel {
        [LocalizedDescription("ModuleSettings_Always")]
        Always,
        [LocalizedDescription("ModuleSettings_OnlyWhenInteractedWith")]
        OnlyWhenInteractedWith,
        [LocalizedDescription("ModuleSettings_Never")]
        Never
    }

    public enum MarkerInfoDisplayMode {
        [LocalizedDescription("ModuleSettings_Default")]
        Default = 0,
        [LocalizedDescription("ModuleSettings_WithoutBackground")]
        WithoutBackground = 1,
        [LocalizedDescription("ModuleSettings_NeverDisplay")]
        NeverDisplay = 100
    }


    public enum MapVisibilityLevel {
        [LocalizedDescription("ModuleSettings_Default")]
        Default,
        [LocalizedDescription("ModuleSettings_Always")]
        Always,
        [LocalizedDescription("ModuleSettings_Never")]
        Never
    }

    public class ModuleSettings {

        private readonly PathingModule _module;

        public ModuleSettings(PathingModule module, SettingCollection settings) {
            _module = module;

            InitGlobalSettings(settings);
            InitPackSettings(settings);
            InitMapSettings(settings);
            InitScriptSettings(settings);
            InitKeyBindSettings(settings);
        }

        #region Global Settings

        private const string GLOBAL_SETTINGS = "global-settings";

        public SettingCollection GlobalSettings { get; private set; }

        public SettingEntry<bool> GlobalPathablesEnabled      { get; private set; }

        private void InitGlobalSettings(SettingCollection settings) {
            this.GlobalSettings = settings.AddSubCollection(GLOBAL_SETTINGS);

            this.GlobalPathablesEnabled = this.GlobalSettings.DefineSetting(nameof(this.GlobalPathablesEnabled), true);
        }

        #endregion

        #region Pack Settings

        private const string PACK_SETTINGS = "pack-settings";

        public SettingCollection PackSettings { get; private set; }

        public SettingEntry<bool>                        PackWorldPathablesEnabled                { get; private set; }
        public SettingEntry<float>                       PackMaxOpacityOverride                   { get; private set; }
        public SettingEntry<float>                       PackMaxViewDistance                      { get; private set; }
        public SettingEntry<float>                       PackMaxTrailAnimationSpeed               { get; private set; }
        public SettingEntry<float>                       PackMarkerScale                          { get; private set; }
        public SettingEntry<bool>                        PackFadeTrailsAroundCharacter            { get; private set; }
        public SettingEntry<bool>                        PackFadePathablesDuringCombat            { get; private set; }
        public SettingEntry<bool>                        PackFadeMarkersBetweenCharacterAndCamera { get; private set; }
        public SettingEntry<bool>                        PackAllowMarkersToAutomaticallyHide      { get; private set; }
        public SettingEntry<MarkerClipboardConsentLevel> PackMarkerConsentToClipboard             { get; private set; }
        public SettingEntry<MarkerInfoDisplayMode>       PackInfoDisplayMode                      { get; private set; }
        public SettingEntry<bool>                        PackAllowInfoText                        { get; private set; }
        public SettingEntry<bool>                        PackAllowInteractIcon                    { get; private set; }
        public SettingEntry<bool>                        PackAllowMarkersToAnimate                { get; private set; }
        public SettingEntry<bool>                        PackEnableSmartCategoryFilter            { get; private set; }
        public SettingEntry<bool>                        PackShowWhenCategoriesAreFiltered        { get; private set; }
        public SettingEntry<bool>                        PackTruncateLongCategoryNames            { get; private set; }
        public SettingEntry<bool>                        PackShowHiddenMarkersReducedOpacity      { get; private set; }
        public SettingEntry<bool>                        PackShowTooltipsOnAchievements           { get; private set; }

        private void InitPackSettings(SettingCollection settings) {
            this.PackSettings = settings.AddSubCollection(PACK_SETTINGS);

            // TODO: Add string to strings.resx for localization.
            // TODO: Add description to settings.
            this.PackWorldPathablesEnabled                = this.PackSettings.DefineSetting(nameof(this.PackWorldPathablesEnabled),                true, () => Strings.Setting_PackWorldPathablesEnabled);
            this.PackMaxOpacityOverride                   = this.PackSettings.DefineSetting(nameof(this.PackMaxOpacityOverride),                   1f, () => Strings.Setting_PackMaxOpacityOverride, () => "");
            this.PackMaxViewDistance                      = this.PackSettings.DefineSetting(nameof(this.PackMaxViewDistance),                      25000f, () => Strings.Setting_PackMaxViewDistance, () => "");
            this.PackMaxTrailAnimationSpeed               = this.PackSettings.DefineSetting(nameof(this.PackMaxTrailAnimationSpeed),               10f, () => Strings.Setting_PackMaxTrailAnimationSpeed, () => "");
            this.PackMarkerScale                          = this.PackSettings.DefineSetting(nameof(this.PackMarkerScale),                          1f,   () => Strings.Setting_PackMarkerScale, () => Strings.Setting_PackMarkerScaleDesc);
            this.PackFadeTrailsAroundCharacter            = this.PackSettings.DefineSetting(nameof(this.PackFadeTrailsAroundCharacter),            true, () => Strings.Setting_PackFadeTrailsAroundCharacter, () => Strings.Setting_PackFadeTrailsAroundCharacterDesc);
            this.PackFadePathablesDuringCombat            = this.PackSettings.DefineSetting(nameof(this.PackFadePathablesDuringCombat),            true, () => Strings.Setting_PackFadePathablesDuringCombat, () => Strings.Setting_PackFadePathablesDuringCombatDesc);
            this.PackFadeMarkersBetweenCharacterAndCamera = this.PackSettings.DefineSetting(nameof(this.PackFadeMarkersBetweenCharacterAndCamera), true, () => Strings.Setting_PackFadeMarkersBetweenCharacterAndCamera, () => Strings.Setting_PackFadeMarkersBetweenCharacterAndCameraDesc);
            this.PackAllowMarkersToAutomaticallyHide      = this.PackSettings.DefineSetting(nameof(this.PackAllowMarkersToAutomaticallyHide),      true, () => Strings.Setting_PackAllowMarkersToAutomaticallyHide, () => Strings.Setting_PackAllowMarkersToAutomaticallyHideDesc);
            this.PackMarkerConsentToClipboard             = this.PackSettings.DefineSetting(nameof(this.PackMarkerConsentToClipboard),             MarkerClipboardConsentLevel.Always, () => Strings.Setting_PackMarkerConsentToClipboard, () => string.Format(Strings.Setting_PackMarkerConsentToClipboardDescription, Blish_HUD.Common.Gw2.KeyBindings.Interact.GetBindingDisplayText()));
            //this.PackAllowInfoText                        = this.PackSettings.DefineSetting(nameof(this.PackAllowInfoText),                        true, () => "Allow Markers to Show Info Text On-Screen", () => "If enabled, certain markers will be able to show information when your character is nearby to the marker.");
            this.PackInfoDisplayMode                      = this.PackSettings.DefineSetting(nameof(this.PackInfoDisplayMode),                      MarkerInfoDisplayMode.Default, () => Strings.Setting_PackInfoDisplayMode, () => Strings.Setting_PackInfoDisplayModeDesc);
            this.PackAllowInteractIcon                    = this.PackSettings.DefineSetting(nameof(this.PackAllowInteractIcon),                    true, () => Strings.Setting_PackAllowInteractIcon, () => Strings.Setting_PackAllowInteractIconDesc);
            this.PackAllowMarkersToAnimate                = this.PackSettings.DefineSetting(nameof(this.PackAllowMarkersToAnimate),                true, () => Strings.Setting_PackAllowMarkersToAnimate, () => Strings.Setting_PackAllowMarkersToAnimateDesc);
            this.PackEnableSmartCategoryFilter            = this.PackSettings.DefineSetting(nameof(this.PackEnableSmartCategoryFilter),            true, () => Strings.Setting_PackEnableSmartCategoryFilter, () => Strings.Setting_PackEnableSmartCategoryFilterDesc);
            this.PackShowWhenCategoriesAreFiltered        = this.PackSettings.DefineSetting(nameof(this.PackShowWhenCategoriesAreFiltered),        true, () => Strings.Setting_PackShowWhenCategoriesAreFiltered, () => Strings.Setting_PackShowWhenCategoriesAreFilteredDesc);
            this.PackTruncateLongCategoryNames            = this.PackSettings.DefineSetting(nameof(this.PackTruncateLongCategoryNames),            false, () => Strings.Setting_PackTruncateLongCategoryNames, () => Strings.Setting_PackTruncateLongCategoryNamesDesc);
            this.PackShowHiddenMarkersReducedOpacity      = this.PackSettings.DefineSetting(nameof(this.PackShowHiddenMarkersReducedOpacity),      false, () => Strings.Setting_PackShowHiddenMarkersReducedOpacity, () => Strings.Setting_PackShowHiddenMarkersReducedOpacityDesc);
            this.PackShowTooltipsOnAchievements           = this.PackSettings.DefineSetting(nameof(this.PackShowTooltipsOnAchievements),           false, () => Strings.Setting_PackShowTooltipsOnAchievements, () => Strings.Setting_PackShowTooltipsOnAchievementsDesc);

            this.PackMaxOpacityOverride.SetRange(0f, 1f);
            this.PackMaxViewDistance.SetRange(25f, 50000f);
            this.PackMaxTrailAnimationSpeed.SetRange(0f, 10f);
            this.PackMarkerScale.SetRange(0.1f, 4f);

            // Reset this one back to false.
            this.PackShowHiddenMarkersReducedOpacity.Value = false;
        }

        #endregion

        #region Map Settings

        private const string MAP_SETTINGS = "map-settings";

        public SettingCollection MapSettings { get; private set; }

        public SettingEntry<bool>               MapPathablesEnabled                   { get; private set; }
        public SettingEntry<MapVisibilityLevel> MapMarkerVisibilityLevel              { get; private set; }
        public SettingEntry<MapVisibilityLevel> MapTrailVisibilityLevel               { get; private set; }
        public SettingEntry<float>              MapDrawOpacity                        { get; private set; }
        public SettingEntry<MapVisibilityLevel> MiniMapMarkerVisibilityLevel          { get; private set; }
        public SettingEntry<MapVisibilityLevel> MiniMapTrailVisibilityLevel           { get; private set; }
        public SettingEntry<float>              MiniMapDrawOpacity                    { get; private set; }
        public SettingEntry<bool>               MapShowAboveBelowIndicators           { get; private set; }
        public SettingEntry<bool>               MapFadeVerticallyDistantTrailSegments { get; private set; }
        public SettingEntry<float>              MapTrailWidth                         { get; private set; }
        public SettingEntry<bool>               MapShowTooltip                        { get; private set; }

        private void InitMapSettings(SettingCollection settings) {
            this.MapSettings = settings.AddSubCollection(MAP_SETTINGS);

            // TODO: Add string to strings.resx for localization.
            // TODO: Add description to settings.
            this.MapPathablesEnabled                   = this.MapSettings.DefineSetting(nameof(this.MapPathablesEnabled),                   true,                       () => Strings.Setting_MapPathablesEnabled);
            this.MapMarkerVisibilityLevel              = this.MapSettings.DefineSetting(nameof(this.MapMarkerVisibilityLevel),              MapVisibilityLevel.Default, () => Strings.Setting_MapShowMarkersOnFullscreen,  () => "");
            this.MapTrailVisibilityLevel               = this.MapSettings.DefineSetting(nameof(this.MapTrailVisibilityLevel),               MapVisibilityLevel.Default, () => Strings.Setting_MapShowTrailsOnFullscreen,   () => "");
            this.MapDrawOpacity                        = this.MapSettings.DefineSetting(nameof(this.MapDrawOpacity),                        1f,                         () => Strings.Setting_MapDrawOpacity,                 () => "");
            this.MiniMapMarkerVisibilityLevel          = this.MapSettings.DefineSetting(nameof(this.MiniMapMarkerVisibilityLevel),          MapVisibilityLevel.Default, () => Strings.Setting_MapShowMarkersOnCompass,     () => "");
            this.MiniMapTrailVisibilityLevel           = this.MapSettings.DefineSetting(nameof(this.MiniMapTrailVisibilityLevel),           MapVisibilityLevel.Default, () => Strings.Setting_MapShowTrailsOnCompass,      () => "");
            this.MiniMapDrawOpacity                    = this.MapSettings.DefineSetting(nameof(this.MiniMapDrawOpacity),                    1f,                         () => Strings.Setting_MiniMapDrawOpacity,                    () => "");
            this.MapShowAboveBelowIndicators           = this.MapSettings.DefineSetting(nameof(this.MapShowAboveBelowIndicators),           true,                       () => Strings.Setting_MapShowAboveBelowIndicators, () => "");
            this.MapFadeVerticallyDistantTrailSegments = this.MapSettings.DefineSetting(nameof(this.MapFadeVerticallyDistantTrailSegments), true,                       () => Strings.Setting_MapFadeVerticallyDistantTrailSegments, () => "");
            this.MapShowTooltip                        = this.MapSettings.DefineSetting(nameof(this.MapShowTooltip),                        true,                       () => Strings.Setting_MapShowTooltip, () => Strings.Setting_MapShowTooltipDesc);
            this.MapTrailWidth                         = this.MapSettings.DefineSetting(nameof(this.MapTrailWidth),                         2f,                         () => Strings.Setting_MapTrailWidth, () => Strings.Setting_MapTrailWidthDesc);

            this.MapDrawOpacity.SetRange(0f, 1f);
            this.MiniMapDrawOpacity.SetRange(0f, 1f);
            this.MapTrailWidth.SetRange(0.5f, 4.5f);
        }

        #endregion

        #region Script Settings

        private const string SCRIPT_SETTINGS = "script-settings";

        public SettingCollection ScriptSettings { get; private set; }

        public SettingEntry<bool> ScriptsEnabled        { get; private set; }
        public SettingEntry<bool> ScriptsConsoleEnabled { get; private set; }

        private void InitScriptSettings(SettingCollection settings) {
            this.ScriptSettings = settings.AddSubCollection(SCRIPT_SETTINGS);

            this.ScriptsEnabled        = this.ScriptSettings.DefineSetting(nameof(this.ScriptsEnabled),         true, () => Strings.Setting_ScriptsEnabled,    () => Strings.Setting_ScriptsEnabledDesc);
            this.ScriptsConsoleEnabled = this.ScriptSettings.DefineSetting(nameof(this.ScriptsConsoleEnabled), false, () => Strings.Setting_ScriptsConsoleEnabled, () => Strings.Setting_ScriptsConsoleEnabledDesc);
        }

        #endregion

        #region Keybind Settings

        private const string KEYBIND_SETTINGS = "keybind-settings";

        public SettingCollection KeyBindSettings { get; private set; }

        public SettingEntry<KeyBinding> KeyBindTogglePathables      { get; private set; }
        public SettingEntry<KeyBinding> KeyBindToggleWorldPathables { get; private set; }
        public SettingEntry<KeyBinding> KeyBindToggleMapPathables   { get; private set; }
        public SettingEntry<KeyBinding> KeyBindReloadMarkerPacks    { get; private set; }

        private void InitKeyBindSettings(SettingCollection settings) {
            this.KeyBindSettings = settings.AddSubCollection(KEYBIND_SETTINGS);

            // TODO: Add strings to strings.resx for localization.
            // TODO: Add description to settings.
            this.KeyBindTogglePathables      = this.KeyBindSettings.DefineSetting(nameof(this.KeyBindTogglePathables), new KeyBinding(),      () => Strings.Setting_KeyBindTogglePathables,          () => "");
            this.KeyBindToggleWorldPathables = this.KeyBindSettings.DefineSetting(nameof(this.KeyBindToggleWorldPathables), new KeyBinding(), () => Strings.Setting_KeyBindToggleWorldPathables, () => "");
            this.KeyBindToggleMapPathables   = this.KeyBindSettings.DefineSetting(nameof(this.KeyBindToggleMapPathables), new KeyBinding(),   () => Strings.Setting_KeyBindToggleMapPathables,   () => "");
            this.KeyBindReloadMarkerPacks    = this.KeyBindSettings.DefineSetting(nameof(this.KeyBindReloadMarkerPacks), new KeyBinding(),    () => Strings.Setting_KeyBindReloadMarkerPacks,     () => "");

            HandleInternalKeyBinds();
        }

        private void HandleInternalKeyBinds() {
            this.KeyBindTogglePathables.Value.BlockSequenceFromGw2 = true;
            this.KeyBindTogglePathables.Value.Enabled = true;

            this.KeyBindToggleWorldPathables.Value.BlockSequenceFromGw2 = true;
            this.KeyBindToggleWorldPathables.Value.Enabled = true;

            this.KeyBindToggleMapPathables.Value.BlockSequenceFromGw2 = true;
            this.KeyBindToggleMapPathables.Value.Enabled = true;

            this.KeyBindReloadMarkerPacks.Value.BlockSequenceFromGw2 = true;
            this.KeyBindReloadMarkerPacks.Value.Enabled = true;

            this.KeyBindTogglePathables.Value.Activated      += ToggleGlobalPathablesEnabled;
            this.KeyBindToggleWorldPathables.Value.Activated += TogglePackWorldPathablesEnabled;
            this.KeyBindToggleMapPathables.Value.Activated   += ToggleMapPathablesEnabled;
            this.KeyBindReloadMarkerPacks.Value.Activated    += ReloadMarkerPacks;
        }

        private void ReloadMarkerPacks(object sender, EventArgs e) {
            if (_module.PackInitiator != null) { 
                _module.PackInitiator.ReloadPacks();
            }
        }

        private void ToggleGlobalPathablesEnabled(object sender, EventArgs e) {
            this.GlobalPathablesEnabled.Value = !this.GlobalPathablesEnabled.Value;
        }

        private void TogglePackWorldPathablesEnabled(object sender, EventArgs e) {
            this.PackWorldPathablesEnabled.Value = !this.PackWorldPathablesEnabled.Value;
        }

        private void ToggleMapPathablesEnabled(object sender, EventArgs e) {
            this.MapPathablesEnabled.Value = !this.MapPathablesEnabled.Value;
        }

        #endregion

        public void Unload() {
            this.KeyBindTogglePathables.Value.Enabled      = false;
            this.KeyBindToggleWorldPathables.Value.Enabled = false;
            this.KeyBindToggleMapPathables.Value.Enabled   = false;
            this.KeyBindReloadMarkerPacks.Value.Enabled    = false;

            this.KeyBindTogglePathables.Value.Activated      -= ToggleGlobalPathablesEnabled;
            this.KeyBindToggleWorldPathables.Value.Activated -= TogglePackWorldPathablesEnabled;
            this.KeyBindToggleMapPathables.Value.Activated   -= ToggleMapPathablesEnabled;
            this.KeyBindReloadMarkerPacks.Value.Activated    -= ReloadMarkerPacks;
        }

    }
}
