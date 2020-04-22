using System;
using System.Collections.Generic;
using System.Text;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using MicrowaveOvenClasses.Boundary;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    [TestFixture]
    class IT_1B_Buttons
    {
        private UserInterface _uut;
        private Button _powerButton;
        private Button _timeButton;
        private Button _startCancelButton;
        private Door _door;
        private IDisplay _display;
        private ILight _light;
        private ICookController _cookController;

        [SetUp]
        public void Setup()
        {
            _powerButton = new Button();
            _timeButton = new Button();
            _startCancelButton = new Button();
            _door = new Door();
            _display = Substitute.For<IDisplay>();
            _light = Substitute.For<ILight>();
            _cookController = Substitute.For<ICookController>();
            _uut = new UserInterface(
                _powerButton,
                _timeButton,
                _startCancelButton,
                _door,
                _display,
                _light,
                _cookController);

        }

        [TestCase(1, 50)]
        [TestCase(2, 100)]
        [TestCase(4, 200)]
        [TestCase(14, 700)]
        [TestCase(15, 700)]
        public void sut_Power_Pressed_Multiple_Times_test(int times, int result)
        {
            for (int i = 0; i < times; i++)
            {
                _powerButton.Press();
            }
            _display.Received(1).ShowPower(result);
        }
        [TestCase(1, 1)]
        [TestCase(15, 15)]
        [TestCase(30, 30)]
        [TestCase(200, 200)]
        public void sut_Time_Pressed_test(int times, int result)
        {
            _powerButton.Press();
            for (int i = 0; i < times; i++)
            {
                _timeButton.Press();
            }
            _display.Received().ShowTime(result, 0);
        }

        [Test]
        public void sut_Start_test()
        {
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            _light.Received(1).TurnOn();
        }

        [Test]
        public void sut_Stop_test()
        {
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            _startCancelButton.Press();
            _light.Received(1).TurnOff();
        }

        [TestCase(4, 10)]
        [TestCase(12, 50)]
        [TestCase(14, 100)]
        public void sut_Start_Multiple_Power_Time_test(int PowerPress, int TimePress)
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
        public void sut_Start_Cooking_called_test(int PowerPress, int TimePress, int PowerLevel, int Time)
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
            _cookController.Received(1).StartCooking(PowerLevel, Time);

        }
        [Test]
        public void sut_StartCancelButton_Not_Clear_test()
        {
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            _display.Received(0).Clear();
        }

        [Test]
        public void sut_StartCancelButton_Clear_test()
        {
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            _startCancelButton.Press();
            _display.Received(1).Clear();
        }
    }
}
