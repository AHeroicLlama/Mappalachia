using Library;

namespace Tests
{
	[TestClass]
	public class LibraryHelperTests
	{
		[TestMethod]
		public void WithoutTrailingSlashTest()
		{
			string back = @"string\\\";
			string fwd = @"string///";
			string both = @"string//\\//\\";
			string mixed = @"\/a\/\/b\/\/";

			back = back.WithoutTrailingSlash();
			fwd = fwd.WithoutTrailingSlash();
			both = both.WithoutTrailingSlash();
			mixed = mixed.WithoutTrailingSlash();

			Assert.AreEqual("string", back);
			Assert.AreEqual("string", fwd);
			Assert.AreEqual("string", both);
			Assert.AreEqual(@"\/a\/\/b", mixed);
		}

		[TestMethod]
		public void ToSqliteCollectionTest()
		{
#pragma warning disable IDE0300, IDE0301
			string[] arrayOfNone = Array.Empty<string>();
			string[] arrayOfOne = { "alpha" };
			string[] arrayOfMany = { "alpha", "bravo,", "charlie, " };
			List<string> listOfMany = new List<string>() { "alpha", "bravo", "charlie" };
#pragma warning restore IDE0300, IDE0301

			string arrayOfNoneConverted = arrayOfNone.ToSqliteCollection();
			string arrayOfOneConverted = arrayOfOne.ToSqliteCollection();
			string arrayOfManyConverted = arrayOfMany.ToSqliteCollection();
			string listOfManyConverted = listOfMany.ToSqliteCollection();

			Assert.AreEqual("()", arrayOfNoneConverted);
			Assert.AreEqual("('alpha')", arrayOfOneConverted);
			Assert.AreEqual("('alpha','bravo,','charlie, ')", arrayOfManyConverted);
			Assert.AreEqual("('alpha','bravo','charlie')", listOfManyConverted);
		}

		[TestMethod]
		public void PluralizeTest()
		{
			List<object> empty = new List<object>() { };
			List<object> one = new List<object>() { new object() };
			List<object> many = new List<object>() { new object(), new object(), new object(), new object() };

			string emptyPlural = Common.Pluralize(empty);
			string onePlural = Common.Pluralize(one);
			string manyPlural = Common.Pluralize(many);

			Assert.AreEqual("s", emptyPlural);
			Assert.AreEqual(onePlural, string.Empty);
			Assert.AreEqual("s", manyPlural);
		}

		[TestMethod]
		public void ToHexTest()
		{
			uint zero = uint.MinValue;
			uint max = uint.MaxValue;
			uint appalachia = 2480661;

			string zeroHex = zero.ToHex();
			string maxHex = max.ToHex();
			string appalachiaHex = appalachia.ToHex();

			Assert.AreEqual("00000000", zeroHex);
			Assert.AreEqual("FFFFFFFF", maxHex);
			Assert.AreEqual("0025DA15", appalachiaHex);
		}
	}
}