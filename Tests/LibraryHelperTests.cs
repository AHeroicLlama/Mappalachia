using Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

			Assert.AreEqual(back, "string");
			Assert.AreEqual(fwd, "string");
			Assert.AreEqual(both, "string");
			Assert.AreEqual(mixed, @"\/a\/\/b");
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

			Assert.AreEqual(arrayOfNoneConverted, "()");
			Assert.AreEqual(arrayOfOneConverted, "('alpha')");
			Assert.AreEqual(arrayOfManyConverted, "('alpha','bravo,','charlie, ')");
			Assert.AreEqual(listOfManyConverted, "('alpha','bravo','charlie')");
		}

		[TestMethod]
		public void StdDevTest()
		{
			List<double> collection = new List<double>() { -5, 0, 0, -1, 60, Math.PI };
			List<double> balanced = new List<double>() { 1, 0, 1, 0 };

			double stdCollection = collection.StdDev();
			double stdBalanced = balanced.StdDev();

			Assert.AreEqual(stdCollection, 22.699765059593, 0.001);
			Assert.AreEqual(stdBalanced, 0.5);
		}

		[TestMethod]
		public void PluralizeTest()
		{
			List<object> empty = new List<object>() { };
			List<object> one = new List<object>() { new object() };
			List<object> many = new List<object>() { new object(), new object(), new object(), new object() };

			string emptyPlural = Misc.Pluralize(empty);
			string onePlural = Misc.Pluralize(one);
			string manyPlural = Misc.Pluralize(many);

			Assert.AreEqual(emptyPlural, "s");
			Assert.AreEqual(onePlural, string.Empty);
			Assert.AreEqual(manyPlural, "s");
		}

		[TestMethod]
		public void ToHexTest()
		{
			uint zero = uint.MinValue;
			uint max = uint.MaxValue;
			uint appalachia = 2480661;

			string zeroHex = zero.ToHex();
			string maxHex = max.ToHex();
			string appalchiaHex = appalachia.ToHex();

			Assert.AreEqual(zeroHex, "00000000");
			Assert.AreEqual(maxHex, "FFFFFFFF");
			Assert.AreEqual(appalchiaHex, "0025DA15");
		}

		[TestMethod]
		public void MiscTest()
		{
			Assert.AreEqual(Misc.Kilobyte, 1024);
		}
	}
}