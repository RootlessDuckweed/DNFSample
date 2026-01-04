using System.Globalization;
using FixMath;

namespace SkillSystem.Config
{
    public class LogicFrameConfig
    {
        public static long LogicFrameId = 0;
        public const float LogicFrameInterval = 0.066f;
        public const int LogicFrameIntervalMs = 66;
        public static readonly FixInt LogicFrameIntervalFix = 68L;
        public static readonly FixInt LogicFrameIntervalMsFix = 67584L;
    }
}