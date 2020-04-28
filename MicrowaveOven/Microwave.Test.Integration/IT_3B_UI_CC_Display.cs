using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* The interface between Cook Controller and display is tested with the method Showtime
 * The interface between UI and display is tested with the method ShowPower 
 */


namespace Microwave.Test.Integration
{
    [TestFixture]
    public class IT_3B_UI_CC_Display
    {
        private UserInterface _userInterface;
        private Button _powerButton;
        private Button _timeButton;
        private Button _startCancelButton;
        private Door _door;
        private CookController _cookController;

        private Display _display;
        private Light _light;
        private ITimer _timer;
        private IPowerTube _powerTube;
        private IOutput _output;

        [SetUp]
        public void Setup()
        {
            _powerButton = new Button();
            _timeButton = new Button();
            _startCancelButton = new Button();
            _door = new Door();
            _output = Substitute.For<IOutput>();
            _light = new Light(_output);


            _display = new Display(_output);

            _timer = Substitute.For<ITimer>();
            _powerTube = Substitute.For<IPowerTube>();

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


        [TestCase(1,1,0)]
        [TestCase(3,3,0)]
        [TestCase(25,25,0)]
        public void ShowTime(int TimePress, int min, int sec)
        {
            //act
            _powerButton.Press();

            for (int i = 0; i < TimePress; i++)
            {
                _timeButton.Press();
            }

            _output.Received(1).OutputLine($"Display shows: {min:D2}:{sec:D2}");
         }

        [TestCase(1,50)]
        [TestCase(3,150)]
        public void ShowPower(int Powerpress, int power)
        {
            _timeButton.Press();

            for (int i = 0; i < Powerpress; i++)
            {
                _powerButton.Press();
            }

            _output.Received(1).OutputLine($"Display shows: {power} W");
        }
        
        [TestCase(15,50)]
        [TestCase(17,150)]
        public void ShowPower_reset(int Powerpress, int power)
        {
            _timeButton.Press();

            for (int i = 0; i < Powerpress; i++)
            {
                _powerButton.Press();
            }

            _output.Received(2).OutputLine($"Display shows: {power} W");
        }

        [Test]
        public void ShowClear()
        {
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            _startCancelButton.Press();
        }


    }
}

