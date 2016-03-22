using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gramma.Configuration;
using Gramma.LanguageModel.Provision;
using Gramma.Lexica.Provision;
using Gramma.Lexica.Sources;

namespace Gramma.Lexica.Configuration
{
	/// <summary>
	/// Intended to be the root of the XAML configuration.
	/// </summary>
	public class LexicaSetup : Gramma.Configuration.IXamlLoadListener
	{
		#region Public properties

		/// <summary>
		/// The available language providers.
		/// </summary>
		public LanguageProviders LanguageProviders { get; } = new LanguageProviders();

		/// <summary>
		/// The lexicon providers, indexed by <see cref="LanguageProvider"/>.
		/// </summary>
		public LexiconProviders LexiconProviders { get; } = new LexiconProviders();

		/// <summary>
		/// The lexicon providers, indexed by <see cref="LanguageProvider"/>.
		/// </summary>
		public LexiconSourceSets LexiconSourceSets { get; } = new LexiconSourceSets();

		#endregion

		#region IXamlLoadListener methods

		void IXamlLoadListener.OnPostLoad(object sender)
		{
			foreach (var lexiconSourceSet in this.LexiconSourceSets)
			{
				if (lexiconSourceSet.LanguageProvider == null)
					throw new LexiconException("The lexicon set has no LanguageProvider specified.");

				lexiconSourceSet.AssignLanguageProviderToSources();
			}
		}

		#endregion
	}
}
