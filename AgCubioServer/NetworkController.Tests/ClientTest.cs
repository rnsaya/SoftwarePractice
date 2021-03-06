// <copyright file="ClientTest.cs">Copyright ©  2015</copyright>
using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetworkController;

namespace NetworkController.Tests
{
    /// <summary>This class contains parameterized unit tests for Client</summary>
    [PexClass(typeof(Client))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class ClientTest
    {
        /// <summary>Test stub for SendMessage(String, Action`1&lt;Boolean&gt;)</summary>
        [PexMethod]
        internal void SendMessageTest(
            [PexAssumeUnderTest]Client target,
            string message,
            Action<bool> afterSend
        )
        {
            target.SendMessage(message, afterSend);
            // TODO: add assertions to method ClientTest.SendMessageTest(Client, String, Action`1<Boolean>)
        }
    }
}
