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
    public class Serialmote : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<Serialmote> _logger;
        private static SerialPortStream? _serialPort;

        public Serialmote(ILogger<Serialmote> logger)
        {
            if (_serialPort == null || !_serialPort.IsOpen)
            {
                _serialPort = new SerialPortStream("COM4", 9600, 8, Parity.None, StopBits.One);
                _serialPort.Open();
                _serialPort.Close();

                if (!_serialPort.IsOpen)
                {
                    _serialPort = new SerialPortStream("COM4", 115200, 8, Parity.None, StopBits.One);
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
            _serialPort.Write(hexData, 0, hexData.Length);

            // Send a response to the client indicating success
            string response = $"Input {input} Output {output} selected.";

            return response;
        }

        [HttpGet("Logs")]
        public async Task Logs()
        {
            // Set the response headers for SSE
            Response.ContentType = "text/event-stream";
            Response.Headers.Add("Cache-Control", "no-cache");
            Response.Headers.Add("Connection", "keep-alive");

            // Continuously read data from the COM port and send it to the client as SSE events
            using (var writer = new StreamWriter(Response.Body, Encoding.UTF8, 4096, true))
            {
                while (true)
                {
                    byte[] buffer = new byte[_serialPort.ReadBufferSize];
                    int bytesRead = await _serialPort.ReadAsync(buffer, 0, buffer.Length);
                    string responseData = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                    // Add timestamp to the data
                    string timestampedData = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} {responseData}";

                    // Send the data as an SSE event
                    await writer.WriteLineAsync($"data: {timestampedData}\n");
                    await writer.FlushAsync();
                }
            }
        }







    }
}
