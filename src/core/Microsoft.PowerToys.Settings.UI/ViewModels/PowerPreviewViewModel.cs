﻿// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.CompilerServices;
using Microsoft.PowerToys.Settings.UI.Helpers;
using Microsoft.PowerToys.Settings.UI.Lib;
using Microsoft.PowerToys.Settings.UI.Views;

namespace Microsoft.PowerToys.Settings.UI.ViewModels
{
    public class PowerPreviewViewModel : Observable
    {
        private const string ModuleName = "File Explorer";

        private PowerPreviewSettings Settings { get; set; }

        public PowerPreviewViewModel()
        {
            try
            {
                Settings = SettingsUtils.GetSettings<PowerPreviewSettings>(ModuleName);
            }
            catch
            {
                Settings = new PowerPreviewSettings();
                SettingsUtils.SaveSettings(Settings.ToJsonString(), ModuleName);
            }

            _svgRenderIsEnabled = Settings.Properties.EnableSvgPreview;
            _svgThumbnailIsEnabled = Settings.Properties.EnableSvgThumbnail;
            _mdRenderIsEnabled = Settings.Properties.EnableMdPreview;
        }

        private bool _svgRenderIsEnabled = false;
        private bool _mdRenderIsEnabled = false;
        private bool _svgThumbnailIsEnabled = false;

        public bool SVGRenderIsEnabled
        {
            get
            {
                return _svgRenderIsEnabled;
            }

            set
            {
                if (value != _svgRenderIsEnabled)
                {
                    _svgRenderIsEnabled = value;
                    Settings.Properties.EnableSvgPreview = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool SVGThumbnailIsEnabled
        {
            get
            {
                return _svgThumbnailIsEnabled;
            }

            set
            {
                if (value != _svgThumbnailIsEnabled)
                {
                    _svgThumbnailIsEnabled = value;
                    Settings.Properties.EnableSvgThumbnail = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool MDRenderIsEnabled
        {
            get
            {
                return _mdRenderIsEnabled;
            }

            set
            {
                if (value != _mdRenderIsEnabled)
                {
                    _mdRenderIsEnabled = value;
                    Settings.Properties.EnableMdPreview = value;
                    RaisePropertyChanged();
                }
            }
        }

        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            // Notify UI of property change
            OnPropertyChanged(propertyName);

            if (ShellPage.DefaultSndMSGCallback != null)
            {
                SndPowerPreviewSettings snd = new SndPowerPreviewSettings(Settings);
                SndModuleSettings<SndPowerPreviewSettings> ipcMessage = new SndModuleSettings<SndPowerPreviewSettings>(snd);
                ShellPage.DefaultSndMSGCallback(ipcMessage.ToJsonString());
            }
        }
    }
}
