﻿using AtgDev.Voicemeeter;
using SuchByte.MacroDeck.Logging;
using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace PW.MacroDeck.VoicemeeterPlugin.Services.Voicemeeter
{
    internal static class VoicemeeterControlHelpers
    {
        public static string ErrorStr = "Communication Error";

        public static void TestLogin(int loginResult, Action onSuccessfulLogin = null)
        {
            switch (loginResult)
            {
                case ResultCodes.Ok:
                    onSuccessfulLogin?.Invoke();
                    break;
                case ResultCodes.OkVmNotLaunched:
                    break;
                case ResultCodes.Error:
                    throw new Exception("Not installed or could not connect.");
                default:
                    throw new Exception("Unexpected connection. Connection was not correctly closed previously.");
            }
        }

        // 0: OK(no error).
        // -1: error
        // -2: no server.
        // -3: no level available
        // -4: out of range
        public static void TestLevelResult(int result)
        {
            switch (result)
            {
                case ResultCodes.Ok: break;
                case ResultCodes.Error: throw new Exception("Error");
                case ResultCodes.NoServer: throw new Exception("Not Connected");
                case ResultCodes.NoLevelAvailable: break;
                case -4: throw new ArgumentException("Channel out of range");
                default: throw UnknownError(result);
            }
        }

        //0: OK(no error).
        //-1: error
        //-2: no server.
        //-3: unknown parameter
        //-4: structure mismatch
        public static void TestResult(int result, [CallerMemberName] string callerName = "")
        {
            try
            {
                TestResultThrow(result);
            }
            catch (Exception ex)
            {
                MacroDeckLogger.Warning(PluginInstance.Plugin, typeof(Control), $"{callerName}: {ex.Message}");
            }
        }

        //0: OK(no error).
        //-1: error
        //-2: no server.
        //-3: unknown parameter
        //-4: structure mismatch
        internal static void TestResultThrow(int result)
        {
            switch (result)
            {
                case ResultCodes.Ok: break;
                case ResultCodes.Error: throw new Exception("Parameter Error");
                case ResultCodes.NoServer: throw new Exception("Not Connected");
                case ResultCodes.UnexpectedError1: throw new ArgumentException("Parameter not found");
                case ResultCodes.UnexpectedError2: throw new Exception("Structure mismatch");
                default: throw UnknownError(result);
            }
        }

        private static Exception UnknownError(int result) => new Exception($"Unknown ({result})");
    }
}