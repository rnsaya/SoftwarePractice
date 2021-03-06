// <copyright file="ServerWorldTest.cs">Copyright ©  2015</copyright>
using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;

namespace Model.Tests
{
    /// <summary>This class contains parameterized unit tests for ServerWorld</summary>
    [PexClass(typeof(ServerWorld))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class ServerWorldTest
    {
        /// <summary>Test stub for ServerHeartbeat(Func`2&lt;Int32,Player&gt;)</summary>
        [PexMethod]
        public void ServerHeartbeatTest(
            [PexAssumeUnderTest]ServerWorld target,
            Func<int, Player> playerFunc
        )
        {
            target.ServerHeartbeat(playerFunc);
            // TODO: add assertions to method ServerWorldTest.ServerHeartbeatTest(ServerWorld, Func`2<Int32,Player>)
        }
    }
}
