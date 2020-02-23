using System;
using System.Globalization;
using System.Threading;

namespace WizardPlatformer {
	public static class Program {

		[STAThread]
		static void Main() {
			Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-us");

			using (var game = new WizardPlatformer())
				game.Run();
		}
	}
}
