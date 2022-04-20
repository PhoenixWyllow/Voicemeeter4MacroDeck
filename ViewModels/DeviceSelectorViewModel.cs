﻿using PW.VoicemeeterPlugin.Models;
using PW.VoicemeeterPlugin.Services.Voicemeeter;
using SuchByte.MacroDeck.Logging;
using SuchByte.MacroDeck.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PW.VoicemeeterPlugin.ViewModels
{
    public abstract class DeviceSelectorViewModel : ISavableConfigViewModel
    {
        private readonly PluginAction _action;

        private readonly DeviceConfigModel configuration;

        ISerializableConfiguration ISavableConfigViewModel.SerializableConfiguration => configuration;

        protected DeviceSelectorViewModel(PluginAction action)
        {
            _action = action;
            configuration = DeviceConfigModel.Deserialize(action.Configuration);

            SelectedDevice = AvailableDevices.FirstOrDefault(d => d.Id.Equals(configuration.Option.Id));
            AvailableActions = GetAvailableActionsForDevice(SelectedDevice);
            ChangeAction(configuration.Action);
        }

        public string[] AvailableActions { get; private set; }
        public IEnumerable<VmIOInfo> AvailableDevices { get; } = AvailableValues.IOInfo;
        public VmIOInfo SelectedDevice { get; private set; }
        public string SelectedAction { get; private set; }

        public void ChangeDevice(string selectedDevice)
        {
            SelectedDevice = AvailableDevices.FirstOrDefault(device => device.Name.Equals(selectedDevice));
            AvailableActions = GetAvailableActionsForDevice(SelectedDevice);
        }

        protected abstract string[] GetAvailableActionsForDevice(VmIOInfo device);

        public void ChangeAction(string selectedAction)
        {
            SelectedAction = selectedAction;
        }

        public void SaveConfig()
        {
            try
            {
                SetConfig();
                MacroDeckLogger.Info(PluginInstance.Plugin, $"{GetType().Name}: config saved");
            }
            catch (Exception ex)
            {
                MacroDeckLogger.Error(PluginInstance.Plugin, $"{GetType().Name}: config NOT saved");
                MacroDeckLogger.Error(PluginInstance.Plugin, $"{GetType().Name}: {ex.Message}");
            }
        }

        public void SetConfig()
        {
            configuration.Name = SelectedDevice.Name;
            configuration.Action = SelectedAction;
            configuration.Option = AvailableValues.Options.Find(option => option.Option.Equals(SelectedAction) && option.Id.Equals(SelectedDevice.Id));

            _action.ConfigurationSummary = configuration.ToString();
            _action.Configuration = configuration.Serialize();
        }
    }
}
