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
    class IT_1B_UI_Buttons
    {
        private UserInterface _ui;
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
            _ui = new UserInterface(
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

  


       
    }
}
