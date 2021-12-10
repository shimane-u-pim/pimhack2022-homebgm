using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioPlayerHost
{
    internal class ControlApi
    {
        public string? type { get; set; } = null;

        public string? target { get; set; } = null;

        public float volume { get; set; } = float.NaN;

        public string? file { get; set; } = null;
    }
}
