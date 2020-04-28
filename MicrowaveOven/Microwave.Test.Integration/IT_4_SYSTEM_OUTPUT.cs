using System;
using System.IO;
using System.Threading;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;
using Timer = MicrowaveOvenClasses.Boundary.Timer;

namespace Microwave.Test.Integration
{   [TestFixture]
    public class IT_4_SYSTEM_OUTPUT
    {
        private UserInterface _userInterface;
        private Button _powerButton;
        private Button _timeButton;
        private Button _startCancelButton;
        private Door _door;
        private CookController _cookController;
        private Display _display;
        private Light _light;
        private PowerTube _powerTube;
        private Timer _timer;
        private Output _output;

        private StringWriter _str;

        [SetUp]
        public void Setup()
        {
            _output = new Output();
            
            _powerButton = new Button();
            _timeButton = new Button();
            _startCancelButton = new Button();
            _door = new Door();
            
            _light = new Light(_output);


            _display = new Display(_output);

            
            _timer = new Timer();

            _powerTube = new PowerTube(_output);

            _cookController = new CookController(_timer, _display, _powerTube, _userInterface);

            _userInterface = new UserInterface(
                _powerButton,
                _timeButton,
                _startCancelButton,
                _door,
                _display,
                _light,
                _cookController);

            _str = new StringWriter();
            Console.SetOut(_str);
        }

        //Display -> Output
        [Test]
        public void Output_Timer_test()
        {
            _powerButton.Press();
            _timeButton.Press();
            
            Assert.That(_str.ToString().Contains($"Display shows: 01:00"));
        }

        [Test]
        public void Output_Power_test()
        {
            _powerButton.Press();

            Assert.That(_str.ToString().Contains($"Display shows: 50 W"));
        }

        [Test]
        public void Output_Clear_test()
        {
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            _startCancelButton.Press();

            Assert.That(_str.ToString().Contains($"Display cleared"));
        }

        //Powertube -> Output
        [Test]
        public void Output_PowerOn()
        {
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();

            Assert.That(_str.ToString().Contains($"PowerTube works with 7"));
        }

        [Test]
        public void Output_Turnoff()
        {
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            _startCancelButton.Press();

            Assert.That(_str.ToString().Contains($"PowerTube turned off"));
        }

        //Light -> Output
        [Test]
        public void Output_LightOn()
        {
            _door.Open();

            Assert.That(_str.ToString().Contains("Light is turned on"));
        }

        [Test]
        public void Output_LightOff()
        {
            _door.Open();
            _door.Close();

            Assert.That(_str.ToString().Contains("Light is turned off"));
        }
    }
}