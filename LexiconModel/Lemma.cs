using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gramma.Lexica.LexiconModel
{
	/// <summary>
	/// A lemma of a dictionary, a key.
	/// </summary>
	[Serializable]
	public class Lemma
	{
		#region Private fields

		private static string[] emptyNotes = new string[0];

		#endregion

		#region Construction

		/// <summary>
		/// Create.
		/// </summary>
		/// <param name="key">The key of the lemma.</param>
		/// <param name="form">The form of the lemma.</param>
		/// <param name="senses">The senses of the lemma.</param>
		/// <param name="notes">Optional notes for the lemma.</param>
		/// <param name="etymology">Optional etymology of the lemma.</param>
		public Lemma(
			string key,
			string form, 
			IReadOnlyList<Sense> senses, 
			IReadOnlyList<string> notes = null, 
			Etymology etymology = null)
		{
			if (key == null) throw new ArgumentNullException(nameof(key));
			if (form == null) throw new ArgumentNullException(nameof(form));
			if (senses == null) throw new ArgumentNullException(nameof(senses));

			if (notes == null) notes = emptyNotes;

			this.Key = key;
			this.Form = form;
			this.Senses = senses;
			this.Notes = notes ?? emptyNotes;
			this.Etymology = etymology;
		}

		#endregion

		#region Public properties

		/// <summary>
		/// The key of the lemma.
		/// </summary>
		public string Key { get; private set; }

		/// <summary>
		/// The form of the lemma.
		/// </summary>
		public string Form { get; private set; }

		/// <summary>
		/// The senses of the lemma.
		/// </summary>
		public IReadOnlyList<Sense> Senses { get; private set; }

		/// <summary>
		/// The notes for the lemma.
		/// </summary>
		public IReadOnlyList<string> Notes { get; private set; }

		/// <summary>
		/// Optional etymology of the lemma, or null.
		/// </summary>
		public Etymology Etymology { get; private set; }

		#endregion
	}
}
