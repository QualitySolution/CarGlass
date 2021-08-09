using System;
using QS.Updater.DB;

namespace CarGlass
{
	partial class MainClass
	{
		public static UpdateConfiguration MakeUpdateConfiguration() {
			var configuration = new UpdateConfiguration();

			//Настраиваем обновления
			configuration.AddUpdate(
				new Version(1, 3),
				new Version(1, 4),
				"CarGlass.Updates.1.4.sql");

			configuration.AddMicroUpdate(
				new Version(1, 4),
				new Version(1, 4, 2),
				"CarGlass.Updates.1.4.2.sql");

			configuration.AddUpdate(
				new Version(1, 4),
				new Version(1, 5),
				"CarGlass.Updates.1.5.sql");

			configuration.AddMicroUpdate(
				new Version(1, 5),
				new Version(1, 5, 1),
				"CarGlass.Updates.1.5.1.sql");

			configuration.AddMicroUpdate(
				new Version(1, 5, 1),
				new Version(1, 5, 2),
				"CarGlass.Updates.1.5.2.sql");

			configuration.AddUpdate(
				new Version(1, 5),
				new Version(1, 6),
				"CarGlass.Updates.1.6.sql");

			configuration.AddUpdate(
				new Version(1, 6),
				new Version(1, 7),
				"CarGlass.Updates.1.7.sql");

			configuration.AddMicroUpdate(
				new Version(1, 7),
				new Version(1, 7, 3),
				"CarGlass.Updates.1.7.3.sql");

			configuration.AddUpdate(
				new Version(1, 7, 3),
				new Version(1, 8),
				"CarGlass.Updates.1.8.sql");

			configuration.AddMicroUpdate(
				new Version(1, 8),
				new Version(1, 8, 2),
				"CarGlass.Updates.1.8.2.sql");

			return configuration;
		}
	}
}
