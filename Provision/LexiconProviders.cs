using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gramma.GenericContentModel;
using Gramma.LanguageModel.Provision;

namespace Gramma.Lexica.Provision
{
	/// <summary>
	/// Read-only collection of <see cref="LexiconProvider"/> elements,
	/// keyed by <see cref="LanguageFacet.LanguageProvider"/>,
	/// suitable for XAML.
	/// </summary>
	[Serializable]
	public class LexiconProviders : ReadOnlyMultiMap<LanguageProvider, LexiconProvider>
	{
	}
}
