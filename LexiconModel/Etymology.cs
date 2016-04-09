using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grammophone.Lexica.LexiconModel
{
	/// <summary>
	/// Etymology of a lemma.
	/// </summary>
	[Serializable]
	public class Etymology
	{
		#region Construction

		/// <summary>
		/// Create.
		/// </summary>
		/// <param name="description">The description of the etymology.</param>
		/// <param name="reference">The optional reference to another lemma, or null.</param>
		public Etymology(string description, string reference = null)
		{
			if (description == null) throw new ArgumentNullException(nameof(description));

			this.Description = description;
			this.Reference = reference;
		}

		#endregion

		#region Public properties

		/// <summary>
		/// The description of the etymology.
		/// </summary>
		public string Description { get; private set; }

		/// <summary>
		/// The optional reference to another lemma, or null.
		/// </summary>
		public string Reference { get; private set; }

		#endregion
	}
}
