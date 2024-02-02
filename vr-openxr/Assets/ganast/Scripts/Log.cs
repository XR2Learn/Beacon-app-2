using UnityEngine;

namespace com.ganast.log.unity {

	public class Log {

		public static void Message(object context, string scope, string message = null) {
			string s = "[" + context.GetType().Name + "] " + scope;
			if (message != null) {
				s += ", " + message;
			}
			Debug.Log(s);
		}
	}
}
