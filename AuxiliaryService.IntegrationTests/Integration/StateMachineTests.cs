using System;
using NUnit.Framework;
using Ninject;
using AuxiliaryService.API.Shared.StateMachine.DTO;
using AuxiliaryService.API.Shared.StateMachine;

namespace AuxiliaryService.IntegrationTests
{
	[TestFixture]
    public class StateMachineTests : AopTransactedTests
    {
        #region private

        #endregion

        #region Setup

        [SetUp]
        public void Setup()
        {
        }

        #endregion

        class SmTestContext : StateMachineBaseContext
        {
            public string Data { get; set; }
        }

        private void DoAction1(SmTestContext context)
        {
            context.Data += "->1";
        }

        private void DoAction2(SmTestContext context)
        {
            context.Data += "->2";
        }

        private void ErrorAction(SmTestContext context)
        {
            throw new Exception("error action");
        }

        [Test]
        public void GoThroghStatesAndPersists()
        {
            var smCfg =
                TestKernel.Get<IStateMachineConfiguration<SmTestContext>>()
                .Create("TestSM", "Test State Machine");

            smCfg
                .Configure("1", "2", isInitial: true)
                .Do(DoAction1);

            smCfg
                .Configure("2", "3", isInitial: false)
                .Do(DoAction2)
                .SavePoint();

            var ctx = new SmTestContext()
            {
                Data = "0",
                RefId = Guid.NewGuid()
            };

            smCfg
                .Instance(ctx)
                .Start();

            Assert.AreEqual(ctx.Data, "0->1->2");

            SetComplete();
        }

        [Test]
        public void Resume()
        {
            var refId = Guid.NewGuid();
            var cfg = TestKernel.Get<IStateMachineConfiguration<SmTestContext>>();

            var smCfg =
                cfg
                .Create("TestSMAndResume", "Test State Machine");

            smCfg.Configure("1", "2", isInitial: true).Do(DoAction1).SavePoint();
            smCfg.Configure("2", "3", isInitial: false).Do(ErrorAction);

            var ctx = new SmTestContext()
            {
                Data = "0",
                RefId = refId
            };

            try
            {
                smCfg.Instance(ctx).Start();
            }
            catch (Exception)
            {
            }

            SetComplete();

            smCfg = cfg.Create("TestSMAndResume", "Test State Machine");

            smCfg.Configure("1", "2", isInitial: true).Do(DoAction1);
            smCfg.Configure("2", "3", isInitial: false).Do(DoAction2);

            smCfg.Instance(ctx).Resume().Start();

            SetComplete();

            Assert.AreEqual(ctx.Data, "0->1->2");
        }

        [Test]
        public void CycledTraverseNowAllowed()
        {
            var cfg = TestKernel.Get<IStateMachineConfiguration<SmTestContext>>();

            var smCfg = cfg.Create("CycleSM", "Test State Machine");

            smCfg.Configure("1", "2", isInitial: true).Do(DoAction1);
            smCfg.Configure("2", "3", isInitial: false).Do(DoAction1);
            smCfg.Configure("3", "1", isInitial: false).Do(DoAction1);

            Assert.Throws<Exception>(() => smCfg.Instance(new SmTestContext() { RefId = Guid.NewGuid() }));
        }
    }
}