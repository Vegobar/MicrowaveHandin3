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

namespace Microwave.Test.Integration
{
    [TestFixture]
    class IT_3_Timer
    {
        private UserInterface _userInterface;
        private Button _powerButton;
        private Button _timeButton;
        private Button _startCancelButton;
        private Door _door;
        private CookController _cookController;

        private IDisplay _display;
        private ILight _light;
        private Timer _timer;
        private IPowerTube _powerTube;

        [SetUp]
        public void Setup()
        {
            _powerButton = new Button();
            _timeButton = new Button();
            _startCancelButton = new Button();
            _door = new Door();


            _display = Substitute.For<IDisplay>();
            _light = Substitute.For<ILight>();
            _timer = new Timer();
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

        [TestCase]
        public void TimeeIsUP()
        {
            //act
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();

            System.Threading.Thread.Sleep(60000);

            _powerTube.Received(1).TurnOff();

        }


        [TestCase]
        public void TikTok_Time()
        {

        }


    }
}
