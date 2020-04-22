using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class IT_1A_Door
    {
        private UserInterface _userInterface;
        private Door _uut_door;

        private ILight _light;
        private IDisplay _display;
        private IButton _start, _power, _time;
        private ICookController _cooker;

        [SetUp]
        public void Setup()
        {
            _light = Substitute.For<ILight>();
            _start = Substitute.For<IButton>();
            _power = Substitute.For<IButton>();
            _time = Substitute.For<IButton>();
            _display = Substitute.For<IDisplay>();
            _cooker = Substitute.For<ICookController>();

            _uut_door = new Door();
            _userInterface = new UserInterface(_power, _time, _start, _uut_door, _display, _light, _cooker);
        }

        [Test]
        public void blabla()
        {
        }
    }
}
