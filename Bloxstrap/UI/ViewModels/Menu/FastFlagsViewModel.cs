﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Wpf.Ui.Mvvm.Contracts;

using CommunityToolkit.Mvvm.Input;

using Bloxstrap.UI.Elements.Menu.Pages;

namespace Bloxstrap.UI.ViewModels.Menu
{
    public class FastFlagsViewModel : NotifyPropertyChangedViewModel
    {
        private readonly Page _page;

        public FastFlagsViewModel(Page page) 
        { 
            _page = page;
        }

        private void OpenFastFlagEditor()
        {
            if (Window.GetWindow(_page) is INavigationWindow window)
                window.Navigate(typeof(FastFlagEditorPage));
        }

        public ICommand OpenFastFlagEditorCommand => new RelayCommand(OpenFastFlagEditor);

        public Visibility ShowDebugFlags => App.Settings.Prop.OhHeyYouFoundMe ? Visibility.Visible : Visibility.Collapsed;

        public bool HttpRequestLogging
        {
            get => App.FastFlags.GetPreset("HTTP.Log") is not null;
            set => App.FastFlags.SetPreset("HTTP.Log", value ? 12 : null);
        }

        public string HttpRequestProxy
        {
            get => App.FastFlags.GetPreset("HTTP.Proxy.Address.1") ?? "";

            set
            {
                App.FastFlags.SetPreset("HTTP.Proxy.Enable", String.IsNullOrEmpty(value) ? null : true);
                App.FastFlags.SetPreset("HTTP.Proxy.Address", String.IsNullOrEmpty(value) ? null : value);
            }
        }

        public string StateOverlayFlags
        {
            get => App.FastFlags.GetPreset("UI.FlagState") ?? "";
            set => App.FastFlags.SetPreset("UI.FlagState", String.IsNullOrEmpty(value) ? null : value);
        }

        public int FramerateLimit
        {
            get => int.TryParse(App.FastFlags.GetPreset("Rendering.Framerate"), out int x) ? x : 60;
            set => App.FastFlags.SetPreset("Rendering.Framerate", value);
        }

        public IReadOnlyDictionary<string, string> RenderingModes => FastFlagManager.RenderingModes;

        public string SelectedRenderingMode
        {
            get => App.FastFlags.GetPresetEnum(RenderingModes, "Rendering.Mode", "True");
            set
            {
                App.FastFlags.SetPresetEnum("Rendering.Mode", RenderingModes[value], "True");
                App.FastFlags.CheckManualFullscreenPreset();
            }
        }

        public string AntiAliasQuality
        {
            get => App.FastFlags.GetPresetEnum(RenderingModes, "Rendering.MSAA", "True");
            set
            {
                App.FastFlags.SetPresetEnum("Rendering.MSAA", RenderingModes[value], "True");
            }
        }

        public bool FixDisplayScaling
        {
            get => App.FastFlags.GetPreset("Rendering.DisableScaling") == "True";
            set => App.FastFlags.SetPreset("Rendering.DisableScaling", value ? "True" : null);
        }

        public IReadOnlyDictionary<string, Dictionary<string, string?>> IGMenuVersions => FastFlagManager.IGMenuVersions;

        public string SelectedIGMenuVersion
        {
            get
            {
                // yeah this kinda sucks
                foreach (var version in IGMenuVersions)
                {
                    bool flagsMatch = true;

                    foreach (var flag in version.Value)
                    {
                        foreach (var presetFlag in FastFlagManager.PresetFlags.Where(x => x.Key.StartsWith($"UI.Menu.Style.{flag.Key}")))
                        { 
                            if (App.FastFlags.GetValue(presetFlag.Value) != flag.Value)
                                flagsMatch = false;
                        }
                    }

                    if (flagsMatch)
                        return version.Key;
                }

                return IGMenuVersions.First().Key;
            }

            set
            {
                foreach (var flag in IGMenuVersions[value])
                    App.FastFlags.SetPreset($"UI.Menu.Style.{flag.Key}", flag.Value);
            }
        }

        public IReadOnlyDictionary<string, string> LightingModes => FastFlagManager.LightingModes;

        public string SelectedLightingMode
        {
            get => App.FastFlags.GetPresetEnum(LightingModes, "Rendering.Lighting", "True");
            set => App.FastFlags.SetPresetEnum("Rendering.Lighting", LightingModes[value], "True");
        }

        public IReadOnlyDictionary<TextureQuality, string?> TextureQualities => FastFlagManager.TextureQualityLevels;

        public TextureQuality SelectedTextureQuality
        {
            get => TextureQualities.Where(x => x.Value == App.FastFlags.GetPreset("Rendering.TextureQuality.Level")).FirstOrDefault().Key;
            set
            {
                if (value == TextureQuality.Default)
                {
                    App.FastFlags.SetPreset("Rendering.TextureQuality", null);
                }
                else
                {
                    App.FastFlags.SetPreset("Rendering.TextureQuality.OverrideEnabled", "True");
                    App.FastFlags.SetPreset("Rendering.TextureQuality.Level", TextureQualities[value]);
                }
            }
        }

        public bool GuiHidingEnabled
        {
            get => App.FastFlags.GetPreset("UI.Hide") == "32380007";
            set => App.FastFlags.SetPreset("UI.Hide", value ? "32380007" : null);
        }

        public bool PlayerShadowEnabled
        {
            get => App.FastFlags.GetPreset("Rendering.ShadowIntensity") == "True";
            set => App.FastFlags.SetPreset("Rendering.ShadowIntensity", value ? "True" : null);
        }

        public bool PostFXEnabled
        {
            get => App.FastFlags.GetPreset("Rendering.DisablePostFX") == "False";
            set => App.FastFlags.SetPreset("Rendering.DisablePostFX", value ? "False" : null);
        }

        public bool DisableTerrainTextures
        {
            get => App.FastFlags.GetPreset("Rendering.TerrainTextureQuality") == "0";
            set => App.FastFlags.SetPreset("Rendering.TerrainTextureQuality", value ? "0" : null);
        }
    }
}
