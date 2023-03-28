using System;
using System.Diagnostics;
using System.IO;

namespace Blanketmen.Hypnos
{
    public enum LogType
    {
        Normal = 0,
        Warning = 1,
        Error = 2
    }

    [Flags]
    public enum LogChannel
    {
        None = 0,
        Environment = 1,
        Resource = Environment << 1,
        Network = Resource << 1,
        Input = Network << 1,
        GameTime = Input << 1,
        GameState = GameTime << 1,
        UI = GameState << 1,
        Audio = UI << 1,
        All = -1
    }

    public sealed partial class Kernel
    {
        public const int AllLogChannel = -1;
        public const string TraceLogCondition = "TRACE_LOG";
        public const string DebugLogCondition = "DEBUG_LOG";

        public static event Action<string, LogType> OnLogReceived;

        public static int TraceLogChannel = AllLogChannel;
        public static int DebugLogChannel = AllLogChannel;

        public static void SetLogWriter(StreamWriter sw)
        {
            if (sw == null)
            {
                return;
            }

            sw.AutoFlush = true;
            Console.SetOut(sw);
        }

        [Conditional(DebugLogCondition)]
        public static void Log(string message, int channel = AllLogChannel)
        {
            if ((channel & DebugLogChannel) == 0)
            {
                return;
            }

            Console.Out.WriteLine(message);
            OnLogReceived?.Invoke(message, LogType.Normal);
        }

        [Conditional(DebugLogCondition)]
        public static void LogWarning(string message, int channel = AllLogChannel)
        {
            if ((channel & DebugLogChannel) == 0)
            {
                return;
            }

            Console.Out.WriteLine(message);
            OnLogReceived?.Invoke(message, LogType.Warning);
        }

        [Conditional(DebugLogCondition)]
        public static void LogError(string message)
        {
            Console.Out.WriteLine(message);
            OnLogReceived?.Invoke(message, LogType.Error);
        }

        [Conditional(DebugLogCondition)]
        public static void Assert(bool condition, string message, int channel = AllLogChannel)
        {
            if ((channel & DebugLogChannel) == 0)
            {
                return;
            }

            Console.Out.WriteLine(message);
            OnLogReceived?.Invoke(message, LogType.Error);
        }
    }
}