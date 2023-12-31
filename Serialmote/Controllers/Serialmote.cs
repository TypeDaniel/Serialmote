using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RJCP.IO.Ports;

namespace Serialmote.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SerialmoteController : BackgroundService
    {
        private static SerialPortStream? _serialPort;
        private readonly ILogger<SerialmoteController> _logger;
        private readonly SerialSettings _serialSettings;

        public SerialmoteController(ILogger<SerialmoteController> logger, SerialSettings serialSettings)
        {
            _logger = logger;
            _serialSettings = serialSettings;

            if (_serialPort == null || !_serialPort.IsOpen)
            {
                
                _serialPort = new SerialPortStream(
                    _serialSettings.PortName,
                    9600, 
                    _serialSettings.DataBits,
                    _serialSettings.Parity,
                    _serialSettings.StopBits);

                _serialPort.Open();
                _serialPort.Close();

                if (!_serialPort.IsOpen)
                {
                    _serialPort = new SerialPortStream(
                        _serialSettings.PortName,
                        115200, 
                        _serialSettings.DataBits,
                        _serialSettings.Parity,
                        _serialSettings.StopBits);

                    _serialPort.Open();
                }
            }
        }



        [HttpGet("{input}/{output}", Name = "Controller")]
        public string Get(string input, string output)
        {
            byte[] hexData;

            if (input == "1" && output == "1")
            {
                hexData = new byte[] { 0x63, 0x69, 0x72, 0x20, 0x30, 0x30, 0x0D, 0x0A };
            }
            else if (input == "2" && output == "1")
            {
                hexData = new byte[] { 0x63, 0x69, 0x72, 0x20, 0x30, 0x31, 0x0D, 0x0A };
            }
            else if (input == "3" && output == "1")
            {
                hexData = new byte[] { 0x63, 0x69, 0x72, 0x20, 0x30, 0x32, 0x0D, 0x0A };
            }
            else if (input == "4" && output == "1")
            {  
                hexData = new byte[] { 0x63, 0x69, 0x72, 0x20, 0x30, 0x33, 0x0D, 0x0A };
            }
            else if (input == "1" && output == "2")
            {
                hexData = new byte[] { 0x63, 0x69, 0x72, 0x20, 0x30, 0x38, 0x0D, 0x0A };
            }
            else if (input == "2" && output == "2")
            {
                hexData = new byte[] { 0x63, 0x69, 0x72, 0x20, 0x30, 0x39, 0x0D, 0x0A };
            }
            else if (input == "3" && output == "2")
            {
                hexData = new byte[] { 0x63, 0x69, 0x72, 0x20, 0x30, 0x61, 0x0D, 0x0A };
            }
            else if (input == "4" && output == "2")
            {
                hexData = new byte[] { 0x63, 0x69, 0x72, 0x20, 0x30, 0x62, 0x0D, 0x0A };
            }
            else if (input == "1" && output == "3")
            {
                hexData = new byte[] { 0x63, 0x69, 0x72, 0x20, 0x31, 0x30, 0x0D, 0x0A };
            }
            else if (input == "2" && output == "3")
            {
                hexData = new byte[] { 0x63, 0x69, 0x72, 0x20, 0x31, 0x31, 0x0D, 0x0A };
            }
            else if (input == "3" && output == "3")
            {
                hexData = new byte[] { 0x63, 0x69, 0x72, 0x20, 0x31, 0x32, 0x0D, 0x0A };
            }
            else if (input == "4" && output == "3")
            {
                hexData = new byte[] { 0x63, 0x69, 0x72, 0x20, 0x31, 0x33, 0x0D, 0x0A };
            }
            else if (input == "1" && output == "4")
            {
                hexData = new byte[] { 0x63, 0x69, 0x72, 0x20, 0x31, 0x38, 0x0D, 0x0A };
            }
            else if (input == "2" && output == "4")
            {
                hexData = new byte[] { 0x63, 0x69, 0x72, 0x20, 0x31, 0x39, 0x0D, 0x0A };
            }
            else if (input == "3" && output == "4")
            {
                hexData = new byte[] { 0x63, 0x69, 0x72, 0x20, 0x31, 0x61, 0x0D, 0x0A };
            }
            else if (input == "4" && output == "4")
            {
                hexData = new byte[] { 0x63, 0x69, 0x72, 0x20, 0x31, 0x62, 0x0D, 0x0A };
            }




            else
            {
                return "Invalid input or output.";
            }



            // Send the hex data to the serial port
            _serialPort?.Write(hexData, 0, hexData.Length);

            // Send a response to the client indicating success
            string response = $"Input {input} Output {output} selected.";

            return response;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {


                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            }
        }
    }
}