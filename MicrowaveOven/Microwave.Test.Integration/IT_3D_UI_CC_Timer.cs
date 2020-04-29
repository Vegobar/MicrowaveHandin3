using System.Threading;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;
using Timer = MicrowaveOvenClasses.Boundary.Timer;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class IT_3D_UI_CC_Timer
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
        private IOutput _output;

        [SetUp]
        public void Setup()
        {
            _output = Substitute.For<IOutput>();
            
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
        }

        [TestCase(00,58,2100)]
        [TestCase(00,55,5100)]
        [TestCase(00,50,10100)]
        public void Timer_check(int min, int sec, int sleeptime)
        {
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();

            Thread.Sleep(sleeptime);
            
            _output.Received(1).OutputLine($"Display shows: {min:D2}:{sec:D2}");
        }

        [Test]
        public void Timer_Expired()
        {
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();

            Thread.Sleep(61000);

            _output.Received(1).OutputLine($"PowerTube turned off");
        }
    }
}