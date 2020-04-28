using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class IT_3C_UI_CC_PowerTube
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

        private ITimer _timer;
        private IOutput _output;

        [SetUp]
        public void Setup()
        {
            _output = Substitute.For<IOutput>();
            _timer = Substitute.For<ITimer>();
            _powerButton = new Button();
            _timeButton = new Button();
            _startCancelButton = new Button();
            _door = new Door();
            
            _light = new Light(_output);


            _display = new Display(_output);

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
        }

        [TestCase(7, 1)]
        [TestCase(21, 3)]
        public void TurnOn_test(int power, int Powerpress)
        {

            for (int i = 0; i < Powerpress; i++)
            {
                _powerButton.Press();
            }

            _timeButton.Press();
            _startCancelButton.Press();

            _output.Received(1).OutputLine($"PowerTube works with {power}");
        }

        [Test]
        public void TurnOff_DoorOpen_test()
        {
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            _door.Open();

            _output.Received(1).OutputLine($"PowerTube turned off");
        }

        [Test]
        public void TurnOff_CancelPressed_test()
        {
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            _startCancelButton.Press();

            _output.Received(1).OutputLine($"PowerTube turned off");
        }


    }
}
