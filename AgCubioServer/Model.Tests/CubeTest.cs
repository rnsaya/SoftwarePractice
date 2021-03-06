// <copyright file="CubeTest.cs">Copyright ©  2015</copyright>
using System;
using System.Drawing;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;

namespace Model.Tests
{
    /// <summary>This class contains parameterized unit tests for Cube</summary>
    [PexClass(typeof(Cube))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class CubeTest
    {
        /// <summary>Test stub for GetRect()</summary>
        [PexMethod]
        public Rectangle GetRectTest([PexAssumeUnderTest]Cube target)
        {
            Rectangle result = target.GetRect();
            return result;
            // TODO: add assertions to method CubeTest.GetRectTest(Cube)
        }
    }
}
