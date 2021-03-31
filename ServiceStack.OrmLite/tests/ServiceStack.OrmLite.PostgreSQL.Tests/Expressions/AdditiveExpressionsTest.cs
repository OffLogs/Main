﻿using NUnit.Framework;
using ServiceStack.OrmLite.Tests;

namespace ServiceStack.OrmLite.PostgreSQL.Tests.Expressions
{
    [TestFixtureOrmLiteDialects(Dialect.AnyPostgreSql)]
    public class AdditiveExpressionsTest : ExpressionsTestBase
    {
        public AdditiveExpressionsTest(DialectContext context) : base(context) {}

        [Test]
        public void Can_select_constant_add_expression()
        {
            var expected = new TestType()
            {
                IntColumn = 7,
                BoolColumn = true,
                StringColumn = "test"
            };

            EstablishContext(10, expected);


            using (var con =  OpenDbConnection())
            {
                var actual = con.Select<TestType>(q => q.IntColumn == 4 + 3);

                Assert.IsNotNull(actual);
                Assert.AreEqual(1, actual.Count);
                CollectionAssert.Contains(actual, expected); 
            }
        }

        [Test]
        public void Can_select_constant_subtract_expression()
        {
            var expected = new TestType()
            {
                IntColumn = 7,
                BoolColumn = true,
                StringColumn = "test"
            };

            EstablishContext(10, expected);

            using (var con = OpenDbConnection())
            {
                var actual = con.Select<TestType>(q => q.IntColumn == 10 - 3);

                Assert.IsNotNull(actual);
                Assert.AreEqual(1, actual.Count);
                CollectionAssert.Contains(actual, expected);
            }
        }

        [Test]
        public void Can_select_variable_add_expression()
        {
// ReSharper disable ConvertToConstant.Local
            var a = 4;
            var b = 3;
// ReSharper restore ConvertToConstant.Local

            var expected = new TestType()
            {
                IntColumn = 7,
                BoolColumn = true,
                StringColumn = "test"
            };

            EstablishContext(10, expected);

            using (var con = OpenDbConnection())
            {
                var actual = con.Select<TestType>(q => q.IntColumn == a + b);

                Assert.IsNotNull(actual);
                Assert.AreEqual(1, actual.Count);
                CollectionAssert.Contains(actual, expected);
            }
        }

        [Test]
        public void Can_select_variable_subtract_expression()
        {
            // ReSharper disable ConvertToConstant.Local
            var a = 10;
            var b = 3;
            // ReSharper restore ConvertToConstant.Local

            var expected = new TestType()
            {
                IntColumn = 7,
                BoolColumn = true,
                StringColumn = "test"
            };

            EstablishContext(10, expected);

            using (var con = OpenDbConnection())
            {
                var actual = con.Select<TestType>(q => q.IntColumn == a - b);

                Assert.IsNotNull(actual);
                Assert.AreEqual(1, actual.Count);
                CollectionAssert.Contains(actual, expected);
            }
        }

        [Test]
        public void Can_select_method_add_expression()
        {
            var expected = new TestType()
            {
                IntColumn = 7,
                BoolColumn = true,
                StringColumn = "test"
            };

            EstablishContext(10, expected);

            using (var con = OpenDbConnection())
            {
                var actual = con.Select<TestType>(q => q.IntColumn == GetValue(4) + GetValue(3));

                Assert.IsNotNull(actual);
                Assert.AreEqual(1, actual.Count);
                CollectionAssert.Contains(actual, expected);
            }
        }

        [Test]
        public void Can_select_method_subtract_expression()
        {
            var expected = new TestType()
            {
                IntColumn = 7,
                BoolColumn = true,
                StringColumn = "test"
            };

            EstablishContext(10, expected);

            using (var con = OpenDbConnection())
            {
                var actual = con.Select<TestType>(q => q.IntColumn == GetValue(10) - GetValue(3));

                Assert.IsNotNull(actual);
                Assert.AreEqual(1, actual.Count);
                CollectionAssert.Contains(actual, expected);
            }
        }
    }
}