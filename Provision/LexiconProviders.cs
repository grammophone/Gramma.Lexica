using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grammophone.GenericContentModel;
using Grammophone.LanguageModel.Provision;

namespace Grammophone.Lexica.Provision
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
