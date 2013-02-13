using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO.Ports;
using System.Timers;
using System.Text.RegularExpressions;

namespace SmartClasses.RFID
{
    public class RFIDReader
    {
        public class ValueChangedEventargs : EventArgs
        {
            public enum ECardType { None = 0, EMMarine = 1 }

            private string result;
            private string code;
            private string id;
            private ECardType cardType;
            private string hash;

            internal bool IsValid;
            internal string Hash
            {
                get
                {
                    return hash;
                }
            }

            public string Result 
            { 
                get
                {
                    return result;
                } 
                internal set
                {
                    //"Em-Marine[4C00] 204,52494\r"

                    result = value;
                    byte[] input = Encoding.UTF8.GetBytes(result);
                    byte[] output = MD5.Create().ComputeHash(input);
                    hash = Convert.ToBase64String(output);

                    string cardTypePatern = "Em-Marine*";
                    if (Regex.IsMatch(result, cardTypePatern))
                    {
                        cardType = ECardType.EMMarine;
                        code = result.Substring(cardTypePatern.Length, 4);
                        id = result.Substring(result.IndexOf(' '), 10);
                        IsValid = Regex.IsMatch(code, "[A-Z]{1}[0-9]{3}") && Regex.IsMatch(code, "[0-9]{3},[0-9]{5}");
                    };

                    
                } 
            }
            public string Code 
            {
                get { return code; } 
            }
            public string ID
            {
                get { return id; }
            }
            public Exception Error { get; internal set; }
            public ECardType CardType
            {
                get { return cardType; }
            }

            public ValueChangedEventargs()
            {
                cardType = ECardType.None;
                IsValid = false;
            }

        }

        public event EventHandler<ValueChangedEventargs> ValueChanged;

        private string portName;
        private Timer scanner;
        private ValueChangedEventargs lastEventResult;

        public string Port 
        {
            get
            {
                return portName;
            }
            set
            {
                var searchingPort = SerialPort.GetPortNames().Where(x => String.Compare(value, x) == 0);
                if (searchingPort.Count() > 0)
                {
                    portName = value;
                }
                else
                {
                    throw new Exception(String.Format("Port '{0}' was not found!", value));
                };
            }
        }

        public RFIDReader()
        {
            scanner = new Timer()
            {
                Enabled = false,
                Interval = 100
            };
            scanner.Elapsed += scanner_Elapsed;
        }
        public RFIDReader(string PortName) : this()
        {
            Port = PortName;
        }

        public void Start()
        {
            scanner.Start();
        }
        public void Stop()
        {
            scanner.Stop();
        }
        protected void scanner_Elapsed(object sender, ElapsedEventArgs e)
        {
            scanner.Stop();
            ReadRFID();
            scanner.Start();
        }

        private void ReadRFID()
        {
            ValueChangedEventargs eventResult = new ValueChangedEventargs();
            using (SerialPort rfidReader = new SerialPort(portName, 9600, Parity.None, 8, StopBits.One))
            {
                rfidReader.ReadTimeout = 3000;
                try
                {
                    rfidReader.Close();
                    rfidReader.Open();
                    eventResult.Result = rfidReader.ReadLine();
                }
                catch(Exception e)
                {
                    if (!(e is TimeoutException))
                    {
                        eventResult.Error = e;
                    };
                }
                finally
                {
                    rfidReader.Close();
                };
            };
            if (lastEventResult == null)
            {
                lastEventResult = eventResult;
            };
            if (lastEventResult.Hash != eventResult.Hash && eventResult.CardType != ValueChangedEventargs.ECardType.None)
            {
                lastEventResult = eventResult;    
                OnValueChanged(eventResult);
            };
            
        }

        protected virtual void OnValueChanged(ValueChangedEventargs e)
        {
            if (ValueChanged != null)
            {
                ValueChanged(this, e);
            };
        }

    }
}
