using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FirstWave.Niot.Game
{
	public static class Constants
	{
		public static class Databases
		{
			public const string DATABASE_PATH = "Databases";
			public const string ABILITY_DB_NAME = "AbilityDatabase.asset";

			public static string GetPathForDb(string dbName)
			{
				return string.Format("Assets/{0}/{1}", DATABASE_PATH, dbName);
			}
		}

		public static class Ranges
		{
			public const int FIELD_EFFECT_SIZE = 7;
			public const int STANDARD_ABILITY_SIZE = 4;
			public const int FINISHER_ABILITY_SIZE = 2;
		}
	}
}
