using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grammophone.Lexica.LexiconModel
{
	/// <summary>
	/// A sense of a lemma.
	/// </summary>
	[Serializable]
	public class Sense
	{
		#region Provate fields

		private static IReadOnlyList<Sense> emptySubsenses = new Sense[0];

		#endregion

		#region Construction

		/// <summary>
		/// Create.
		/// </summary>
		/// <param name="description">The sense's description.</param>
		/// <param name="label">An optional label for the sense, to aid numbering, or null.</param>
		/// <param name="subsenses">Optional list of subsenses.</param>
		public Sense(string description, string label = null, IReadOnlyList<Sense> subsenses = null)
		{
			if (description == null) throw new ArgumentNullException(nameof(description));

			this.Description = description;
			this.Label = label ?? String.Empty;
			this.Subsenses = subsenses ?? emptySubsenses;
		}

		#endregion

		#region Public properties

		/// <summary>
		/// The sense's description.
		/// </summary>
		public string Description { get; private set; }

		/// <summary>
		/// An optional label for the sense, to aid numbering, or empty string.
		/// </summary>
		public string Label { get; private set; }

		/// <summary>
		/// Optional reference to another <see cref="Lemma"/> via its <see cref="Lemma.Key"/>.
		/// </summary>
		public string Reference { get; private set; }

		/// <summary>
		/// List of subsenses.
		/// </summary>
		public IReadOnlyList<Sense> Subsenses { get; private set; }

		#endregion
	}
}
