using KeePassLib.Cryptography.KeyDerivation;

namespace KeePassChallenge {
	internal static class Extensions {
		public static byte[] GetSeed(this KdfEngine kdf, KdfParameters p) {
			var getSeed = new GetSeedClass { Par = p };
			return getSeed.Do((dynamic)kdf);
		}

		private class GetSeedClass {
			public KdfParameters Par { get; set; }

			public byte[] Do(KdfEngine kdf) {
				return null;
			}

			public byte[] Do(Argon2Kdf kdf) {
				return Par.GetByteArray(Argon2Kdf.ParamSalt);
			}

			public byte[] Do(AesKdf kdf) {
				return Par.GetByteArray(AesKdf.ParamSeed);
			}
		}
	}
}