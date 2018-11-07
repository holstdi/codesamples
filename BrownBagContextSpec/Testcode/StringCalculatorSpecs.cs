using BrownBagContextSpec.Applicationcode;
using FluentAssertions;
using NUnit.Framework;

// ReSharper disable InconsistentNaming

namespace BrownBagContextSpec.Testcode
{
    public class StringCalculatorSpecs 
    {
        public class concern : ContextSpecification<StringCalculator>
        {
            protected override void Establish_context()
            {
                base.Establish_context();
            }

            protected override StringCalculator Create_subject_under_test()
            {
                return base.Create_subject_under_test();
            }

            protected override void Because()
            {
                base.Because();
            }

        }

        public class when_calculating_sum : concern
        {
            private string the_input_string;
            private int the_result;


            protected override void Because()
            {
                the_result = SUT.Add(the_input_string);
            }

            public class and_input_string_is_empty : when_calculating_sum
            {
                protected override void Establish_context()
                {
                    base.Establish_context();
                    the_input_string = string.Empty;
                }

                [Test]
                public void it_should_return_0()
                {
                    the_result.Should().Be(0);
                }

            }

            public class and_input_consists_of_a_single_number : when_calculating_sum
            {
                protected override void Establish_context()
                {
                    the_input_string = "1";
                    base.Establish_context();
                }

                [Test]
                public void it_should_return_1()
                {
                    the_result.Should().Be(1);

                }
            }

        }
    }
}
