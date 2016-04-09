using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grammophone.Lexica.LexiconModel
{
	/// <summary>
	/// Result of a lexicon query.
	/// </summary>
	[Serializable]
	public struct LemmaResult
	{
		#region Construction

		/// <summary>
		/// Create.
		/// </summary>
		/// <param name="lemma">The <see cref="LexiconModel.Lemma"/> found.</param>
		/// <param name="distance">The edit distance of the found <see cref="LexiconModel.Lemma"/> from the queried form.</param>
		public LemmaResult(Lemma lemma, double distance)
		{
			if (lemma == null) throw new ArgumentNullException(nameof(lemma));

			this.Lemma = lemma;
			this.Distance = distance;
		}

		#endregion

		#region Public properties

		/// <summary>
		/// The <see cref="LexiconModel.Lemma"/> found.
		/// </summary>
		public Lemma Lemma { get; private set; }

		/// <summary>
		/// The edit distance of the found <see cref="LexiconModel.Lemma"/> from the queried form.
		/// </summary>
		public double Distance { get; private set; }

		#endregion
	}
}
