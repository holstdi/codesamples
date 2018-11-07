// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;

namespace BrownBagContextSpec
{
    public abstract class BaseSpecification
    {
        protected Exception exception_caught;

        /// <summary>
        ///     The main setup
        /// </summary>
        [OneTimeSetUp]
        public virtual void BaseSetup()
        {
            Establish_context();
            Because();
        }

        /// <summary>
        ///     The main teardown
        /// </summary>
        [OneTimeTearDown]
        public virtual void BaseTeardown()
        {
            Dispose_context();
        }

        /// <summary>
        ///     Establish the contexts where every test is running in
        /// </summary>
        protected virtual void Establish_context()
        {
        }

        /// <summary>
        ///     Perform here the action on the sut to be tested
        /// </summary>
        protected virtual void Because()
        {
        }

        /// <summary>
        ///     Dispose the local context of the test.
        /// </summary>
        protected virtual void Dispose_context()
        {
        }

        protected void Trap<TException>(Action action) where TException : Exception
        {
            try
            {
                action();
            }
            catch (Exception exception)
            {
                if (exception as TException == null)
                    throw;

                exception_caught = exception;
                Console.WriteLine("Trapped Exception: {0}", exception);
            }
        }
    }

    /// <summary>
    ///     Base class for test fixtures
    /// </summary>
    public abstract class BaseSpecification<TSubject> : BaseSpecification where TSubject : class
    {

        /// <summary>
        ///     Gets the SUT.
        /// </summary>
        /// <value>
        ///     The SUT.
        /// </value>
        protected TSubject SUT { get; set; }

        /// <summary>
        ///     The main setup
        /// </summary>
        public override void BaseSetup()
        {
            Establish_context();
            SUT = Create_subject_under_test();
            Because();
        }

        /// <summary>
        ///     CreateMessageGroup subject under tests
        /// </summary>
        /// <returns />
        protected abstract TSubject Create_subject_under_test();
    }


    public abstract class ContextSpecification<TSubject> : BaseSpecification<TSubject> where TSubject : class
    {
        protected readonly List<object> _dependencies = new List<object>();

        /// <summary>
        ///     The main setup
        /// </summary>
        public override void BaseSetup()
        {
            Establish_context();
            SUT = Create_subject_under_test();
            Because();
        }

        /// <summary>
        ///     CreateMessageGroup subject under tests
        /// </summary>
        /// <returns />
        protected override TSubject Create_subject_under_test()
        {
            var ctors = typeof (TSubject).GetConstructors();
            var mostGreedyCtor = ctors.OrderByDescending(ctor => ctor.GetParameters().Length).First();
            var @params = (from param in mostGreedyCtor.GetParameters() select param.ParameterType into type let configuredDependency = _dependencies.FirstOrDefault(type.IsInstanceOfType) select configuredDependency ?? MockRepository.GenerateStub(type)).ToArray();
            return (TSubject) mostGreedyCtor.Invoke(@params);
        }

        /// <summary>
        ///     GetSubCategory a dependency (mock) on the SUT of the specified type
        /// </summary>
        /// <remarks>
        ///     A mock is an object that we can set expectations on, and which will verify that the expected actions have indeed occurred.
        ///     If you want to verify the behavior of the code under test, you will use a mock with the appropriate expectation, and verify that.
        /// </remarks>
        /// <typeparam name="TMock">The type of the mock.</typeparam>
        /// <returns />
        protected TMock Dependency<TMock>() where TMock : class
        {
            var dependency = MockRepository.GenerateMock<TMock>();
            _dependencies.Add(dependency);
            return dependency;
        }

        /// <summary>
        ///     GetSubCategory a stub of the specified type
        /// </summary>
        /// <remarks>
        ///     If you want just to pass a value that may need to act in a certain way,
        ///     but isn't the focus of this test, you will use a stub. A stub's properties will automatically behave
        ///     like normal properties, and you can't set expectations on them.
        ///     IMPORTANT: A stub will never cause a test to fail.
        /// </remarks>
        /// <typeparam name="TStub">The type of the stub.</typeparam>
        /// <returns />
        protected TStub DependencyAsStub<TStub>() where TStub : class
        {
            var dependency = MockRepository.GenerateStub<TStub>();
            _dependencies.Add(dependency);
            return dependency;
        }

        /// <summary>
        ///     CreateMessageGroup a dependency for the SUT and register it in the container
        /// </summary>
        /// <typeparam name="TMock">The type of the mock.</typeparam>
        /// <returns />
        protected TMock DependencyAsPartial<TMock>(params object[] parameters) where TMock : class
        {
            var dependency = MockRepository.GeneratePartialMock<TMock>();
            _dependencies.Add(dependency);
            return dependency;
        }
    }

    public abstract class StaticSpecification : BaseSpecification
    {
    }
}




