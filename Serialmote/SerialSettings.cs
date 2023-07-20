using RJCP.IO.Ports;

namespace Serialmote.Controllers
{
    public class SerialSettings
    {
        public string PortName { get; set; } = "COM1";
        public int DataBits { get; set; }
        public Parity Parity { get; set; }
        public StopBits StopBits { get; set; }
    }
}