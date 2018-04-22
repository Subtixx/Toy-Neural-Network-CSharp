using System.Diagnostics;

namespace DoodleClassification
{
    [DebuggerDisplay("{" + nameof(Label) + "}")]
    public struct Doodle
    {
        public enum DoodleLabel
        {
            Cat = 0,
            Rainbow = 1,
            Train = 2
        }

        public byte[] Data;
        public DoodleLabel Label;
    }
}