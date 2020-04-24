using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class IT_2_Cookcontroller
    {
        private UserInterface _userInterface;
        private Button _powerButton;
        private Button _timeButton;
        private Button _startCancelButton;
        private Door _door;
        private CookController _cookController;

        private IDisplay _display;
        private ILight _light;
        private ITimer _timer;
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

        [TestCase]
        public void StartCooking_called()
        {
            //act
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();


            //Assert
            _powerTube.Received(1).TurnOn(50);
        }


        [TestCase]
        public void CancelCooking_called()
        {
            //Act
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            _startCancelButton.Press();


            //Assert
            _powerTube.Received(1).TurnOff();
        }


        [TestCase]
        public void Cooking_TimerCalled()
        {
            //Act
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();

            //Assert
            _timer.Received(1).Start(60);

        }

        [TestCase(4, 10)]
        [TestCase(12, 50)]
        [TestCase(14, 100)]
        public void ButtonPushedMultipleTimes(int PowerPress, int TimePress)
        {
            for (int i = 0; i < PowerPress; i++)
            {
                _powerButton.Press();
            }

            for (int i = 0; i < TimePress; i++)
            {
                _timeButton.Press();
            }
            _startCancelButton.Press();
            _light.Received(1).TurnOn();
        }


        [TestCase(2, 10)]
        [TestCase(5, 30)]
        [TestCase(6, 20)]
        public void sut_Stop_Multiple_Power_Time_test(int PowerPress, int TimePress)
        {
            for (int i = 0; i < PowerPress; i++)
            {
                _powerButton.Press();
            }

            for (int i = 0; i < TimePress; i++)
            {
                _timeButton.Press();
            }
            _startCancelButton.Press();
            _startCancelButton.Press();
            _light.Received(1).TurnOff();
        }



        [TestCase(4, 10, 200, 600)]
        [TestCase(10, 20, 500, 1200)]
        [TestCase(14, 30, 700, 1800)]
        public void Start_Cooking_called(int PowerPress, int TimePress, int PowerLevel, int Time)
        {
            for (int i = 0; i < PowerPress; i++)
            {
                _powerButton.Press();
            }

            for (int i = 0; i < TimePress; i++)
            {
                _timeButton.Press();
            }
            _startCancelButton.Press();
            _powerTube.Received(1).TurnOn(PowerLevel);
        }

        [TestCase(4, 10, 200, 600)]
        [TestCase(10, 20, 500, 1200)]
        [TestCase(14, 30, 700, 1800)]
        public void Start_Cooking_TimerReceived(int PowerPress, int TimePress, int PowerLevel, int Time)
        {
            for (int i = 0; i < PowerPress; i++)
            {
                _powerButton.Press();
            }

            for (int i = 0; i < TimePress; i++)
            {
                _timeButton.Press();
            }
            _startCancelButton.Press();
            _timer.Received(1).Start(Time);
        }



        [Test]
        public void StartCancelButton_Not_Clear()
        {
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            _display.Received(0).Clear();
        }


        [Test]
        public void StartCancelButton_Clear()
        {
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            _startCancelButton.Press();
            _display.Received(1).Clear();
        }


        [Test]
        public void DoorOpen_StartCooking()
        {
            _door.Open();
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();

            _powerTube.DidNotReceive().TurnOn(50);

        }
    }
}
