using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants {
    // Camera Values
    public static class CameraStats {
        public static float C_cameraMoveSpeed = 20.0f;
    }

    public static class Global {
        public enum C_CoverTypeAndDirection { FN, FS, FE, FW, HN, HS, HE, HW };
        public enum C_CoverType { HALF, WHOLE, NONE };
    }
		
	// Weapon Values
	public static class WeaponStats {
		public static class PistolStats {
			public static int maxShots = 6;
			public static int range = 8;
			public static int damage = 12;
			public static int moveRange = 6;
		}
		public static class ShotgunStats {
			public static int maxShots = 2;
			public static int range = 5;
			public static int damage = 34;
			public static int moveRange = 7;
		}
		public static class RifleStats {
			public static int maxShots = 1;
			public static int range = 10;
			public static int damage = 40;
			public static int moveRange = 5;
		}
	}

    public static class WinScreen
    {
        public static string C_WinText = "";
    }
}
