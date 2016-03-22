using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gramma.LanguageModel.TrainingSources;

namespace Gramma.Lexica.Sources
{
	/// <summary>
	/// Root of a definition of <see cref="TrainingSource{Lemma}"/> sources
	/// required to import a <see cref="Lexicon"/>.
	/// </summary>
	public class LexiconSourceSet : SourceSet
	{
		#region Private fields

		private string name, description;

		#endregion

		#region Construction

		/// <summary>
		/// Create.
		/// </summary>
		public LexiconSourceSet()
		{
			name = String.Empty;
			description = String.Empty;
		}

		#endregion

		#region Public properties

		/// <summary>
		/// The root of the <see cref="TrainingSource{Lemma}"/> sources.
		/// </summary>
		public LexiconSources Sources { get; } = new LexiconSources();

		/// <summary>
		/// The name of the <see cref="Lexicon"/> to be imported.
		/// </summary>
		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				if (value == null) throw new ArgumentNullException(nameof(value));

				name = value;
			}
		}

		/// <summary>
		/// The description of the <see cref="Lexicon"/> to be imported.
		/// </summary>
		public string Description
		{
			get
			{
				return description;
			}
			set
			{
				if (value == null) throw new ArgumentNullException(nameof(value));

				description = value;
			}
		}

		#endregion

		#region Public methods

		/// <summary>
		/// Assigns the the language provider to the <see cref="Sources"/> recursively.
		/// </summary>
		public override void AssignLanguageProviderToSources()
		{
			AssignLanguageProviderToSources(this.Sources);
		}

		/// <summary>
		/// Import the lexicon specified in the <see cref="Sources"/>.
		/// </summary>
		/// <returns>Returns a task whose result contains the lexicon.</returns>
		public Task<Lexicon> ImportLexiconAsync()
		{
			if (this.LanguageProvider == null) throw new LexiconException("LanguageProvider has not been set for the lexicon set.");

			return Task.Factory.StartNew(() =>
			{
				var lexicon = new Lexicon(this.LanguageProvider, this.Name, this.Description);

				foreach (var source in Sources)
				{
					using (source)
					{
						source.Open();

						foreach (var lemma in source.GetData())
						{
							lexicon.Add(lemma);
						}
					}
				}

				return lexicon;
			},
			TaskCreationOptions.LongRunning);

		}

		#endregion
	}
}
