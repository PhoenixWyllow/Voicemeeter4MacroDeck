﻿using PW.VoicemeeterPlugin.Models;
using PW.VoicemeeterPlugin.Services.Voicemeeter;
using SuchByte.MacroDeck.Logging;
using SuchByte.MacroDeck.Plugins;
using SuchByte.MacroDeck.Variables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PW.VoicemeeterPlugin.ViewModels
{
    internal class DeviceToggleViewModel : DeviceSelectorViewModel
    {
        public DeviceToggleViewModel(PluginAction action) : base(action)
        {
        }

        protected override string[] GetAvailableActionsForDevice(VmIOInfo device)
        {
            return AvailableValues.Options.Where(opt => opt.Id.Equals(device.Id) && opt.Type == VariableType.Bool)
                                          .Select(opt => opt.Option)
                                          .ToArray();
            //return device.Type switch
            //{
            //    VmIOType.Strip => AvailableValues.StripToggles.ToArray(),
            //    VmIOType.Bus => AvailableValues.BusToggles.ToArray(),
            //    _ => Array.Empty<string>(),
            //};   
        }
    }
}